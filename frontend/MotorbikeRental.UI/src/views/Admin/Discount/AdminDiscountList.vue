<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import DiscountList from '../../../components/Admin/Discount/DiscountList.vue'
import { onMounted, ref } from 'vue'
import { discountService } from '@/api/services/discountService'
import { useRouter } from 'vue-router'

const router = useRouter()
const discounts = ref([])
const isLoading = ref(true)
const query = ref({
  Search: '',
  PageNumber: 1,
  PageSize: 12,
  FilterStartDate: null,
  FilterEndDate: null,
  IsActive: null,
})
const totalCount = ref(0)

onMounted(async () => {
    try{
        const response = await discountService.getAll(query.value)
        discounts.value = response.data.data; // Lấy data từ response
        totalCount.value = response.data.totalCount;
    }
    catch (error) {
        console.error('Error fetching discounts:', error)
    } finally {
        isLoading.value = false
    }
})

const updateQuery = async (newQuery) => {
    Object.assign(query.value, newQuery)
    isLoading.value = true
    try {
        const response = await discountService.getAll(query.value)
        discounts.value = response.data.data
        totalCount.value = response.data.totalCount
    } catch (error) {
        console.error('Error updating query:', error)
    } finally {
        isLoading.value = false
    }
}

const handleCreateDiscount = () => {
    router.push({ name: 'AdminDiscountCreate' })
}
</script>
<template>
  <AdminLayout>
    <DiscountList 
      :discounts="discounts" 
      :isLoading="isLoading" 
      :totalCount="totalCount" 
      :query="query"
      @updateQuery="updateQuery"
      @create-discount="handleCreateDiscount"
    />
  </AdminLayout>
</template>