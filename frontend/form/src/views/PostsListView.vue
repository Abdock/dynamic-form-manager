<!-- src/views/PostsListView.vue -->
<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { usePostsStore } from '../stores/posts';
import { useAuthStore } from '../stores/auth';

const router = useRouter();
const postsStore = usePostsStore();
const authStore = useAuthStore();

const searchText = ref('');
const currentPage = ref(1);
const itemsPerPage = ref(10);

const totalPages = computed(() => Math.ceil(postsStore.totalPosts / itemsPerPage.value));

// Calculate current skip value based on page and items per page
const currentSkip = computed(() => (currentPage.value - 1) * itemsPerPage.value);

// Format date to a more readable format
const formatDate = (dateString) => {
    const date = new Date(dateString);
    return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    }).format(date);
};

// Load posts on component mount
onMounted(async () => {
    await loadPosts();
});

// Load posts with pagination and optional search
const loadPosts = async () => {
    await postsStore.fetchPosts(currentSkip.value, itemsPerPage.value, searchText.value);
};

// Handle page change
const changePage = async (page) => {
    if (page < 1 || page > totalPages.value) return;

    currentPage.value = page;
    await loadPosts();

    // Scroll to top of the list
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

// Handle search
const handleSearch = async () => {
    currentPage.value = 1; // Reset to first page on new search
    await loadPosts();
};

// Navigate to create post page
const goToCreatePost = () => {
    router.push('/posts/create');
};

// Navigate to post details
const viewPostDetails = (postId) => {
    router.push(`/posts/${postId}`);
};
</script>

<template>
    <div class="posts-container">
        <div class="posts-header">
            <h1>Posts</h1>
            <button v-if="authStore.isAuthenticated" @click="goToCreatePost" class="btn-create">
                Create Post
            </button>
        </div>

        <!-- Search bar -->
        <div class="search-bar">
            <input
                v-model="searchText"
                type="text"
                placeholder="Search posts..."
                @keyup.enter="handleSearch"
            />
            <button @click="handleSearch" class="btn-search">Search</button>
        </div>

        <!-- Loading indicator -->
        <div v-if="postsStore.loading" class="loading-indicator">
            Loading posts...
        </div>

        <!-- Error message -->
        <div v-else-if="postsStore.error" class="error-message">
            {{ postsStore.error }}
        </div>

        <!-- No posts found -->
        <div v-else-if="postsStore.posts.length === 0" class="no-posts">
            No posts found.
            <div v-if="searchText" class="search-hint">
                Try adjusting your search criteria.
            </div>
        </div>

        <!-- Posts list -->
        <div v-else class="posts-list">
            <div
                v-for="post in postsStore.posts"
                :key="post.id"
                class="post-card"
                @click="viewPostDetails(post.id)"
            >
                <div class="post-content">
                    <div class="post-preview">
                        <!-- Display a preview of the post content -->
                        {{ JSON.stringify(post.node).substring(0, 100) }}...
                    </div>
                    <div class="post-date">
                        Posted on {{ formatDate(post.createdAt) }}
                    </div>
                </div>
            </div>
        </div>

        <!-- Pagination -->
        <div v-if="totalPages > 1" class="pagination">
            <button
                :disabled="currentPage === 1"
                @click="changePage(currentPage - 1)"
                class="pagination-btn"
            >
                Previous
            </button>

            <span class="page-info">
        Page {{ currentPage }} of {{ totalPages }}
      </span>

            <button
                :disabled="currentPage === totalPages"
                @click="changePage(currentPage + 1)"
                class="pagination-btn"
            >
                Next
            </button>
        </div>
    </div>
</template>

<style scoped>
.posts-container {
    max-width: 800px;
    margin: 2rem auto;
    padding: 1rem;
}

.posts-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1.5rem;
}

.btn-create {
    background-color: #4caf50;
    color: white;
    border: none;
    padding: 0.5rem 1rem;
    border-radius: 4px;
    font-size: 1rem;
    cursor: pointer;
    transition: background-color 0.2s;
}

.btn-create:hover {
    background-color: #3e8e41;
}

.search-bar {
    display: flex;
    margin-bottom: 1.5rem;
    gap: 0.5rem;
}

.search-bar input {
    flex: 1;
    padding: 0.75rem;
    border-radius: 4px;
    border: 1px solid #ccc;
    font-size: 1rem;
}

.btn-search {
    background-color: #646cff;
    color: white;
    border: none;
    padding: 0 1rem;
    border-radius: 4px;
    font-size: 1rem;
    cursor: pointer;
}

.loading-indicator, .error-message, .no-posts {
    text-align: center;
    padding: 2rem;
    background-color: #f8f9fa;
    border-radius: 8px;
}

.error-message {
    color: #e44;
    background-color: #fee;
}

.search-hint {
    margin-top: 0.5rem;
    font-size: 0.9rem;
    color: #888;
}

.posts-list {
    display: flex;
    flex-direction: column;
    gap: 1rem;
}

.post-card {
    background-color: #f8f9fa;
    border-radius: 8px;
    padding: 1.5rem;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    cursor: pointer;
    transition: transform 0.2s, box-shadow 0.2s;
}

.post-card:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
}

.post-preview {
    margin-bottom: 1rem;
    color: #333;
    font-size: 1rem;
    line-height: 1.4;
}

.post-date {
    color: #777;
    font-size: 0.9rem;
}

.pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 2rem;
    gap: 1rem;
}

.pagination-btn {
    background-color: #646cff;
    color: white;
    border: none;
    padding: 0.5rem 1rem;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.2s;
}

.pagination-btn:hover {
    background-color: #535bf2;
}

.pagination-btn:disabled {
    background-color: #a3a4d8;
    cursor: not-allowed;
}

.page-info {
    font-size: 0.9rem;
    color: #555;
}
</style>