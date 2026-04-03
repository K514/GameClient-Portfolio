const lb = document.getElementById('lightbox');
const lbImg = document.getElementById('lbImg');
const lbCap = document.getElementById('lbCap');
const lbClose = document.getElementById('lbClose');

document.addEventListener('click', (e) => {
    const target = e.target;

    if (target.matches('.project-gallery img')) {
    const full = target.getAttribute('data-full') || target.src;
    lbImg.src = full;
    lbCap.textContent = target.alt || '';
    lb.classList.add('open');
    document.body.classList.add('scroll-lock');
    }

    if (target === lb || target === lbClose) {
    lb.classList.remove('open');
    lbImg.src = '';
    document.body.classList.remove('scroll-lock');
    }
});

document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
    lb.classList.remove('open');
    lbImg.src = '';
    document.body.classList.remove('scroll-lock');
    }
});