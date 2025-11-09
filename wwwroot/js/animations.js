function initHeroAnimation() {
}

document.addEventListener('DOMContentLoaded', () => {
    initHeroAnimation();
    initScrollAnimations();
    initSkillBars();
    initTypingAnimation();
    initExperienceAnimations();
});

function initScrollAnimations() {
    const sections = document.querySelectorAll('.section');
    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) entry.target.classList.add('visible');
        });
    }, { threshold: 0.1, rootMargin: '0px 0px -50px 0px' });

    sections.forEach(section => observer.observe(section));
}

function initSkillBars() {
    const skillBars = document.querySelectorAll('.skill-bar');
    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const fill = entry.target.querySelector('.skill-fill');
                const years = entry.target.getAttribute('data-years');
                if (fill && years) {
                    const percentage = (parseInt(years) / 3) * 100;
                    fill.style.width = percentage + '%';
                    entry.target.classList.add('animated');
                }
            }
        });
    }, { threshold: 0.5 });

    skillBars.forEach(bar => observer.observe(bar));
}

function initTypingAnimation() {
    const typingElement = document.getElementById('typing-subtitle');
    if (!typingElement) return;

    const texts = ['Full-Stack Software Engineer', 'Python Developer', 'C# Developer', 'Angular Developer', 'DevOps Engineer'];
    let textIndex = 0;
    let charIndex = 0;
    let isDeleting = false;

    function typeText() {
        const fullText = texts[textIndex];
        
        if (isDeleting) {
            typingElement.textContent = fullText.substring(0, --charIndex);
            if (charIndex === 0) {
                isDeleting = false;
                textIndex = (textIndex + 1) % texts.length;
            }
        } else {
            typingElement.textContent = fullText.substring(0, ++charIndex);
            if (charIndex > fullText.length) {
                isDeleting = true;
                setTimeout(typeText, 3000);
                return;
            }
        }

        setTimeout(typeText, isDeleting ? 50 : 100);
    }

    setTimeout(typeText, 1000);
}

function initExperienceAnimations() {
    const experienceItems = document.querySelectorAll('.experience-item');
    const observer = new IntersectionObserver((entries, index) => {
        entries.forEach((entry, i) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, i * 100);
            }
        });
    }, { threshold: 0.1, rootMargin: '0px 0px -50px 0px' });

    experienceItems.forEach(item => {
        item.style.opacity = '0';
        item.style.transform = 'translateY(20px)';
        item.style.transition = 'opacity 0.6s ease-out, transform 0.6s ease-out';
        observer.observe(item);
    });
}
