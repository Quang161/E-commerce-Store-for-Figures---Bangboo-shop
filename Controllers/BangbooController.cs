using Bangboo_E_Commerce.Entities;
using Bangboo_E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Web.Helpers;
using YourNamespace.Models;

public class BangbooController : Controller
{
    private readonly BangbooShopContext _context;
    private readonly ILogger<BangbooController> _logger;

    public BangbooController(BangbooShopContext context, ILogger<BangbooController> logger)
    {
        _context = context;
        _logger = logger;
    }
    public IActionResult Product()
    {
        var bangbooImages = _context.BangbooImageDbs
            .Select(img => new BangbooImage_Code
            {
                IDImage = img.IDImage,
                URL = img.Url
            })
            .ToList();

        return View(bangbooImages);
    }
    public IActionResult ProductInfo(string idImage)
    {
        var product = _context.BangbooDbs
            .Where(p => _context.BangbooImageDbs
            .Any(img => img.IDImage == idImage && img.IDImage_Bangboo == p.Idbangboo))
            .Select(p => new BangbooDetailsViewModel
            {
                IDImage_Bangboo = p.Idbangboo,
                ProductId = p.Idbangboo,
                Name = p.NameBangboo,
                Description = p.Description,
                Rank = p.Rank,
                Attribute = p.Attribute,
                Price = p.Price,
                ImageUrl = _context.BangbooImageDbs
                    .Where(img => img.IDImage == idImage)
                    .Select(img => img.Url)
                    .FirstOrDefault()
            })
            .FirstOrDefault();

        if (product == null)
        {
            return NotFound("Bangboo không tồn tại.");
        }

        return View("BangbooDetailsView", product);
    }

    [HttpPost]
    public IActionResult AddToCart(string productId, int quantity)
    {
        var product = _context.BangbooDbs
            .Where(p => p.Idbangboo.ToString() == productId)
            .Select(p => new CartItem
            {
                ProductId = p.Idbangboo.ToString(),
                Name = p.NameBangboo,
                Price = p.Price,
                ImageUrl = _context.BangbooImageDbs
                    .Where(img => img.IDImage_Bangboo == p.Idbangboo)
                    .Select(img => img.Url)
                    .FirstOrDefault(),
                Quantity = quantity
            })
            .FirstOrDefault();

        if (product == null)
        {
            return Json(new { success = false, message = "Product not found." });
        }

        var cart = HttpContext.Session.GetString("Cart");
        List<CartItem> cartItems;

        if (string.IsNullOrEmpty(cart))
        {
            cartItems = new List<CartItem>();
        }
        else
        {
            cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        var existingItem = cartItems.FirstOrDefault(c => c.ProductId == product.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += product.Quantity;
        }
        else
        {
            cartItems.Add(product);
        }
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

        string userId = HttpContext.Session.GetString("UserId");
        if (!string.IsNullOrEmpty(userId))
        {
            var dbCartItem = _context.CartBoos
                .FirstOrDefault(c => c.IdPhatheon == userId && c.IdBangbooDb == product.ProductId);

            if (dbCartItem != null)
            {
                dbCartItem.Quantity += product.Quantity;
            }
            else
            {
                dbCartItem = new CartBoo
                {
                    IdPhatheon = userId,
                    IdBangbooDb = product.ProductId,
                    Quantity = product.Quantity
                };
                _context.CartBoos.Add(dbCartItem);
            }
            _context.SaveChanges();
        }

        return StatusCode(204);
    }

    public IActionResult GetCart()
    {
        var userId = HttpContext.Session.GetString("UserId");

        if (userId != null)
        {
            var cartItems = _context.CartBoos
                .Where(c => c.IdPhatheon == userId)
                .Include(c => c.IdBangbooDb)
                .ToList();

            return Json(cartItems);
        }

        return Json(new List<CartItem>());
    }

    public IActionResult Cart()
    {
        string userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = _context.CartBoos
            .Where(c => c.IdPhatheon == userId)
            .Select(c => new CartItem
            {
                ProductId = c.IdBangbooDb,
                Name = c.IdbangbooDbNavigation.NameBangboo,
                Price = c.IdbangbooDbNavigation.Price,
                Quantity = c.Quantity,
                ImageUrl = c.IdbangbooDbNavigation.BangbooImageDbs.FirstOrDefault().Url
            })
            .ToList();

        var totalPrice = cartItems.Sum(item => item.Price * item.Quantity);

        ViewBag.CartItems = cartItems;
        ViewBag.TotalPrice = totalPrice;

        return View();
    }

    public class UpdateQuantityModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCartQuantity([FromBody] UpdateQuantityModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.ProductName) || model.Quantity < 1)
        {
            return BadRequest("Invalid data.");
        }

        string userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var cartItem = _context.CartBoos.FirstOrDefault(c => c.IdPhatheon == userId && c.IdbangbooDbNavigation.NameBangboo == model.ProductName);
        if (cartItem != null)
        {
            cartItem.Quantity = model.Quantity;
            _context.SaveChanges();
        }

        var cartSession = HttpContext.Session.GetString("Cart");
        if (!string.IsNullOrEmpty(cartSession))
        {
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
            var itemToUpdate = cartItems.FirstOrDefault(c => c.Name == model.ProductName);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = model.Quantity;
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
            }
        }

        var updatedCartItem = _context.CartBoos
            .Where(c => c.IdPhatheon == userId && c.IdbangbooDbNavigation.NameBangboo == model.ProductName)
            .Select(c => new
            {
                productId = c.IdBangbooDb,
                newTotal = c.Quantity * c.IdbangbooDbNavigation.Price
            })
            .FirstOrDefault();

        var totalPrice = _context.CartBoos
            .Where(c => c.IdPhatheon == userId)
            .Sum(c => c.Quantity * c.IdbangbooDbNavigation.Price);

        return Json(new
        {
            productId = updatedCartItem.productId,
            newTotal = updatedCartItem.newTotal,
            totalPrice = totalPrice
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFromCart([FromBody] string productName)
    {
        if (string.IsNullOrEmpty(productName))
        {
            return BadRequest("Product name is required.");
        }

        string userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var cartItem = _context.CartBoos.FirstOrDefault(c => c.IdPhatheon == userId && c.IdbangbooDbNavigation.NameBangboo == productName);
        if (cartItem != null)
        {
            _context.CartBoos.Remove(cartItem);
            _context.SaveChanges();
        }

        var cartSession = HttpContext.Session.GetString("Cart");
        if (!string.IsNullOrEmpty(cartSession))
        {
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
            var itemToRemove = cartItems.FirstOrDefault(c => c.Name == productName);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
            }
        }
        return Ok();
    }

    public IActionResult Checkout()
    {
        string userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = _context.CartBoos
            .Where(c => c.IdPhatheon == userId)
            .Select(c => new
            {
                ImageUrl = c.IdbangbooDbNavigation.BangbooImageDbs.FirstOrDefault().Url,
                c.Quantity,
                ProductId = c.IdBangbooDb,
                Name = c.IdbangbooDbNavigation.NameBangboo,
                Price = c.IdbangbooDbNavigation.Price,
                Total = c.Quantity * c.IdbangbooDbNavigation.Price
            })
            .ToList();

        ViewBag.CartItems = cartItems;

        ViewBag.TotalAmount = cartItems.Sum(c => c.Total);

        return View();
    }

    [HttpPost]
    public IActionResult ConfirmCheckout(string transactionId, string sellerInfo, string paymentMethod)
    {
        string userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = _context.CartBoos
         .Where(c => c.IdPhatheon == userId)
         .Select(c => new CartItem
         {
             ProductId = c.IdBangbooDb,
             Name = c.IdbangbooDbNavigation.NameBangboo,
             Price = c.IdbangbooDbNavigation.Price,
             Quantity = c.Quantity,
             ImageUrl = c.IdbangbooDbNavigation.BangbooImageDbs.FirstOrDefault().Url
         })
         .ToList();

        if (cartItems == null || !cartItems.Any())
        {
            return RedirectToAction("Cart");
        }

        var totalAmount = cartItems.Sum(c => c.Total);

        var order = new Boorder
        {
            IdPhaethon = userId,
            PaymentMethod = paymentMethod,
            TransactionId = transactionId,
            SellerInfo = sellerInfo,
            TotalAmount = totalAmount,
            CreatedDate = DateTime.Now,
        };

        _context.Boorders.Add(order);
        _context.SaveChanges();

        var orderDetails = cartItems.Select(item => new BoorderDetail
        {
            IDOrder_DB = order.Idorder,
            IDBangboo = item.ProductId,
            Quantity = item.Quantity,
            Unit_Price = (decimal)item.Price,
            Total_Price = (decimal)item.Price * item.Quantity
        }).ToList();

        _context.BoorderDetails.AddRange(orderDetails);
        _context.SaveChanges();

        var cartItemsToRemove = _context.CartBoos.Where(c => c.IdPhatheon == userId).ToList();
        _context.CartBoos.RemoveRange(cartItemsToRemove);
        _context.SaveChanges();

        HttpContext.Session.Remove("Cart");

        return RedirectToAction("ThankYou");
    }

    public IActionResult Order()
    {
        string userId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var orders = _context.Boorders
            .Where(o => o.IdPhaethon == userId)
            .OrderByDescending(o => o.Idorder)
            .ToList();

        return View(orders);
    }

    public IActionResult ThankYou()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetOrderDetails(int orderId)
    {
        var orderDetails = _context.BoorderDetails
            .Where(d => d.IDOrder_DB == orderId)
            .Select(d => new
            {
                transactionId = d.Boorder.TransactionId,
                sellerInfo = d.Boorder.SellerInfo,
                paymentMethod = d.Boorder.PaymentMethod,
                totalAmount = d.Quantity * d.Unit_Price,
                createdDate = d.Boorder.CreatedDate,
                productImage = d.Bangboo.BangbooImageDbs.FirstOrDefault().Url,
                productName = d.Bangboo.NameBangboo,
                quantity = d.Quantity,
                price = d.Unit_Price,
                total = d.Quantity * d.Unit_Price,
                rank = d.Bangboo.Rank,
                faction = d.Bangboo.Faction,
                attribute = d.Bangboo.Attribute
            })
            .ToList();

        return Json(orderDetails);
    }
}