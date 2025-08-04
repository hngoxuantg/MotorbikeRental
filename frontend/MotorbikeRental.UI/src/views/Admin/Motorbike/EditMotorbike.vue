<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import EditMotorbikeCard from '@/components/Admin/Motorbikes/EditMotorbikeCard.vue';
import { motorbikeService } from '@/api/services/motorbikeService';
import { onMounted, reactive, ref } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const motorbike = ref(null)
const isLoading = ref(true)

onMounted(async () => {
    const id = route.params.id
    try {
        motorbike.value = await motorbikeService.getById(id)
    } finally {
        isLoading.value = false
    }
})
</script>
<template>
    <AdminLayout>
    <div v-if="isLoading">Đang tải...</div>
    <div v-else-if="motorbike">
      <EditMotorbikeCard :motorbike="motorbike" />
    </div>
    <div v-else>
      <p>Không tìm thấy xe máy.</p>
    </div>
  </AdminLayout>
</template>