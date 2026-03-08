document.addEventListener('DOMContentLoaded', function() {
    // 1. 获取DOM元素（精准匹配你的结构）
    const carouselList = document.querySelector('.carousel-area .carousel-list');
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    const indicatorItems = document.querySelectorAll('.indicator li');
    const carouselArea = document.querySelector('.carousel-area');

    // 2. 核心配置（匹配你的5张轮播图）
    const itemCount = 5; // 固定5张图
    const singleWidth = 20; // 单张占20%（100/5）
    let currentIndex = 0; // 默认第一张
    let autoPlayTimer = null;
    const autoPlayInterval = 3000; // 3秒切换
    let isAnimating = false; // 动画锁（防止重复触发）

    // 3. 核心方法：更新轮播位置（修复强制重绘+过渡控制）
    function updateCarousel(withoutTransition = false) {
        // 动画中不执行
        if (isAnimating) return;
        
        // 控制过渡动画：true=无过渡（瞬间切换），false=有过渡（丝滑）
        if (withoutTransition) {
            carouselList.style.transition = 'none';
        } else {
            carouselList.style.transition = 'transform 0.5s ease';
        }
        
        // 设置平移位置（核心动效）
        carouselList.style.transform = `translateX(-${currentIndex * singleWidth}%)`;
        
        // 更新指示器激活态
        indicatorItems.forEach((li, index) => {
            li.classList.toggle('active', index === currentIndex);
        });

        // 强制重绘（关键：解决过渡失效问题）
        carouselList.offsetHeight; // 替代void，兼容性更好
    }

    // 4. 下一张（修复动画时序+解锁逻辑）
    function goToNext() {
        if (isAnimating) return;
        isAnimating = true; // 上锁

        // 最后一张切第一张：无缝逻辑
        if (currentIndex === itemCount - 1) {
            // 步骤1：先滑到"假位置"（超出容器，视觉上是正向切）
            carouselList.style.transition = 'transform 0.5s ease';
            carouselList.style.transform = `translateX(-${(itemCount) * singleWidth}%)`;
            
            // 步骤2：动画结束后，瞬间重置到第一张（无过渡）
            setTimeout(() => {
                currentIndex = 0;
                updateCarousel(true); // 无过渡重置
                isAnimating = false; // 解锁
            }, 500); // 匹配过渡时长0.5s
        } else {
            // 普通切换：正常加索引
            currentIndex++;
            updateCarousel(false);
            // 动画结束后解锁
            setTimeout(() => isAnimating = false, 500);
        }
    }

    // 5. 上一张（修复反向无缝逻辑）
    function goToPrev() {
        if (isAnimating) return;
        isAnimating = true; // 上锁

        // 第一张切最后一张：无缝逻辑
        if (currentIndex === 0) {
            // 步骤1：瞬间切到假位置（无过渡）
            updateCarousel(true);
            carouselList.style.transform = `translateX(${singleWidth}%)`;
            
            // 步骤2：强制重绘后，滑到最后一张
            setTimeout(() => {
                carouselList.style.transition = 'transform 0.5s ease';
                currentIndex = itemCount - 1;
                updateCarousel(false);
                isAnimating = false; // 解锁
            }, 10); // 10ms确保重绘完成
        } else {
            // 普通切换：正常减索引
            currentIndex--;
            updateCarousel(false);
            setTimeout(() => isAnimating = false, 500);
        }
    }

    // 6. 指示器点击（修复点击无响应）
    function bindIndicatorEvent() {
        indicatorItems.forEach((li, index) => {
            li.addEventListener('click', function() {
                if (isAnimating) return;
                currentIndex = index;
                updateCarousel(false); // 点击有过渡
            });
        });
    }

    // 7. 自动播放（修复定时器叠加）
    function startAutoPlay() {
        // 先清旧定时器，避免叠加
        if (autoPlayTimer) clearInterval(autoPlayTimer);
        autoPlayTimer = setInterval(goToNext, autoPlayInterval);
    }

    // 8. 暂停自动播放
    function stopAutoPlay() {
        if (autoPlayTimer) clearInterval(autoPlayTimer);
    }

    // 9. 鼠标悬停/离开（修复暂停/恢复）
    function bindHoverEvent() {
        carouselArea.addEventListener('mouseenter', stopAutoPlay);
        carouselArea.addEventListener('mouseleave', startAutoPlay);
    }

    // 10. 初始化（修复事件绑定顺序）
    function initCarousel() {
        // 先初始化位置（确保页面加载就显示第一张）
        updateCarousel(false);
        // 绑定按钮事件
        prevBtn.addEventListener('click', goToPrev);
        nextBtn.addEventListener('click', goToNext);
        // 绑定指示器
        bindIndicatorEvent();
        // 绑定悬停
        bindHoverEvent();
        // 启动自动播放
        startAutoPlay();
    }

    // 执行初始化（必执行）
    initCarousel();
});