<!-- src/App.vue -->
<script setup>
import { RouterView } from 'vue-router';
import Navbar from './components/Navbar.vue';
</script>

<template>
    <Navbar />
    <main class="main-content">
        <RouterView />
    </main>
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