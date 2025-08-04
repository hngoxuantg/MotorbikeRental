<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import CategoryList from '@/components/Admin/Motorbikes/CategoryList.vue'
import { onMounted, ref } from 'vue'
import { categoryService } from '@/api/services/categoryService'
import { useRouter } from 'vue-router'

const router = useRouter()
const categories = ref([])

onMounted(async () => {
  try {
    const res = await categoryService.getAll()
    categories.value = res.data
  } catch (error) {
    console.error('Lỗi lấy danh sách loại xe:', error)
  }
})

function handleCreateCategory() {
  router.push('/admin/category/create')
}
</script>

<template>
  <AdminLayout>
    <CategoryList
      :categories="categories"
      @create-category="handleCreateCategory"
    />
  </AdminLayout>
</template>