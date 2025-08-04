<script setup>
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { motorbikeService } from '@/api/services/motorbikeService'
import DetailMotorbikeCard from '@/components/Admin/Motorbikes/DetailMotorbikeCard.vue'
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { nextTick } from 'vue'

const route = useRoute()
const router = useRouter() 
const motorbike = ref(null)
const isLoading = ref(true)

onMounted(async () => {
  try {
    const id = route.params.id
    motorbike.value = await motorbikeService.getById(id)
  } catch (error) {
    console.error('Lỗi lấy chi tiết xe máy:', error)
  } finally {
    isLoading.value = false
  }
})
async function handleDelete(motorbike) {
  if (!motorbike?.motorbikeId) {
    alert('Không tìm thấy ID xe để xóa!')
    return
  }
  try {
    const res = await motorbikeService.deleteMotorbike(motorbike.motorbikeId)
    if (res?.success) {
      alert('Đã xóa xe thành công!')
      router.push('/admin/Index')
    } else {
      alert(res?.message || 'Xóa xe thất bại!')
    }
  } catch (error) {
    // Ưu tiên lấy message từ backend nếu có
    alert(error?.response?.data?.message || 'Xóa xe thất bại!')
  }
}

</script>
<template>
<AdminLayout>
  <div>
    <div v-if="isLoading">Đang tải...</div>
    <div v-else-if="motorbike">
      <DetailMotorbikeCard :motorbike="motorbike" @delete="handleDelete" />
    </div>
    <div v-else>
      <p>Không tìm thấy xe máy.</p>
    </div>
  </div>
</AdminLayout>
</template>