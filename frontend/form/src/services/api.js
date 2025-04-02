const API_URL = '/api';

export const api = {
    async login(email, password) {
        try {
            const response = await fetch(`${API_URL}/users/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            const data = await response.json();
            if (!data.isSuccess) {
                throw new Error('Invalid credentials');
            }

            return data.response;
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    },

    async register(email, username, password) {
        try {
            const response = await fetch(`${API_URL}/users/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, username, password }),
            });

            const data = await response.json();
            if (!data.isSuccess) {
                throw new Error('Registration failed');
            }

            return data.response;
        } catch (error) {
            console.error('Registration error:', error);
            throw error;
        }
    },

    async getUserInfo() {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error('No authentication token found');
            }

            const response = await fetch(`${API_URL}/users/me`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });

            const data = await response.json();
            if (!data.isSuccess) {
                throw new Error('Failed to get user info');
            }

            return data.response;
        } catch (error) {
            console.error('Get user info error:', error);
            throw error;
        }
    },

    // Posts endpoints
    async getPosts(skip = 0, take = 10, searchText = '') {
        try {
            let url = `${API_URL}/posts?skip=${skip}&take=${take}`;

            if (searchText) {
                url += `&searchText=${searchText}`;
            }

            const response = await fetch(url);
            const data = await response.json();

            return data;
        } catch (error) {
            console.error('Get posts error:', error);
            throw error;
        }
    },

    async createPost(node) {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error('No authentication token found');
            }

            const response = await fetch(`${API_URL}/posts`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                },
                body: JSON.stringify({ node }),
            });

            const data = await response.json();
            if (!data.isSuccess) {
                throw new Error('Failed to create post');
            }

            return data.response;
        } catch (error) {
            console.error('Create post error:', error);
            throw error;
        }
    },

    async getPostById(id) {
        try {
            const response = await fetch(`${API_URL}/posts/${id}`);
            const data = await response.json();

            if (!data.isSuccess) {
                throw new Error('Failed to get post');
            }

            return data.response;
        } catch (error) {
            console.error('Get post error:', error);
            throw error;
        }
    }
};