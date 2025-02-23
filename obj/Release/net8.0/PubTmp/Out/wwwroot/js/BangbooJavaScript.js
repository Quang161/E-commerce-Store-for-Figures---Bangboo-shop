let slideIndex = 1;
let maxThumbnails = 10;
let currentStartIndex = 0;

document.addEventListener('DOMContentLoaded', () => {
    showSlides(slideIndex);
    updateThumbnails(currentStartIndex);

    const firstThumbnail = document.querySelector(".thumbnail-item");
    if (firstThumbnail) {
        firstThumbnail.classList.add("active-thumbnail");
    }
});
function plusSlides(n) {
    const thumbnails = document.querySelectorAll(".thumbnail-item");
    const totalThumbnails = thumbnails.length;

    currentStartIndex += n * maxThumbnails;

    if (currentStartIndex < 0) {
        currentStartIndex = Math.max(0, totalThumbnails - (totalThumbnails % maxThumbnails || maxThumbnails));
    } else if (currentStartIndex >= totalThumbnails) {
        currentStartIndex = 0;
    }

    updateThumbnails(currentStartIndex);
}

function currentSlide(n) {
    slideIndex = n;
    showSlides(slideIndex);
    updateThumbnails(currentStartIndex);

    const thumbnails = document.querySelectorAll(".thumbnail-item");
    thumbnails.forEach(thumbnail => thumbnail.classList.remove("active-thumbnail"));

    const currentThumbnail = thumbnails[slideIndex - 1];
    if (currentThumbnail) {
        currentThumbnail.classList.add("active-thumbnail");
    }

    const cardContainers = document.querySelectorAll('.card-container');
    cardContainers.forEach(cardContainer => cardContainer.classList.remove('show'));

    const targetCardContainer = cardContainers[slideIndex - 1];
    if (targetCardContainer) {
        setTimeout(() => {
            targetCardContainer.classList.add('show');
        }, 10);
    }

    const cartContainer = document.getElementById("cart-container");
    if (cartContainer) {
        cartContainer.style.display = "none";
    }
}

function showSlides(n) {
    let slides = document.getElementsByClassName("mySlides");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }

    for (let i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }

    if (slides[slideIndex - 1]) {
        slides[slideIndex - 1].style.display = "block";
    }
}

function updateThumbnails(startIndex) {
    const thumbnails = document.querySelectorAll(".thumbnail-item");

    thumbnails.forEach(thumbnail => {
        thumbnail.style.display = "none";
    });

    const endIndex = Math.min(startIndex + maxThumbnails, thumbnails.length);
    for (let i = startIndex; i < endIndex; i++) {
        thumbnails[i].style.display = "block";
    }
}


document.querySelectorAll('.thumbnail-item').forEach((thumbnail, index) => {
    thumbnail.addEventListener('click', () => {
        currentSlide(index + 1);
    });
});

document.querySelector(".prev").addEventListener("click", () => {
    plusSlides(-1);
});

document.querySelector(".next").addEventListener("click", () => {
    plusSlides(1);
});

document.addEventListener('click', function (event) {
    const cardContainers = document.querySelectorAll('.card-container');
    const thumbnails = document.querySelectorAll('.thumbnail-item');
    const activeThumbnail = document.querySelector('.thumbnail-item.active-thumbnail');
    const activeCardContainerIndex = [...thumbnails].indexOf(activeThumbnail);

    let clickedInsideThumbnail = false;
    thumbnails.forEach(thumbnail => {
        if (thumbnail.contains(event.target)) {
            clickedInsideThumbnail = true;
        }
    });

    let clickedInsideCardContainer = false;
    cardContainers.forEach(cardContainer => {
        if (cardContainer.contains(event.target)) {
            clickedInsideCardContainer = true;
        }
    });

    if (!clickedInsideThumbnail && !clickedInsideCardContainer) {
        cardContainers.forEach((cardContainer, index) => {
            if (index !== activeCardContainerIndex) {
                cardContainer.classList.remove('show');
            }
        });
    }
});

document.addEventListener('DOMContentLoaded', () => {
    const firstThumbnail = document.querySelector(".thumbnail-item");
    const cardContainer = document.querySelector(".card-container");
    const buyButtons = document.querySelectorAll(".buy-button");
    const cart = document.querySelector(".cart-header");
    const totalPriceElement = document.getElementById("total-price");

    if (firstThumbnail) {
        firstThumbnail.classList.add("active-thumbnail");
        if (cardContainer) {
            cardContainer.classList.add("show");
        }
    }

    slideIndex = 1;
    currentStartIndex = 0;
    showSlides(slideIndex);
    updateThumbnails(currentStartIndex);

    if (cart && totalPriceElement) {
        let totalPrice = 0;

        buyButtons.forEach(button => {
            button.addEventListener("click", (e) => {
                e.preventDefault();
                const card = button.closest(".card");
                const productPrice = parseInt(card.querySelector(".price").textContent, 10);

                totalPrice += productPrice;
                totalPriceElement.textContent = totalPrice;
            });
        });
    }
});

function getSelectedThumbnailImage() {
    const selectedThumbnail = document.querySelector(".thumbnail-container .active-thumbnail img");
    return selectedThumbnail ? selectedThumbnail.src : null;
}
