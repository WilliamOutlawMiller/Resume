document.addEventListener('DOMContentLoaded', () => {
    initCarousel();
    initScrollAnimations();
});

function initCarousel() {
    const carousel = document.getElementById('projectCarousel');
    if (!carousel) return;

    const slides = carousel.querySelectorAll('.project-slide');
    const indicators = document.querySelectorAll('.carousel-indicator');
    const prevBtn = document.querySelector('.carousel-btn-prev');
    const nextBtn = document.querySelector('.carousel-btn-next');
    
    let currentSlide = 0;
    
    function showSlide(index) {
        if (index < 0) index = slides.length - 1;
        if (index >= slides.length) index = 0;
        
        slides.forEach((slide, i) => slide.classList.toggle('active', i === index));
        indicators.forEach((indicator, i) => indicator.classList.toggle('active', i === index));
        
        currentSlide = index;
        updateURL();
    }
    
    function updateURL() {
        const activeSlide = slides[currentSlide];
        if (activeSlide && window.location.hash) {
            window.history.replaceState(null, '', window.location.pathname + '#' + activeSlide.id);
        }
    }
    
    function goToProjectByHash() {
        const hash = window.location.hash.substring(1);
        if (hash) {
            const targetSlide = carousel.querySelector('#' + hash);
            if (targetSlide) {
                const slideIndex = parseInt(targetSlide.getAttribute('data-slide-index'));
                if (!isNaN(slideIndex)) {
                    showSlide(slideIndex);
                    setTimeout(() => targetSlide.scrollIntoView({ behavior: 'smooth', block: 'center' }), 100);
                }
            }
        } else {
            showSlide(0);
        }
    }
    
    function goToSlide(index) {
        if (index < 0) index = slides.length - 1;
        if (index >= slides.length) index = 0;
        showSlide(index);
        const activeSlide = slides[index];
        if (activeSlide) {
            window.history.replaceState(null, '', window.location.pathname + '#' + activeSlide.id);
        }
    }
    
    prevBtn?.addEventListener('click', () => goToSlide(currentSlide - 1));
    nextBtn?.addEventListener('click', () => goToSlide(currentSlide + 1));
    
    indicators.forEach((indicator, index) => {
        indicator.addEventListener('click', () => goToSlide(index));
    });
    
    document.addEventListener('keydown', e => {
        if (e.key === 'ArrowLeft') goToSlide(currentSlide - 1);
        if (e.key === 'ArrowRight') goToSlide(currentSlide + 1);
    });
    
    window.addEventListener('hashchange', goToProjectByHash);
    goToProjectByHash();
}

function initScrollAnimations() {
    const timelineItems = document.querySelectorAll('.timeline-item');
    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.1, rootMargin: '0px 0px -50px 0px' });

    timelineItems.forEach((item, index) => {
        item.style.transitionDelay = `${index * 0.1}s`;
        observer.observe(item);
    });
}
