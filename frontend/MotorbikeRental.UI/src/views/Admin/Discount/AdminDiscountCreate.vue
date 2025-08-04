<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import DiscountCreate from '../../../components/Admin/Discount/DiscountCreate.vue'
import { onMounted, ref } from 'vue'
import { discountService } from '@/api/services/discountService'
import { categoryService } from '../../../api/services/categoryService'
import { useRouter } from 'vue-router'

const router = useRouter()
const isLoading = ref(false)
const categories = ref([])

onMounted(async () => {
  isLoading.value = true
  try {
    const response = await categoryService.getAll()
    categories.value = response.data
  } catch (error) {
    console.error('Error fetching categories:', error)
  } finally {
    isLoading.value = false
  }
})

async function createDiscount(formData) {
  isLoading.value = true
  try {
    await discountService.createDiscount(formData)
    alert('Tạo mã giảm giá thành công!')
    router.push('/admin/discounts')
  } catch (error) {
    console.error('Error creating discount:', error)
    alert('Có lỗi xảy ra khi tạo mã giảm giá!')
  } finally {
    isLoading.value = false
  }
}

function goBack() {
  router.push('/admin/discounts')
}
</script>

<template>
  <AdminLayout>
    <DiscountCreate
      :categories="categories"
      :isLoading="isLoading"
      @submit="createDiscount"
      @back="goBack"
    />
  </AdminLayout>
</template>