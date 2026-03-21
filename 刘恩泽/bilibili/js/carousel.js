document.addEventListener('DOMContentLoaded', function() {
    const carouselList = document.querySelector('.carousel-area .carousel-list');
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    const indicatorItems = document.querySelectorAll('.indicator li');
    const carouselArea = document.querySelector('.carousel-area');

    if (!carouselList) return;
    const originalItems = Array.from(carouselList.children);
    const realCount = originalItems.length; 

    const firstClone = originalItems[0].cloneNode(true);
    const lastClone = originalItems[realCount - 1].cloneNode(true);
    carouselList.appendChild(firstClone);
    carouselList.insertBefore(lastClone, originalItems[0]);

    const totalCount = carouselList.children.length; 
    const singleWidth = 100 / totalCount;
    
    carouselList.style.width = `${totalCount * 100}%`; 
    Array.from(carouselList.children).forEach(item => {
        item.style.width = `${singleWidth}%`;
    });

    let currentIndex = 1; 
    let autoPlayTimer = null;
    const autoPlayInterval = 3000; 
    let isAnimating = false; 

    updateCarousel(true);

    function updateCarousel(withoutTransition = false) {
        if (withoutTransition) {
            carouselList.style.transition = 'none';
        } else {
            carouselList.style.transition = 'transform 0.5s ease-in-out';
        }

        carouselList.style.transform = `translateX(-${currentIndex * singleWidth}%)`;
        updateIndicators();
    }


    function updateIndicators() {
        indicatorItems.forEach(li => li.classList.remove('active'));
        let realIndex = currentIndex - 1;
        if (realIndex < 0) realIndex = realCount - 1;
        if (realIndex >= realCount) realIndex = 0;
        if (indicatorItems[realIndex]) {
            indicatorItems[realIndex].classList.add('active');
        }
    }

    function goToNext() {
        if (isAnimating) return;
        isAnimating = true;
        currentIndex++;
        updateCarousel(false); 
        if (currentIndex === totalCount - 1) {
            setTimeout(() => {
                currentIndex = 1; 
                updateCarousel(true); 
                isAnimating = false; 
            }, 500);
        } else {
            setTimeout(() => isAnimating = false, 500);
        }
    }

    function goToPrev() {
        if (isAnimating) return;
        isAnimating = true;
        currentIndex--;
        updateCarousel(false);
        if (currentIndex === 0) {
            setTimeout(() => {
                currentIndex = realCount; 
                updateCarousel(true); 
                isAnimating = false;
            }, 500);
        } else {
            setTimeout(() => isAnimating = false, 500);
        }
    }
    nextBtn.addEventListener('click', goToNext);
    prevBtn.addEventListener('click', goToPrev);
    indicatorItems.forEach((li, index) => {
        li.addEventListener('click', () => {
            if (isAnimating) return;
            currentIndex = index + 1; 
            updateCarousel(false);
        });
    });

    carouselArea.addEventListener('mouseenter', () => {
        if (autoPlayTimer) clearInterval(autoPlayTimer);
    });
    carouselArea.addEventListener('mouseleave', () => {
        autoPlayTimer = setInterval(goToNext, autoPlayInterval);
    });

    autoPlayTimer = setInterval(goToNext, autoPlayInterval);
});