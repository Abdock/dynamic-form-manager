<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { usePostsStore } from '../stores/posts';

const router = useRouter();
const postsStore = usePostsStore();

const title = ref('');
const content = ref('');
const createError = ref('');
const isSubmitting = ref(false);

const createPost = async () => {
    if (!title.value || !content.value) {
        createError.value = 'Please fill in all fields';
        return;
    }

    isSubmitting.value = true;
    createError.value = '';

    try {
        // Create a JSON structure for the post
        const postNode = {
            title: title.value,
            content: content.value,
            createdAt: new Date().toISOString()
        };

        const result = await postsStore.createPost(postNode);

        if (result) {
            router.push('/posts');
        } else {
            createError.value = postsStore.error || 'Failed to create post';
        }
    } catch (error) {
        createError.value = error.message || 'An unexpected error occurred';
    } finally {
        isSubmitting.value = false;
    }
};

const cancelCreate = () => {
    router.push('/posts');
};
</script>

<template>
    <div class="create-post-container">
        <h1>Create Post</h1>

        <div v-if="createError" class="error-message">
            {{ createError }}
        </div>

        <form @submit.prevent="createPost" class="create-post-form">
            <div class="form-group">
                <label for="title">Title</label>
                <input
                    id="title"
                    v-model="title"
                    type="text"
                    placeholder="Enter post title"
                    required
                />
            </div>

            <div class="form-group">
                <label for="content">Content</label>
                <textarea
                    id="content"
                    v-model="content"
                    placeholder="Write your post content here..."
                    rows="8"
                    required
                ></textarea>
            </div>

            <div class="form-actions">
                <button
                    type="button"
                    @click="cancelCreate"
                    class="btn-cancel"
                >
                    Cancel
                </button>

                <button
                    type="submit"
                    :disabled="isSubmitting"
                    class="btn-submit"
                >
                    {{ isSubmitting ? 'Creating...' : 'Create Post' }}
                </button>
            </div>
        </form>
    </div>
</template>

<style scoped>
.create-post-container {
    max-width: 800px;
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

.create-post-form {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
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

input, textarea {
    padding: 0.75rem;
    border-radius: 4px;
    border: 1px solid #ccc;
    font-size: 1rem;
    font-family: inherit;
}

textarea {
    resize: vertical;
}

.form-actions {
    display: flex;
    justify-content: space-between;
    margin-top: 1rem;
}

.btn-cancel {
    background-color: #f8f9fa;
    color: #555;
    border: 1px solid #ccc;
    padding: 0.75rem 1.5rem;
    border-radius: 4px;
    font-size: 1rem;
    cursor: pointer;
    transition: background-color 0.2s;
}

.btn-cancel:hover {
    background-color: #e9ecef;
}

.btn-submit {
    background-color: #4caf50;
    color: white;
    border: none;
    padding: 0.75rem 1.5rem;
    border-radius: 4px;
    font-size: 1rem;
    cursor: pointer;
    transition: background-color 0.2s;
}

.btn-submit:hover {
    background-color: #3e8e41;
}

.btn-submit:disabled {
    background-color: #9fd4a1;
    cursor: not-allowed;
}

.error-message {
    background-color: #fee;
    color: #e44;
    padding: 0.75rem;
    border-radius: 4px;
    margin-bottom: 1rem;
    text-align: center;
}
</style>