import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '../stores/auth';

const Home = () => import('../views/HomeView.vue');
const Login = () => import('../views/LoginView.vue');
const Register = () => import('../views/RegisterView.vue');
const PostsList = () => import('../views/PostsListView.vue');
const CreatePost = () => import('../views/CreatePostView.vue');
const PostDetail = () => import('../views/PostDetailView.vue');

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home
    },
    {
        path: '/login',
        name: 'login',
        component: Login,
        meta: { requiresGuest: true }
    },
    {
        path: '/register',
        name: 'register',
        component: Register,
        meta: { requiresGuest: true }
    },
    {
        path: '/posts',
        name: 'posts',
        component: PostsList
    },
    {
        path: '/posts/create',
        name: 'create-post',
        component: CreatePost,
        meta: { requiresAuth: true }
    },
    {
        path: '/posts/:id',
        name: 'post-detail',
        component: PostDetail,
        props: true
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach((to, from, next) => {
    const authStore = useAuthStore();
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
    const requiresGuest = to.matched.some(record => record.meta.requiresGuest);

    if (requiresAuth && !authStore.isAuthenticated) {
        next({ name: 'login', query: { redirect: to.fullPath } });
    } else if (requiresGuest && authStore.isAuthenticated) {
        next({ name: 'home' });
    } else {
        next();
    }
});

export default router;