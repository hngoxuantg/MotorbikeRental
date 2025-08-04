<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { customerService } from '../../../api/services/customerService';
import { onMounted, ref } from 'vue'
import CustomerList from '../../../components/Admin/Customers/CustomerTable.vue'

const customers = ref([]);
const isLoading = ref(true);
const query = ref({
    Search: '',
    PageNumber: 1,
    PageSize: 12,
});
const totalCount = ref(0);

onMounted(async () => {
    try{
        const response = await customerService.getAll(query.value);
        customers.value = response.data.data;
        totalCount.value = response.data.totalCount;
        console.log('Customers fetched successfully:', customers.value);
    } catch (error) {
        console.error('Error fetching customers:', error);
    } finally {
        isLoading.value = false;
    }
})

const updateQuery = async (newQuery) => {
    Object.assign(query.value, newQuery);
    isLoading.value = true;
    try {
        const response = await customerService.getAll(query.value);
        customers.value = response.data.data;
        totalCount.value = response.data.totalCount;
    } catch (error) {
        console.error('Error updating query:', error);
    } finally {
        isLoading.value = false;
    }
}
</script>
<template>
    <AdminLayout>
        <CustomerList 
        :customers="customers"
        :isLoading="isLoading" 
        :totalCount="totalCount"
        :query="query"
        @update-query="updateQuery"
         />
    </AdminLayout>
</template>