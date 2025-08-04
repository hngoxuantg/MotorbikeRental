<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { customerService } from '../../../api/services/customerService';
import CustomerDetail from '../../../components/Admin/Customers/CustomerDetail.vue';
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const customer = ref(null)
const isLoading = ref(true)

onMounted(async () => {
  isLoading.value = true;
  try {
    const customerId = route.params.id;
    const response = await customerService.getById(customerId);
    customer.value = response.data;
  } catch (error) {
    console.error('Error fetching customer details:', error);
  } finally {
    isLoading.value = false;
  }
});

async function updateCustomer(params) {
  isLoading.value = true;
  try {
    const response = await customerService.updateCustomer(route.params.id, params);
    customer.value = response.data;
    router.push(`/admin/customer/detail/${customer.value.customerId}`);
  } catch (error) {
    alert('Có lỗi xảy ra khi cập nhật thông tin khách hàng!');
    console.error('Error updating customer details:', error);
  } finally {
    isLoading.value = false;
  }
}

async function deleteCustomer() {
  isLoading.value = true;
  try {
    await customerService.deleteCustomer(route.params.id);
    router.push('/admin/customers');
  } catch (error) {
    console.error('Error deleting customer:', error);
  } finally {
    isLoading.value = false;
  }
}
</script>
<template>
  <AdminLayout>
    <CustomerDetail
      v-if="!isLoading && customer"
      :customer="customer"
      @update-customer="updateCustomer"
      @delete-customer="deleteCustomer"
     />
  </AdminLayout>
</template>