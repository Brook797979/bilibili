document.addEventListener('DOMContentLoaded', () => {
    const themeToggleBtn = document.querySelector('.theme-toggle');
    const body = document.body;
    themeToggleBtn.addEventListener('click', () => {
        body.classList.toggle('dark-theme');
        const isDark = body.classList.contains('dark-theme');
        themeToggleBtn.querySelector('.btn-text').textContent = isDark ? '日间' : '黑夜';
        themeToggleBtn.querySelector('span:first-child').textContent = isDark ? '☀️' : '🌙';
    });

    const searchContainer = document.querySelector('.center-search-container');
    const searchInput = document.querySelector('.search-input');
    if (searchInput && searchContainer) {
        searchInput.addEventListener('focus', () => {
            searchContainer.classList.add('is-active');
        });
        searchInput.addEventListener('blur', () => {
            setTimeout(() => {
                searchContainer.classList.remove('is-active');
            }, 200);
        });
    }
    
    const shortsTrack = document.getElementById('shortsTrack');
    const prevBtn = document.querySelector('.shorts-prev');
    const nextBtn = document.querySelector('.shorts-next');
    if (shortsTrack && prevBtn && nextBtn) {
        nextBtn.addEventListener('click', () => {
            const cardWidth = shortsTrack.querySelector('.short-card').offsetWidth;
            const gap = 20; 
            shortsTrack.scrollBy({ left: (cardWidth + gap) * 2, behavior: 'smooth' });
        });

        prevBtn.addEventListener('click', () => {
            const cardWidth = shortsTrack.querySelector('.short-card').offsetWidth;
            const gap = 20;
            shortsTrack.scrollBy({ left: -((cardWidth + gap) * 2), behavior: 'smooth' });
        });
        shortsTrack.dispatchEvent(new Event('scroll'));
    }
});
