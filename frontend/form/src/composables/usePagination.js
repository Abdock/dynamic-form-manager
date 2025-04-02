import { ref, computed } from 'vue';

export function usePagination(itemsFetcher, options = {}) {
    const { defaultPage = 1, defaultPageSize = 10 } = options;

    const currentPage = ref(defaultPage);
    const pageSize = ref(defaultPageSize);
    const totalItems = ref(0);
    const loading = ref(false);
    const error = ref(null);

    const totalPages = computed(() => Math.ceil(totalItems.value / pageSize.value) || 1);
    const currentSkip = computed(() => (currentPage.value - 1) * pageSize.value);

    const setPage = (page) => {
        if (page < 1 || page > totalPages.value) return;
        currentPage.value = page;
        fetchItems();
    };

    const nextPage = () => {
        if (currentPage.value < totalPages.value) {
            setPage(currentPage.value + 1);
        }
    };

    const prevPage = () => {
        if (currentPage.value > 1) {
            setPage(currentPage.value - 1);
        }
    };

    const setPageSize = (size) => {
        pageSize.value = size;
        currentPage.value = 1; // Reset to first page when changing page size
        fetchItems();
    };

    const fetchItems = async () => {
        if (!itemsFetcher) return;

        loading.value = true;
        error.value = null;

        try {
            const result = await itemsFetcher(currentSkip.value, pageSize.value);
            if (result && typeof result.total === 'number') {
                totalItems.value = result.total;
            }
            return result;
        } catch (err) {
            error.value = err.message || 'Failed to fetch items';
            console.error('Pagination error:', err);
            return null;
        } finally {
            loading.value = false;
        }
    };

    return {
        currentPage,
        pageSize,
        totalItems,
        totalPages,
        currentSkip,
        loading,
        error,
        setPage,
        nextPage,
        prevPage,
        setPageSize,
        fetchItems
    };
}