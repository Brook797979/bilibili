const doubleClickTargets = document.querySelectorAll('.double-click-target');
    doubleClickTargets.forEach(target => {
        target.addEventListener('dblclick', function(e) {
            const rect = target.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;
            const likeIcon = document.createElement('div');
            likeIcon.className = 'like-pop-animation';
            const emojis = ['💖', '👍'];
            likeIcon.innerText = emojis[Math.floor(Math.random() * emojis.length)];
            likeIcon.style.left = `${x}px`;
            likeIcon.style.top = `${y}px`;
            target.appendChild(likeIcon);
            setTimeout(() => {
                likeIcon.remove();
            }, 800);
        });
    });