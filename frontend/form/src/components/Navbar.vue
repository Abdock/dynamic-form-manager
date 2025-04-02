<script setup>
import { computed } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth';

const router = useRouter();
const authStore = useAuthStore();

const isAuthenticated = computed(() => authStore.isAuthenticated);
const username = computed(() => authStore.user?.username || 'User');

const logout = () => {
    authStore.logout();
    router.push('/');
};
</script>

<template>
    <nav class="navbar">
        <div class="navbar-container">
            <div class="navbar-logo">
                <router-link to="/">Dynamic Form Manager</router-link>
            </div>

            <div class="navbar-links">
                <router-link to="/" class="nav-link">Home</router-link>
                <router-link to="/posts" class="nav-link">Posts</router-link>
            </div>

            <div class="navbar-auth">
                <template v-if="isAuthenticated">
                    <div class="user-menu">
                        <span class="username">Welcome, {{ username }}</span>
                        <button @click="logout" class="btn-logout">Logout</button>
                    </div>
                </template>
                <template v-else>
                    <router-link to="/login" class="auth-link">Login</router-link>
                    <router-link to="/register" class="auth-link register">Register</router-link>
                </template>
            </div>
        </div>
    </nav>
</template>

<style scoped>
.navbar {
    background-color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    position: sticky;
    top: 0;
    z-index: 100;
}

.navbar-container {
    max-width: 1200px;
    margin: 0 auto;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
}

.navbar-logo a {
    font-size: 1.5rem;
    font-weight: 700;
    color: #333;
    text-decoration: none;
}

.navbar-links {
    display: flex;
    gap: 1.5rem;
}

.nav-link {
    color: #555;
    text-decoration: none;
    padding: 0.5rem 0;
    position: relative;
}

.nav-link:hover, .nav-link.router-link-active {
    color: #646cff;
}

.nav-link.router-link-active::after {
    content: '';
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    height: 2px;
    background-color: #646cff;
}

.navbar-auth {
    display: flex;
    align-items: center;
    gap: 1rem;
}

.auth-link {
    color: #555;
    text-decoration: none;
    padding: 0.5rem 0.75rem;
}

.auth-link:hover {
    color: #646cff;
}

.auth-link.register {
    background-color: #646cff;
    color: white;
    border-radius: 4px;
    transition: background-color 0.2s;
}

.auth-link.register:hover {
    background-color: #535bf2;
    color: white;
}

.user-menu {
    display: flex;
    align-items: center;
    gap: 1rem;
}

.username {
    color: #555;
    font-weight: 500;
}

.btn-logout {
    background-color: transparent;
    color: #e74c3c;
    border: 1px solid #e74c3c;
    padding: 0.4rem 0.75rem;
    border-radius: 4px;
    font-size: 0.9rem;
    cursor: pointer;
    transition: all 0.2s;
}

.btn-logout:hover {
    background-color: #e74c3c;
    color: white;
}

@media (max-width: 768px) {
    .navbar-container {
        flex-direction: column;
        gap: 1rem;
    }

    .navbar-links, .navbar-auth {
        width: 100%;
        justify-content: center;
    }

    .user-menu {
        flex-direction: column;
        gap: 0.5rem;
    }
}
</style>