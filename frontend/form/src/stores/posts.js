import { ref } from 'vue';
import { defineStore } from 'pinia';
import { api } from '../services/api';

export const usePostsStore = defineStore('posts', () => {
    const posts = ref([]);
    const totalPosts = ref(0);
    const loading = ref(false);
    const error = ref(null);
    const currentPost = ref(null);

    async function fetchPosts(skip = 0, take = 10, searchText = '') {
        loading.value = true;
        error.value = null;

        try {
            const result = await api.getPosts(skip, take, searchText);
            posts.value = result.data;
            totalPosts.value = result.total;
            return result;
        } catch (err) {
            error.value = err.message || 'Failed to fetch posts';
            return null;
        } finally {
            loading.value = false;
        }
    }

    async function createPost(nodeData) {
        loading.value = true;
        error.value = null;

        try {
            const node = JSON.parse(JSON.stringify(nodeData)); // Ensure the data is serializable
            const result = await api.createPost(node);
            return result;
        } catch (err) {
            error.value = err.message || 'Failed to create post';
            return null;
        } finally {
            loading.value = false;
        }
    }

    async function fetchPostById(id) {
        loading.value = true;
        error.value = null;
        currentPost.value = null;

        try {
            const result = await api.getPostById(id);
            currentPost.value = result;
            return result;
        } catch (err) {
            error.value = err.message || 'Failed to fetch post';
            return null;
        } finally {
            loading.value = false;
        }
    }

    return {
        posts,
        totalPosts,
        loading,
        error,
        currentPost,
        fetchPosts,
        createPost,
        fetchPostById
    };
});