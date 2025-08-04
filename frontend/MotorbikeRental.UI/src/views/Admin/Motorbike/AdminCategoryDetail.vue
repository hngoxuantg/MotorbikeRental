<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { categoryService } from '@/api/services/categoryService'
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import CategoryDetail from '@/components/Admin/Motorbikes/CategoryDetail.vue'

const route = useRoute()
const router = useRouter()
const categoryId = ref(null)
const category = ref(null)

onMounted(async () => {
  categoryId.value = route.params.id
  try {
    const res = await categoryService.getById(categoryId.value)
    category.value = res.data
  } catch (error) {
    console.error('Lỗi lấy thông tin loại xe:', error)
  }
})

async function updateCategory(params) {
  try {
    await categoryService.updateCategory(categoryId.value, params)
    alert('Cập nhật loại xe thành công!')
    const res = await categoryService.getById(categoryId.value)
    category.value = res.data
  } catch (error) {
    console.error('Lỗi cập nhật loại xe:', error)
    alert('Có lỗi xảy ra khi cập nhật!')
  }
}

async function deleteCategory(id) {
  try {
    await categoryService.deleteCategory(id)
    alert('Xóa loại xe thành công!')
    router.push('/admin/categories')
  } catch (error) {
    console.error('Lỗi xóa loại xe:', error)
    alert('Có lỗi xảy ra khi xóa!')
  }
}
</script>

<template>
  <AdminLayout>
    <CategoryDetail 
      v-if="category"
      :category="category" 
      @update="updateCategory"
      @delete="deleteCategory"
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