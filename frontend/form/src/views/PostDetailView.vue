<!-- src/views/PostDetailView.vue -->
<script setup>
import { onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { usePostsStore } from '../stores/posts';

const route = useRoute();
const router = useRouter();
const postsStore = usePostsStore();

const postId = computed(() => route.params.id);

const hasAdditionalProperties = computed(() => {
    if (!postsStore.currentPost || !postsStore.currentPost.node) return false;

    const handledProperties = ['title', 'content', 'category', 'publishDate', 'priority', 'tags', 'createdAt'];

    return Object.keys(postsStore.currentPost.node).some(key => !handledProperties.includes(key));
});

const filteredAdditionalProperties = computed(() => {
    if (!postsStore.currentPost || !postsStore.currentPost.node) return {};

    const handledProperties = ['title', 'content', 'category', 'publishDate', 'priority', 'tags', 'createdAt'];
    const result = {};

    Object.entries(postsStore.currentPost.node).forEach(([key, value]) => {
        if (!handledProperties.includes(key)) {
            result[key] = value;
        }
    });

    return result;
});

const formatDate = (dateString) => {
    if (!dateString) return 'Unknown date';
    const date = new Date(dateString);
    return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    }).format(date);
};

const formatSimpleDate = (dateString) => {
    if (!dateString) return 'Unknown date';

    try {
        const date = new Date(dateString);

        if (isNaN(date.getTime())) {
            return 'Invalid date';
        }

        return new Intl.DateTimeFormat('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        }).format(date);
    } catch (error) {
        console.error('Error formatting date:', error);
        return 'Invalid date format';
    }
};

const formatCategory = (category) => {
    if (!category) return 'Unknown';
    return category.charAt(0).toUpperCase() + category.slice(1).replace(/-/g, ' ');
};

const formatPriority = (priority) => {
    if (!priority) return 'Unknown';
    return priority.charAt(0).toUpperCase() + priority.slice(1);
};

const formatTag = (tag) => {
    if (!tag) return '';
    return tag.charAt(0).toUpperCase() + tag.slice(1).replace(/-/g, ' ');
};

const formatPropertyName = (name) => {
    if (!name) return '';
    const result = name.replace(/([A-Z])/g, ' $1');
    return result.charAt(0).toUpperCase() + result.slice(1);
};

const formatPropertyValue = (value) => {
    if (value === null || value === undefined) return 'None';

    if (Array.isArray(value)) {
        return value.join(', ');
    }

    if (typeof value === 'object') {
        return JSON.stringify(value);
    }

    if (typeof value === 'boolean') {
        return value ? 'Yes' : 'No';
    }

    return value.toString();
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

            <div class="post-metadata-bar">
                <div class="metadata-item">
                    <span class="icon">📅</span>
                    <span>Posted: {{ formatDate(postsStore.currentPost.createdAt) }}</span>
                </div>

                <div v-if="postsStore.currentPost.node.publishDate" class="metadata-item">
                    <span class="icon">🗓️</span>
                    <span>Publish date: {{ formatSimpleDate(postsStore.currentPost.node.publishDate) }}</span>
                </div>

                <div v-if="postsStore.currentPost.node.category" class="metadata-item">
                    <span class="icon">📂</span>
                    <span>Category: <span class="category-badge">{{ formatCategory(postsStore.currentPost.node.category) }}</span></span>
                </div>

                <div v-if="postsStore.currentPost.node.priority" class="metadata-item">
                    <span class="icon">🔔</span>
                    <span>Priority: <span :class="'priority-badge priority-' + postsStore.currentPost.node.priority">
            {{ formatPriority(postsStore.currentPost.node.priority) }}
          </span></span>
                </div>
            </div>

            <!-- Tags -->
            <div v-if="postsStore.currentPost.node.tags && postsStore.currentPost.node.tags.length > 0" class="post-tags">
        <span v-for="tag in postsStore.currentPost.node.tags" :key="tag" class="tag">
          {{ formatTag(tag) }}
        </span>
            </div>

            <div class="post-body">
                {{ postsStore.currentPost.node.content }}
            </div>

            <!-- Display any other post properties that weren't explicitly handled -->
            <div
                v-if="hasAdditionalProperties"
                class="post-additional-info"
            >
                <h3>Additional Information</h3>
                <div
                    v-for="(value, key) in filteredAdditionalProperties"
                    :key="key"
                    class="additional-property"
                >
                    <strong>{{ formatPropertyName(key) }}:</strong>
                    <span>{{ formatPropertyValue(value) }}</span>
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

/* Updated metadata styling */
.post-metadata-bar {
    display: flex;
    flex-wrap: wrap;
    gap: 1.5rem;
    margin-bottom: 1.5rem;
    padding-bottom: 1rem;
    border-bottom: 1px solid #e5e5e5;
}

.metadata-item {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    color: #666;
    font-size: 0.9rem;
}

.icon {
    font-size: 1.2rem;
}

.category-badge {
    display: inline-block;
    background-color: #e6f7ff;
    color: #0066cc;
    padding: 0.2rem 0.5rem;
    border-radius: 4px;
    font-weight: 500;
}

.priority-badge {
    display: inline-block;
    padding: 0.2rem 0.5rem;
    border-radius: 4px;
    font-weight: 500;
}

.priority-low {
    background-color: #f0f9eb;
    color: #67c23a;
}

.priority-medium {
    background-color: #fdf6ec;
    color: #e6a23c;
}

.priority-high {
    background-color: #fef0f0;
    color: #f56c6c;
}

.priority-urgent {
    background-color: #f56c6c;
    color: white;
}

/* Tags styling */
.post-tags {
    display: flex;
    flex-wrap: wrap;
    gap: 0.5rem;
    margin-bottom: 1.5rem;
}

.tag {
    background-color: #f0f0f0;
    color: #555;
    padding: 0.3rem 0.7rem;
    border-radius: 16px;
    font-size: 0.85rem;
}

.post-body {
    color: #333;
    line-height: 1.6;
    white-space: pre-line; /* Preserve line breaks from textarea */
    margin-top: 1.5rem;
    margin-bottom: 1.5rem;
    padding: 1.5rem;
    background-color: white;
    border-radius: 8px;
    border: 1px solid #e5e5e5;
}

.post-additional-info {
    margin-top: 2rem;
    padding-top: 1.5rem;
    border-top: 1px solid #ddd;
}

.additional-property {
    margin-bottom: 0.75rem;
    display: flex;
    flex-direction: column;
    gap: 0.25rem;
}

h3 {
    margin-bottom: 1rem;
    color: #555;
    padding-bottom: 0.5rem;
    border-bottom: 1px solid #eee;
}

@media (max-width: 600px) {
    .post-metadata-bar {
        flex-direction: column;
        gap: 0.75rem;
    }
}
</style>