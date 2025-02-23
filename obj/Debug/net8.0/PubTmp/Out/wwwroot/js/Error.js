document.querySelectorAll('input[required]').forEach(input => {
    input.addEventListener('invalid', function () {
        switch (this.id) {
            case 'username':
                this.setCustomValidity('Please enter your username');
                break;
            case 'fullname':
                this.setCustomValidity('Please enter your full name');
                break;
            case 'password':
                this.setCustomValidity('Please enter your password');
                break;
            case 'address':
                this.setCustomValidity('Please enter your address');
                break;
            default:
                this.setCustomValidity('Please fill out this field');
        }
    });

    input.addEventListener('input', function () {
        this.setCustomValidity('');
    });
});

// Email validation
document.getElementById('email').addEventListener('input', function () {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (this.value === '') {
        this.setCustomValidity('Please enter your email address');
    } else if (!emailRegex.test(this.value)) {
        this.setCustomValidity('Please enter a valid email address uid@gmail.com');
    } else {
        this.setCustomValidity('');
    }
});

document.getElementById('email').addEventListener('invalid', function () {
    if (this.value === '') {
        this.setCustomValidity('Please enter your email address');
    } else {
        this.setCustomValidity('Please enter a valid email address uid@gmail.com');
    }
});

// Password length validation
document.getElementById('password').addEventListener('input', function () {
    const password = this.value;

    if (password.length < 6) {
        this.setCustomValidity('Password must be at least 6 characters long');
    } else {
        this.setCustomValidity('');
    }
});

document.getElementById('password').addEventListener('invalid', function () {
    const password = this.value;
    if (password.length < 6) {
        this.setCustomValidity('Password must be at least 6 characters long');
    } else {
        this.setCustomValidity('Please enter your password');
    }
});

// Confirm Password validation
document.getElementById('confirm-password').addEventListener('input', function () {
    const password = document.getElementById('password').value;
    const confirmPassword = this.value;

    if (this.value === '') {
        this.setCustomValidity('Please confirm your password');
    } else if (confirmPassword !== password) {
        this.setCustomValidity('Passwords do not match');
    } else {
        this.setCustomValidity('');
    }
});

document.getElementById('confirm-password').addEventListener('invalid', function () {
    if (this.value === '') {
        this.setCustomValidity('Please confirm your password');
    } else {
        this.setCustomValidity('Passwords do not match');
    }
});
