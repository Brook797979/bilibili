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

    // ======== 4. 竖屏短片区域左右滑动交互 ========
    const shortsTrack = document.getElementById('shortsTrack');
    const prevBtn = document.querySelector('.shorts-prev');
    const nextBtn = document.querySelector('.shorts-next');

    if (shortsTrack && prevBtn && nextBtn) {
        // 点击右箭头：向右滚动
        nextBtn.addEventListener('click', () => {
            // 获取单张卡片的宽度
            const cardWidth = shortsTrack.querySelector('.short-card').offsetWidth;
            const gap = 20; // 对应 CSS 中的 gap 值
            // 每次滚动 两张卡片 的距离，提升效率
            shortsTrack.scrollBy({ left: (cardWidth + gap) * 2, behavior: 'smooth' });
        });

        // 点击左箭头：向左滚动
        prevBtn.addEventListener('click', () => {
            const cardWidth = shortsTrack.querySelector('.short-card').offsetWidth;
            const gap = 20;
            shortsTrack.scrollBy({ left: -((cardWidth + gap) * 2), behavior: 'smooth' });
        });
        
        // // 进阶优化：监听滚动位置，当滚到最左边或最右边时，隐藏对应的箭头
        // shortsTrack.addEventListener('scroll', () => {
        //     // 如果滚到了最左边
        //     if (shortsTrack.scrollLeft <= 0) {
        //         prevBtn.style.opacity = '0.3';
        //         prevBtn.style.cursor = 'not-allowed';
        //     } else {
        //         prevBtn.style.opacity = '1';
        //         prevBtn.style.cursor = 'pointer';
        //     }
            
        //     // 如果滚到了最右边 (scrollWidth 减去 clientWidth 就是最大可滚动距离)
        //     if (shortsTrack.scrollLeft + shortsTrack.clientWidth >= shortsTrack.scrollWidth - 1) {
        //         nextBtn.style.opacity = '0.3';
        //         nextBtn.style.cursor = 'not-allowed';
        //     } else {
        //         nextBtn.style.opacity = '1';
        //         nextBtn.style.cursor = 'pointer';
        //     }
        // });
        
        // 初始化时触发一次判断，把左侧箭头变灰
        shortsTrack.dispatchEvent(new Event('scroll'));
    }
});
