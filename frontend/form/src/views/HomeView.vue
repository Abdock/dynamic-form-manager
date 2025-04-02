<script setup>
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth';

const router = useRouter();
const authStore = useAuthStore();

const goToPosts = () => {
    router.push('/posts');
};

const goToLogin = () => {
    router.push('/login');
};

const goToRegister = () => {
    router.push('/register');
};
</script>

<template>
    <div class="home-container">
        <h1 class="app-title">Dynamic Form Manager</h1>

        <div class="hero-section">
            <h2>Welcome to Dynamic Form Manager</h2>
            <p class="app-description">
                A platform for creating, sharing, and managing dynamic forms and posts.
            </p>

            <div class="actions">
                <button @click="goToPosts" class="btn-primary">
                    Browse Posts
                </button>

                <div v-if="!authStore.isAuthenticated" class="auth-buttons">
                    <button @click="goToLogin" class="btn-secondary">
                        Login
                    </button>
                    <button @click="goToRegister" class="btn-secondary">
                        Register
                    </button>
                </div>
            </div>
        </div>

        <div class="features-section">
            <h3>Features</h3>

            <div class="features-grid">
                <div class="feature-card">
                    <h4>Create Dynamic Posts</h4>
                    <p>Create and share posts with dynamic content structures.</p>
                </div>

                <div class="feature-card">
                    <h4>User Authentication</h4>
                    <p>Secure login and registration system to manage your content.</p>
                </div>

                <div class="feature-card">
                    <h4>Search Functionality</h4>
                    <p>Easily find posts with the built-in search feature.</p>
                </div>

                <div class="feature-card">
                    <h4>Pagination</h4>
                    <p>Navigate through posts with efficient pagination.</p>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.home-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 2rem 1rem;
}

.app-title {
    font-size: 2.5rem;
    text-align: center;
    margin-bottom: 2rem;
    color: #333;
}

.hero-section {
    text-align: center;
    background-color: #f8f9fa;
    padding: 3rem 2rem;
    border-radius: 8px;
    margin-bottom: 3rem;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

h2 {
    font-size: 2rem;
    margin-bottom: 1rem;
    color: #333;
}

.app-description {
    font-size: 1.2rem;
    color: #555;
    max-width: 800px;
    margin: 0 auto 2rem;
}

.actions {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 1.5rem;
}

.btn-primary {
    background-color: #646cff;
    color: white;
    border: none;
    padding: 0.75rem 2rem;
    border-radius: 4px;
    font-size: 1.1rem;
    cursor: pointer;
    transition: background-color 0.2s;
}

.btn-primary:hover {
    background-color: #535bf2;
}

.auth-buttons {
    display: flex;
    gap: 1rem;
}

.btn-secondary {
    background-color: transparent;
    color: #646cff;
    border: 1px solid #646cff;
    padding: 0.75rem 1.5rem;
    border-radius: 4px;
    font-size: 1rem;
    cursor: pointer;
    transition: all 0.2s;
}

.btn-secondary:hover {
    background-color: #646cff;
    color: white;
}

.features-section {
    padding: 2rem;
}

h3 {
    font-size: 1.75rem;
    text-align: center;
    margin-bottom: 2rem;
    color: #333;
}

.features-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 2rem;
}

.feature-card {
    background-color: #f8f9fa;
    padding: 1.5rem;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s, box-shadow 0.2s;
}

.feature-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

h4 {
    font-size: 1.25rem;
    margin-bottom: 0.75rem;
    color: #333;
}

.feature-card p {
    color: #555;
    line-height: 1.5;
}

@media (max-width: 768px) {
    .features-grid {
        grid-template-columns: 1fr;
    }

    .auth-buttons {
        flex-direction: column;
    }
}
</style>