<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth';

const router = useRouter();
const authStore = useAuthStore();

const username = ref('');
const email = ref('');
const password = ref('');
const confirmPassword = ref('');
const registerError = ref('');

const handleRegister = async () => {
    // Reset error
    registerError.value = '';

    // Validate form
    if (!username.value || !email.value || !password.value || !confirmPassword.value) {
        registerError.value = 'Please fill in all fields';
        return;
    }

    if (password.value !== confirmPassword.value) {
        registerError.value = 'Passwords do not match';
        return;
    }

    if (password.value.length < 6) {
        registerError.value = 'Password must be at least 6 characters';
        return;
    }

    const success = await authStore.register(email.value, username.value, password.value);

    if (success) {
        router.push('/posts');
    } else {
        registerError.value = authStore.error || 'Registration failed';
    }
};
</script>

<template>
    <div class="register-container">
        <h1>Register</h1>

        <div v-if="registerError" class="error-message">
            {{ registerError }}
        </div>

        <form @submit.prevent="handleRegister" class="register-form">
            <div class="form-group">
                <label for="username">Username</label>
                <input
                    id="username"
                    v-model="username"
                    type="text"
                    placeholder="Choose a username"
                    required
                />
            </div>

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
                    placeholder="Create a password"
                    required
                />
            </div>

            <div class="form-group">
                <label for="confirm-password">Confirm Password</label>
                <input
                    id="confirm-password"
                    v-model="confirmPassword"
                    type="password"
                    placeholder="Confirm your password"
                    required
                />
            </div>

            <div class="form-actions">
                <button
                    type="submit"
                    :disabled="authStore.loading"
                    class="btn-primary"
                >
                    {{ authStore.loading ? 'Registering...' : 'Register' }}
                </button>
            </div>
        </form>

        <div class="login-link">
            Already have an account? <router-link to="/login">Login here</router-link>
        </div>
    </div>
</template>

<style scoped>
.register-container {
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

.register-form {
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

.login-link {
    margin-top: 1.5rem;
    text-align: center;
}
</style>