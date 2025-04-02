<script setup>
import { onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { usePostsStore } from '../stores/posts';

const route = useRoute();
const router = useRouter();
const postsStore = usePostsStore();

const postId = computed(() => route.params.id);

// Format date to a more readable format
const formatDate = (dateString) => {
    const date = new Date(dateString);
    return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    }).format(date);
};

onMounted(async () => {
    if (postId.value) {
        await postsStore.fetchPostById(postId.value);
    }
});

const goBack = () => {
    router.push('/posts');
};
</script>

<template>
    <div class="post-detail-container">
        <button @click="goBack" class="btn-back">
            &larr; Back to Posts
        </button>

        <!-- Loading indicator -->
        <div v-if="postsStore.loading" class="loading-indicator">
            Loading post...
        </div>

        <!-- Error message -->
        <div v-else-if="postsStore.error" class="error-message">
            {{ postsStore.error }}
            <button @click="goBack" class="btn-retry">
                Go back to posts
            </button>
        </div>

        <!-- Post not found -->
        <div v-else-if="!postsStore.currentPost" class="not-found">
            Post not found.
            <button @click="goBack" class="btn-retry">
                Go back to posts
            </button>
        </div>

        <!-- Post content -->
        <div v-else class="post-content">
            <h1>{{ postsStore.currentPost.node.title }}</h1>

            <div class="post-metadata">
                Posted on {{ formatDate(postsStore.currentPost.createdAt) }}
            </div>

            <div class="post-body">
                {{ postsStore.currentPost.node.content }}
            </div>

            <!-- Display any other post properties -->
            <div v-if="Object.keys(postsStore.currentPost.node).length > 2" class="post-additional-info">
                <h3>Additional Information</h3>
                <div v-for="(value, key) in postsStore.currentPost.node" :key="key">
                    <template v-if="key !== 'title' && key !== 'content'">
                        <strong>{{ key }}:</strong> {{ value }}
                    </template>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.post-detail-container {
    max-width: 800px;
    margin: 2rem auto;
    padding: 1rem;
}

.btn-back {
    background-color: transparent;
    color: #646cff;
    border: none;
    padding: 0.5rem 0;
    margin-bottom: 1.5rem;
    font-size: 1rem;
    cursor: pointer;
    display: inline-flex;
    align-items: center;
}

.btn-back:hover {
    color: #535bf2;
    text-decoration: underline;
}

.loading-indicator, .error-message, .not-found {
    text-align: center;
    padding: 2rem;
    background-color: #f8f9fa;
    border-radius: 8px;
    margin: 2rem 0;
}

.error-message {
    color: #e44;
    background-color: #fee;
}

.btn-retry {
    background-color: #646cff;
    color: white;
    border: none;
    padding: 0.5rem 1rem;
    border-radius: 4px;
    margin-top: 1rem;
    font-size: 0.9rem;
    cursor: pointer;
}

.post-content {
    background-color: #f8f9fa;
    border-radius: 8px;
    padding: 2rem;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

h1 {
    margin-bottom: 1rem;
    color: #333;
}

.post-metadata {
    color: #777;
    font-size: 0.9rem;
    margin-bottom: 2rem;
}

.post-body {
    color: #333;
    line-height: 1.6;
    white-space: pre-line; /* Preserve line breaks from textarea */
}

.post-additional-info {
    margin-top: 2rem;
    padding-top: 1.5rem;
    border-top: 1px solid #ddd;
}

h3 {
    margin-bottom: 0.75rem;
    color: #555;
}
</style>