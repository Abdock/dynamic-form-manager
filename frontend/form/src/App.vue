<!-- src/App.vue -->
<script setup>
import { RouterView } from 'vue-router';
import Navbar from './components/Navbar.vue';
</script>

<template>
    <div class="app">
        <Navbar />
        <main class="main-content">
            <RouterView />
        </main>
    </div>
</template>

<style>
/* Global styles */
body {
    margin: 0;
    min-height: 100vh;
    color: #213547;
    background-color: #f5f5f7;
    font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
    line-height: 1.5;
}

.app {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
}

.main-content {
    flex: 1;
}

/* Reset some default styles */
*, *::before, *::after {
    box-sizing: border-box;
}

h1, h2, h3, h4, h5, h6, p {
    margin-top: 0;
}

button {
    font-family: inherit;
}
</style>