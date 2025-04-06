<!-- src/views/CreatePostView.vue -->
<script setup>
import {computed, reactive, ref} from 'vue';
import {useRouter} from 'vue-router';
import {usePostsStore} from '../stores/posts';
import CustomDatePicker from '../components/CustomDatePicker.vue';
import dayjs from 'dayjs';

const router = useRouter();
const postsStore = usePostsStore();

const title = ref('');
const content = ref('');
const createError = ref('');
const isSubmitting = ref(false);

const category = ref('general');
const publishDate = ref(dayjs().format('YYYY-MM-DD'));
const priority = ref('medium');
const tags = reactive({
    technology: false,
    business: false,
    science: false,
    health: false,
    education: false
});


const categoryOptions = [
    {value: 'general', label: 'General'},
    {value: 'announcement', label: 'Announcement'},
    {value: 'question', label: 'Question'},
    {value: 'tutorial', label: 'Tutorial'},
    {value: 'discussion', label: 'Discussion'}
];


const priorityOptions = [
    {value: 'low', label: 'Low'},
    {value: 'medium', label: 'Medium'},
    {value: 'high', label: 'High'},
    {value: 'urgent', label: 'Urgent'}
];


const today = computed(() => dayjs().format('YYYY-MM-DD'));

const createPost = async () => {
    if (!title.value || !content.value) {
        createError.value = 'Please fill in all required fields';
        return;
    }

    isSubmitting.value = true;
    createError.value = '';

    try {

        const selectedTags = Object.entries(tags)
            .filter(([_, selected]) => selected)
            .map(([tag, _]) => tag);


        const postNode = {
            title: title.value,
            content: content.value,
            category: category.value,
            publishDate: publishDate.value,
            priority: priority.value,
            tags: selectedTags,
            createdAt: dayjs().format()
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
            <!-- Title field -->
            <div class="form-group">
                <label for="title">Title <span class="required">*</span></label>
                <input
                    id="title"
                    v-model="title"
                    type="text"
                    placeholder="Enter post title"
                    required
                />
            </div>

            <!-- Select Category -->
            <div class="form-group">
                <label for="category">Category</label>
                <select
                    id="category"
                    v-model="category"
                    class="form-select"
                >
                    <option v-for="option in categoryOptions" :key="option.value" :value="option.value">
                        {{ option.label }}
                    </option>
                </select>
            </div>

            <!-- Date input with custom date picker -->
            <div class="form-group date-picker-container">
                <CustomDatePicker
                    v-model="publishDate"
                    label="Publication Date"
                    :min-date="today"
                    :disable-past-dates="true"
                    placeholder="Select publication date"
                />
            </div>

            <!-- Radio buttons for Priority -->
            <div class="form-group">
                <label>Priority</label>
                <div class="radio-group">
                    <div v-for="option in priorityOptions" :key="option.value" class="radio-option">
                        <input
                            type="radio"
                            :id="'priority-' + option.value"
                            :value="option.value"
                            v-model="priority"
                            name="priority"
                        />
                        <label :for="'priority-' + option.value">{{ option.label }}</label>
                    </div>
                </div>
            </div>

            <!-- Checkbox group for Tags -->
            <div class="form-group">
                <label>Tags</label>
                <div class="checkbox-group">
                    <div class="checkbox-option">
                        <input
                            type="checkbox"
                            id="tag-technology"
                            v-model="tags.technology"
                        />
                        <label for="tag-technology">Technology</label>
                    </div>
                    <div class="checkbox-option">
                        <input
                            type="checkbox"
                            id="tag-business"
                            v-model="tags.business"
                        />
                        <label for="tag-business">Business</label>
                    </div>
                    <div class="checkbox-option">
                        <input
                            type="checkbox"
                            id="tag-science"
                            v-model="tags.science"
                        />
                        <label for="tag-science">Science</label>
                    </div>
                    <div class="checkbox-option">
                        <input
                            type="checkbox"
                            id="tag-health"
                            v-model="tags.health"
                        />
                        <label for="tag-health">Health</label>
                    </div>
                    <div class="checkbox-option">
                        <input
                            type="checkbox"
                            id="tag-education"
                            v-model="tags.education"
                        />
                        <label for="tag-education">Education</label>
                    </div>
                </div>
            </div>

            <!-- Content field -->
            <div class="form-group">
                <label for="content">Content <span class="required">*</span></label>
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

.date-picker-container {
    margin-bottom: 0.5rem;
}

label {
    font-weight: 500;
    color: #555;
}

.required {
    color: #e44;
    margin-left: 4px;
}

input, textarea, select {
    padding: 0.75rem;
    border-radius: 4px;
    border: 1px solid #ccc;
    font-size: 1rem;
    font-family: inherit;
    background-color: white;
    color: black;
}

textarea {
    resize: vertical;
}

.form-select {
    cursor: pointer;
}

.radio-group {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
    margin-top: 0.5rem;
}

.radio-option {
    display: flex;
    align-items: center;
    gap: 0.5rem;
}

.radio-option input[type="radio"] {
    margin: 0;
    width: 18px;
    height: 18px;
    cursor: pointer;
}

.radio-option label {
    font-weight: normal;
    cursor: pointer;
}

/* Checkbox styling */
.checkbox-group {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
    margin-top: 0.5rem;
}

.checkbox-option {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    min-width: 120px;
}

.checkbox-option input[type="checkbox"] {
    margin: 0;
    width: 18px;
    height: 18px;
    cursor: pointer;
}

.checkbox-option label {
    font-weight: normal;
    cursor: pointer;
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

/* Responsive styling */
@media (max-width: 600px) {
    .radio-group, .checkbox-group {
        flex-direction: column;
        gap: 0.5rem;
    }

    .checkbox-option {
        min-width: auto;
    }
}
</style>