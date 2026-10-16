// Floating Cebu icons (lechon, skewers, guitar, sun...) that drift down and
// get pushed away by the pointer. Respects prefers-reduced-motion, cleans up
// every listener on destroy, and renders crisply on HiDPI screens.
(() => {
    const ICONS = ['☀️', '🍢', '🎸', '🍚', '🐖', '🥥'];
    let rafId = 0;
    let teardown = null;

    function destroy() {
        if (teardown) {
            teardown();
            teardown = null;
        }
    }

    function init() {
        destroy(); // safe against double-init (circuit reconnects, hot reload)

        const canvas = document.getElementById('cebuRainCanvas');
        if (!canvas) return;
        const ctx = canvas.getContext('2d');
        const reduceMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

        const mouse = { x: -1000, y: -1000, radius: 120 };
        let width = 0;
        let height = 0;
        let particles = [];

        const makeParticle = (randomY) => ({
            char: ICONS[Math.floor(Math.random() * ICONS.length)],
            x: Math.random() * width,
            y: randomY ? Math.random() * height : -30,
            size: Math.random() * 14 + 14,
            speedY: Math.random() * 0.8 + 0.4,
            speedX: (Math.random() - 0.5) * 0.3,
            vx: 0,
            vy: 0,
            opacity: Math.random() * 0.25 + 0.2
        });

        const resize = () => {
            const dpr = Math.min(window.devicePixelRatio || 1, 2);
            width = window.innerWidth;
            height = window.innerHeight;
            canvas.width = Math.round(width * dpr);
            canvas.height = Math.round(height * dpr);
            ctx.setTransform(dpr, 0, 0, dpr, 0, 0);

            const target = Math.round(Math.min(40, Math.max(18, width / 45)));
            while (particles.length < target) particles.push(makeParticle(true));
            particles.length = target;
            if (reduceMotion) draw();
        };

        const draw = () => {
            ctx.clearRect(0, 0, width, height);
            for (const p of particles) {
                ctx.globalAlpha = p.opacity;
                ctx.font = `${p.size}px sans-serif`;
                ctx.fillText(p.char, p.x, p.y);
            }
            ctx.globalAlpha = 1;
        };

        const step = () => {
            for (const p of particles) {
                const dx = p.x - mouse.x;
                const dy = p.y - mouse.y;
                const distance = Math.hypot(dx, dy);

                if (distance < mouse.radius) {
                    const force = (mouse.radius - distance) / mouse.radius;
                    const angle = Math.atan2(dy, dx);
                    p.vx += Math.cos(angle) * force * 5;
                    p.vy += Math.sin(angle) * force * 5;
                }

                p.x += p.speedX + p.vx;
                p.y += p.speedY + p.vy;
                p.vx *= 0.9;
                p.vy *= 0.9;

                if (p.y > height + 40) {
                    p.y = -30;
                    p.x = Math.random() * width;
                }
                if (p.x < -30) p.x = width + 20;
                if (p.x > width + 30) p.x = -20;
            }
            draw();
            rafId = requestAnimationFrame(step);
        };

        const onMove = (e) => { mouse.x = e.clientX; mouse.y = e.clientY; };
        const onLeave = () => { mouse.x = -1000; mouse.y = -1000; };

        resize();
        window.addEventListener('resize', resize);
        window.addEventListener('pointermove', onMove, { passive: true });
        document.addEventListener('mouseleave', onLeave);

        if (reduceMotion) draw(); // single static frame, no animation loop
        else rafId = requestAnimationFrame(step);

        teardown = () => {
            cancelAnimationFrame(rafId);
            window.removeEventListener('resize', resize);
            window.removeEventListener('pointermove', onMove);
            document.removeEventListener('mouseleave', onLeave);
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        };
    }

    // Same global names the Blazor component already calls.
    window.initCebuCanvas = init;
    window.destroyCebuCanvas = destroy;
})();
