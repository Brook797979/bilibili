// ======== 3. 灵魂级创意交互：双击封面“点赞/三连”特效 ========
    
    // 你可以把 '.anime-card, .live-cover' 等也加上去，让全站图片都能双击！
    const doubleClickTargets = document.querySelectorAll('.double-click-target');

    doubleClickTargets.forEach(target => {
        target.addEventListener('dblclick', function(e) {
            // 获取鼠标在当前元素内的相对点击坐标
            const rect = target.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            // 创建一个表示“赞”或“硬币”的元素
            const likeIcon = document.createElement('div');
            likeIcon.className = 'like-pop-animation';
            
            // 随机生成爱心、硬币或大拇指
            const emojis = ['💖', '👍'];
            likeIcon.innerText = emojis[Math.floor(Math.random() * emojis.length)];
            
            // 设置生成位置
            likeIcon.style.left = `${x}px`;
            likeIcon.style.top = `${y}px`;

            // 追加到当前卡片内
            target.appendChild(likeIcon);

            // 动画结束后(0.8秒)，自动从DOM中移除它，保持整洁
            setTimeout(() => {
                likeIcon.remove();
            }, 800);
        });
    });