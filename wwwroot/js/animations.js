document.addEventListener('DOMContentLoaded', function() {
    initScrollAnimations();
    initTerminalAnimation();
    initSkillBars();
    initTypingAnimation();
    initExperienceAnimations();
});

function initScrollAnimations() {
    const sections = document.querySelectorAll('.section');
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, observerOptions);

    sections.forEach(section => {
        observer.observe(section);
    });
}

function initTerminalAnimation() {
    const terminalCommand = document.getElementById('terminal-command');
    if (!terminalCommand) return;

    const commands = [
        'whoami',
        'cat resume.md',
        'ls -la projects/',
        'git log --oneline',
        'npm run build',
        'docker ps',
        'kubectl get pods'
    ];

    let commandIndex = 0;
    let charIndex = 0;
    let isDeleting = false;
    let currentCommand = '';

    function typeCommand() {
        const fullCommand = commands[commandIndex];
        
        if (isDeleting) {
            currentCommand = fullCommand.substring(0, charIndex - 1);
            charIndex--;
            
            if (charIndex === 0) {
                isDeleting = false;
                commandIndex = (commandIndex + 1) % commands.length;
            }
        } else {
            currentCommand = fullCommand.substring(0, charIndex + 1);
            charIndex++;
            
            if (charIndex > fullCommand.length) {
                isDeleting = true;
                terminalCommand.textContent = fullCommand;
                setTimeout(typeCommand, 2000);
                return;
            }
        }

        terminalCommand.textContent = currentCommand;
        const speed = isDeleting ? 50 : 100;
        setTimeout(typeCommand, speed);
    }

    setTimeout(typeCommand, 1000);
}

function initSkillBars() {
    const skillBars = document.querySelectorAll('.skill-bar');
    
    const observerOptions = {
        threshold: 0.5
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const skillBar = entry.target;
                const fill = skillBar.querySelector('.skill-fill');
                const level = skillBar.getAttribute('data-level');
                
                if (fill && level) {
                    fill.style.width = level + '%';
                    skillBar.classList.add('animated');
                }
            }
        });
    }, observerOptions);

    skillBars.forEach(bar => {
        observer.observe(bar);
    });
}

function initTypingAnimation() {
    const typingElement = document.getElementById('typing-subtitle');
    if (!typingElement) return;

    const texts = [
        'Full-Stack Software Engineer',
        'Python Developer',
        'C# Developer',
        'Angular Developer',
        'DevOps Engineer'
    ];

    let textIndex = 0;
    let charIndex = 0;
    let isDeleting = false;
    let currentText = '';

    function typeText() {
        const fullText = texts[textIndex];
        
        if (isDeleting) {
            currentText = fullText.substring(0, charIndex - 1);
            charIndex--;
            
            if (charIndex === 0) {
                isDeleting = false;
                textIndex = (textIndex + 1) % texts.length;
            }
        } else {
            currentText = fullText.substring(0, charIndex + 1);
            charIndex++;
            
            if (charIndex > fullText.length) {
                isDeleting = true;
                typingElement.textContent = fullText;
                setTimeout(typeText, 3000);
                return;
            }
        }

        typingElement.textContent = currentText;
        const speed = isDeleting ? 50 : 100;
        setTimeout(typeText, speed);
    }

    setTimeout(typeText, 1000);
}

function initExperienceAnimations() {
    const experienceItems = document.querySelectorAll('.experience-item');
    
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 100);
            }
        });
    }, observerOptions);

    experienceItems.forEach(item => {
        item.style.opacity = '0';
        item.style.transform = 'translateY(20px)';
        item.style.transition = 'opacity 0.6s ease-out, transform 0.6s ease-out';
        observer.observe(item);
    });
}

