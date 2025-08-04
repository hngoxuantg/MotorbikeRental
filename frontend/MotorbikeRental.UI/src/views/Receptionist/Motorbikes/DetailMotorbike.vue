<script setup>
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { motorbikeService } from '@/api/services/motorbikeService'
import DetailMotorbikeCard from '@/components/Receptionist/Motorbikes/DetailMotorbikeCard.vue'
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'

const route = useRoute()
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

</script>
<template>
<ReceptionistLayout>
  <div>
    <div v-if="isLoading">Đang tải...</div>
    <div v-else-if="motorbike">
      <DetailMotorbikeCard :motorbike="motorbike" />
    </div>
    <div v-else>
      <p>Không tìm thấy xe máy.</p>
    </div>
  </div>
</ReceptionistLayout>
</template>