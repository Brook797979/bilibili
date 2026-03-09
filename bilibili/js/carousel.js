document.addEventListener('DOMContentLoaded', function() {
    // 1. 获取DOM元素
    const carouselList = document.querySelector('.carousel-area .carousel-list');
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    const indicatorItems = document.querySelectorAll('.indicator li');
    const carouselArea = document.querySelector('.carousel-area');

    // 如果找不到元素，提前退出避免报错
    if (!carouselList) return;

    // 2. 无缝轮播核心：克隆首尾节点
    const originalItems = Array.from(carouselList.children);
    const realCount = originalItems.length; // 实际图片数量 (通常是 5)

    // 克隆第一张放到最后，克隆最后一张放到最前
    const firstClone = originalItems[0].cloneNode(true);
    const lastClone = originalItems[realCount - 1].cloneNode(true);
    carouselList.appendChild(firstClone);
    carouselList.insertBefore(lastClone, originalItems[0]);

    // 3. 动态计算宽度核心配置
    const totalCount = carouselList.children.length; // 加上克隆后总数 (通常是 7)
    const singleWidth = 100 / totalCount; // 每次移动的精确百分比
    
    // 覆盖 CSS 的宽度：强行设置容器总宽和子元素宽度
    carouselList.style.width = `${totalCount * 100}%`; 
    Array.from(carouselList.children).forEach(item => {
        item.style.width = `${singleWidth}%`;
    });

    // 4. 状态变量
    let currentIndex = 1; // 因为头部加了克隆节点，所以真实的第1张索引变成了 1
    let autoPlayTimer = null;
    const autoPlayInterval = 3000; 
    let isAnimating = false; 

    // 初始化位置：默默移动到真实的第1张
    updateCarousel(true);

    // 5. 核心方法：更新轮播位置
    function updateCarousel(withoutTransition = false) {
        if (withoutTransition) {
            carouselList.style.transition = 'none';
        } else {
            carouselList.style.transition = 'transform 0.5s ease-in-out';
        }
        // 移动轨道
        carouselList.style.transform = `translateX(-${currentIndex * singleWidth}%)`;
        // 更新小圆点
        updateIndicators();
    }

    // 6. 更新指示器状态
    function updateIndicators() {
        indicatorItems.forEach(li => li.classList.remove('active'));
        // 算出对应的小圆点索引
        let realIndex = currentIndex - 1;
        // 容错处理：防止指示器越界报错
        if (realIndex < 0) realIndex = realCount - 1;
        if (realIndex >= realCount) realIndex = 0;

        if (indicatorItems[realIndex]) {
            indicatorItems[realIndex].classList.add('active');
        }
    }

    // 7. 下一张 (包含无缝拉回处理)
    function goToNext() {
        if (isAnimating) return;
        isAnimating = true;

        currentIndex++;
        updateCarousel(false); // 有过渡滑向下一张

        // 如果滑到了末尾的【假首图】
        if (currentIndex === totalCount - 1) {
            setTimeout(() => {
                currentIndex = 1; // 瞬间将索引改成【真首图】
                updateCarousel(true); // 无过渡拉回，欺骗眼睛
                isAnimating = false; // 解除动画锁
            }, 500); // 这个时间必须和 transition 的 0.5s 保持一致
        } else {
            setTimeout(() => isAnimating = false, 500);
        }
    }

    // 8. 上一张 (包含无缝拉回处理)
    function goToPrev() {
        if (isAnimating) return;
        isAnimating = true;

        currentIndex--;
        updateCarousel(false);

        // 如果滑到了头部的【假尾图】
        if (currentIndex === 0) {
            setTimeout(() => {
                currentIndex = realCount; // 瞬间将索引改成【真尾图】
                updateCarousel(true); // 无过渡拉回
                isAnimating = false;
            }, 500);
        } else {
            setTimeout(() => isAnimating = false, 500);
        }
    }

    // 9. 事件绑定
    nextBtn.addEventListener('click', goToNext);
    prevBtn.addEventListener('click', goToPrev);

    indicatorItems.forEach((li, index) => {
        li.addEventListener('click', () => {
            if (isAnimating) return;
            currentIndex = index + 1; // 小圆点从0开始，对应真实的索引需 +1
            updateCarousel(false);
        });
    });

    // 10. 悬停停止/恢复轮播
    carouselArea.addEventListener('mouseenter', () => {
        if (autoPlayTimer) clearInterval(autoPlayTimer);
    });
    carouselArea.addEventListener('mouseleave', () => {
        autoPlayTimer = setInterval(goToNext, autoPlayInterval);
    });

    // 11. 启动！
    autoPlayTimer = setInterval(goToNext, autoPlayInterval);
});