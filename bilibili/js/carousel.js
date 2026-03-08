// 等待DOM加载完成后执行
document.addEventListener('DOMContentLoaded', function() {
    // 1. 获取核心DOM元素
    const carouselList = document.querySelector('.carousel-area .carousel-list');
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    const indicatorItems = document.querySelectorAll('.indicator li');
    const carouselArea = document.querySelector('.carousel-area');

    // 2. 核心配置参数
    const itemCount = 5; // 轮播图总数（5张）
    const singleWidth = 100 / itemCount; // 单张图占容器的百分比（20%）
    let currentIndex = 0; // 当前显示的轮播图索引（默认第1张）
    let autoPlayTimer = null; // 自动轮播定时器
    const autoPlayInterval = 3000; // 自动轮播间隔（3秒）

    // 3. 核心方法：更新轮播图显示
    function updateCarousel() {
        // 3.1 计算并设置轮播列表的平移距离（核心：JS控制CSS transform）
        carouselList.style.transform = `translateX(-${currentIndex * singleWidth}%)`;
        
        // 3.2 更新指示器激活态
        indicatorItems.forEach((li, index) => {
            if (index === currentIndex) {
                li.classList.add('active');
            } else {
                li.classList.remove('active');
            }
        });
    }

    // 4. 切换到上一张
    function goToPrev() {
        currentIndex = (currentIndex - 1 + itemCount) % itemCount;
        updateCarousel();
    }

    // 5. 切换到下一张
    function goToNext() {
        currentIndex = (currentIndex + 1) % itemCount;
        updateCarousel();
    }

    // 6. 点击指示器切换到对应轮播图
    function bindIndicatorEvent() {
        indicatorItems.forEach((li, index) => {
            li.addEventListener('click', function() {
                currentIndex = index;
                updateCarousel();
            });
        });
    }

    // 7. 自动轮播功能
    function startAutoPlay() {
        // 先清除旧定时器，避免叠加
        if (autoPlayTimer) clearInterval(autoPlayTimer);
        autoPlayTimer = setInterval(goToNext, autoPlayInterval);
    }

    // 8. 暂停自动轮播
    function stopAutoPlay() {
        if (autoPlayTimer) clearInterval(autoPlayTimer);
    }

    // 9. 绑定鼠标悬停/离开事件（暂停/恢复自动轮播）
    function bindHoverEvent() {
        carouselArea.addEventListener('mouseenter', stopAutoPlay);
        carouselArea.addEventListener('mouseleave', startAutoPlay);
    }

    // 10. 初始化轮播
    function initCarousel() {
        // 绑定按钮点击事件
        prevBtn.addEventListener('click', goToPrev);
        nextBtn.addEventListener('click', goToNext);
        
        // 绑定指示器事件
        bindIndicatorEvent();
        
        // 绑定悬停事件
        bindHoverEvent();
        
        // 启动自动轮播
        startAutoPlay();
        
        // 初始化显示（默认第一张）
        updateCarousel();
    }

    // 执行初始化
    initCarousel();
});