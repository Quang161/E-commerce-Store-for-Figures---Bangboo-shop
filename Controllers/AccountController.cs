using Bangboo_E_Commerce.Entities;
using Microsoft.AspNetCore.Mvc;
using Bangboo_E_Commerce.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
namespace Bangboo_E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly BangbooShopContext _context;

        public AccountController(BangbooShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.PhaethonUsers
                    .FirstOrDefaultAsync(u => u.IdPhaethon == model.IdPhaethon || u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User name or email is already taken.");
                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Password and confirm password do not match.");
                    return View(model);
                }

                var passwordHasher = new PasswordHasher<string>();
                string hashedPassword = passwordHasher.HashPassword(model.IdPhaethon, model.Password);

                var newUser = new PhaethonUser
                {
                    IdPhaethon = model.IdPhaethon,
                    FullNamePhatheon = model.FullNamePhatheon,
                    Email = model.Email,
                    Password = hashedPassword,
                    Address = model.Address,
                    CreateAt = DateTime.UtcNow
                };

                _context.PhaethonUsers.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.PhaethonUsers
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<string>();
                    var verificationResult = passwordHasher.VerifyHashedPassword(user.IdPhaethon, user.Password, model.Password);

                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("UserId", user.IdPhaethon.ToString());
                        HttpContext.Session.SetString("UserName", user.FullNamePhatheon);

                        var sessionValue = HttpContext.Session.GetString("UserId");
                        Console.WriteLine($"Session set: {sessionValue}");
                        Console.WriteLine("Redirecting to Bangboo page...");

                        return RedirectToAction("Cart", "Bangboo");
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Product", "Bangboo");
        }

        public IActionResult Subscribe()
        {
            return View();
        }
    }
}
