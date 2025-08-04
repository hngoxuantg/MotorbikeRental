<script setup>
import { useRouter } from 'vue-router'
import { categoryService } from '@/api/services/categoryService'
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import CategoryCreate from '@/components/Admin/Motorbikes/CategoryCreate.vue'

const router = useRouter()

async function handleSubmit(formData) {
  try {
    await categoryService.createCategory(formData)
    alert('Thêm loại xe thành công!')
    router.push('/admin/categories')
  } catch (error) {
    console.error('Lỗi thêm loại xe:', error)
    alert('Có lỗi xảy ra khi thêm loại xe!')
  }
}
</script>

<template>
  <AdminLayout>
    <CategoryCreate @submit="handleSubmit" />
  </AdminLayout>
</template>