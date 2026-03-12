document.addEventListener('DOMContentLoaded', () => {
    const themeToggleBtn = document.querySelector('.theme-toggle');
    const body = document.body;
    themeToggleBtn.addEventListener('click', () => {
        body.classList.toggle('dark-theme');
        const isDark = body.classList.contains('dark-theme');
        themeToggleBtn.querySelector('.btn-text').textContent = isDark ? '日间' : '黑夜';
        themeToggleBtn.querySelector('span:first-child').textContent = isDark ? '☀️' : '🌙';
    });
});

document.addEventListener('DOMContentLoaded', () => {
    // ======== 1. 搜索框灵动展开 & 热搜显示 ========
    const searchContainer = document.querySelector('.center-search-container');
    const searchInput = document.querySelector('.search-input');

    if (searchInput && searchContainer) {
        // 获得焦点时：添加 is-active 类（触发 CSS 变宽和显示下拉框）
        searchInput.addEventListener('focus', () => {
            searchContainer.classList.add('is-active');
        });

        // 失去焦点时：移除 is-active 类
        // 用 setTimeout 是为了防止点击下拉热词时，下拉框瞬间消失导致点不到
        searchInput.addEventListener('blur', () => {
            setTimeout(() => {
                searchContainer.classList.remove('is-active');
            }, 200);
        });
    }

    // ======== 2. 番剧时间线简单切换 (小点缀) ========
    // const timelineSpans = document.querySelectorAll('.anime-timeline span');
    // timelineSpans.forEach(span => {
    //     span.addEventListener('click', (e) => {
    //         // 移除所有 active
    //         timelineSpans.forEach(s => s.classList.remove('active'));
    //         // 给当前点击的加上 active
    //         e.target.classList.add('active');
            
    //         // 这里可以假装加载数据，给一个透明度闪烁动画
    //         const grid = document.querySelector('.anime-grid');
    //         grid.style.opacity = '0.5';
    //         setTimeout(() => grid.style.opacity = '1', 200);
    //     });
    // });
});