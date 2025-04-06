import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { api } from '../services/api';

export const useAuthStore = defineStore('auth', () => {
    const token = ref(localStorage.getItem('token') || null);
    const user = ref(null);
    const loading = ref(false);
    const error = ref(null);

    const isAuthenticated = computed(() => !!token.value);

    async function login(email, password) {
        loading.value = true;
        error.value = null;

        try {
            const response = await api.login(email, password);
            console.log(response);
            token.value = response.accessToken;
            localStorage.setItem('token', response.accessToken);
            await fetchUserInfo();
            return true;
        } catch (err) {
            error.value = err.message || 'Failed to login';
            return false;
        } finally {
            loading.value = false;
        }
    }

    async function register(email, username, password) {
        loading.value = true;
        error.value = null;

        try {
            const response = await api.register(email, username, password);
            console.log(response);
            token.value = response.accessToken;
            localStorage.setItem('token', response.accessToken);
            await fetchUserInfo();
            return true;
        } catch (err) {
            error.value = err.message || 'Failed to register';
            return false;
        } finally {
            loading.value = false;
        }
    }

    async function fetchUserInfo() {
        if (!token.value) return;

        loading.value = true;
        error.value = null;

        try {
            const userData = await api.getUserInfo();
            user.value = userData;
        } catch (err) {
            error.value = err.message || 'Failed to get user info';
            logout();
        } finally {
            loading.value = false;
        }
    }

    function logout() {
        token.value = null;
        user.value = null;
        localStorage.removeItem('token');
    }

    if (token.value) {
        fetchUserInfo();
    }

    return {
        token,
        user,
        loading,
        error,
        isAuthenticated,
        login,
        register,
        logout,
        fetchUserInfo
    };
});