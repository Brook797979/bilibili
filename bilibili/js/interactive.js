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