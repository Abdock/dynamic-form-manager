<script setup>
import { ref } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useAuthStore } from '../stores/auth';

const router = useRouter();
const route = useRoute();
const authStore = useAuthStore();

const email = ref('');
const password = ref('');
const loginError = ref('');

const handleLogin = async () => {
    if (!email.value || !password.value) {
        loginError.value = 'Please fill in all fields';
        return;
    }

    const success = await authStore.login(email.value, password.value);

    if (success) {
        const redirectPath = route.query.redirect || '/posts';
        router.push(redirectPath);
    } else {
        loginError.value = authStore.error || 'Login failed';
    }
};
</script>

<template>
    <div class="login-container">
        <h1>Login</h1>

        <div v-if="loginError" class="error-message">
            {{ loginError }}
        </div>

        <form @submit.prevent="handleLogin" class="login-form">
            <div class="form-group">
                <label for="email">Email</label>
                <input
                    id="email"
                    v-model="email"
                    type="email"
                    placeholder="Enter your email"
                    required
                />
            </div>

            <div class="form-group">
                <label for="password">Password</label>
                <input
                    id="password"
                    v-model="password"
                    type="password"
                    placeholder="Enter your password"
                    required
                />
            </div>

            <div class="form-actions">
                <button
                    type="submit"
                    :disabled="authStore.loading"
                    class="btn-primary"
                >
                    {{ authStore.loading ? 'Logging in...' : 'Login' }}
                </button>
            </div>
        </form>

        <div class="register-link">
            Don't have an account? <router-link to="/register">Register here</router-link>
        </div>
    </div>
</template>

<style scoped>
.login-container {
    max-width: 400px;
    margin: 2rem auto;
    padding: 1.5rem;
    border-radius: 8px;
    background-color: #f9f9f9;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

h1 {
    text-align: center;
    margin-bottom: 1.5rem;
    color: #333;
}

.login-form {
    display: flex;
    flex-direction: column;
    gap: 1rem;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

label {
    font-weight: 500;
    color: #555;
}

input {
    padding: 0.75rem;
    border-radius: 4px;
    border: 1px solid #ccc;
    font-size: 1rem;
}

.btn-primary {
    background-color: #646cff;
    color: white;
    border: none;
    padding: 0.75rem;
    border-radius: 4px;
    font-size: 1rem;
    cursor: pointer;
    transition: background-color 0.2s;
}

.btn-primary:hover {
    background-color: #535bf2;
}

.btn-primary:disabled {
    background-color: #a3a4d8;
    cursor: not-allowed;
}

.form-actions {
    margin-top: 1rem;
}

.error-message {
    background-color: #fee;
    color: #e44;
    padding: 0.75rem;
    border-radius: 4px;
    margin-bottom: 1rem;
    text-align: center;
}

.register-link {
    margin-top: 1.5rem;
    text-align: center;
}
</style>