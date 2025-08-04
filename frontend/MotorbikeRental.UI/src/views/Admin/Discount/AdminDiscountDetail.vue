<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import DiscountDetail from '../../../components/Admin/Discount/DiscountDetail.vue'
import { onMounted, ref } from 'vue'
import { discountService } from '@/api/services/discountService'
import { categoryService } from '../../../api/services/categoryService'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()
const discountId = ref(null)
const categories = ref([])
const discount = ref(null)

onMounted(async () => {
  discountId.value = route.params.id
  try {
    const res = await discountService.getById(discountId.value)
    discount.value = res.data
    const categoriesRes = await categoryService.getAll()
    categories.value = categoriesRes.data
  } catch (error) {
    console.error('Error fetching discount details:', error)
  }
})

async function updateDiscount(params) {
  try {
    await discountService.updateDiscount(discountId.value, params)
    alert('Cập nhật mã giảm giá thành công!')
    // Refresh data
    const res = await discountService.getById(discountId.value)
    discount.value = res.data
  } catch (error) {
    console.error('Error updating discount:', error)
    alert('Có lỗi xảy ra khi cập nhật!')
  }
}

async function deleteDiscount(id) {
  try {
    await discountService.deleteDiscount(id)
    alert('Xóa mã giảm giá thành công!')
    router.push('/admin/discounts')
  } catch (error) {
    console.error('Error deleting discount:', error)
    alert('Có lỗi xảy ra khi xóa!')
  }
}

function handleBack() {
  router.push('/admin/discounts')
}
</script>

<template>
  <AdminLayout>
    <DiscountDetail 
      v-if="discount"
      :discount="discount" 
      :categories="categories"
      @update="updateDiscount"
      @delete="deleteDiscount"
      @back="handleBack"
    />
    <div v-else class="loading">
      Đang tải...
    </div>
  </AdminLayout>
</template>

<style scoped>
.loading {
  text-align: center;
  padding: 50px;
  font-size: 16px;
  color: #6b7280;
}
</style>