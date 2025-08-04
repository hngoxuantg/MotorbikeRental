<script setup>
import MaintenanceLayout from '@/views/layouts/Maintenance/MaintenanceLayout.vue'
import MotorbikeList from '@/components/Maintenance/MaintenanceRecord/MotorbikeList.vue'
import { onMounted, ref } from 'vue'
import { maintenanceService } from '@/api/services/maintenanceService'
const motorbikes = ref([])
const isLoading = ref(true)
onMounted(async () => {
  try {
    const response = await maintenanceService.getMotorbikes()
    motorbikes.value = response.data
  } catch (error) {
    console.error('Error fetching motorbikes:', error)
  } finally {
    isLoading.value = false
  }
})

const updateQuery = async (newQuery) => {
  Object.assign(motorbikes.value, newQuery)
  isLoading.value = true
  try {
    const response = await maintenanceService.getMotorbikes(newQuery)
    motorbikes.value = response.data
  } catch (error) {
    console.error('Error updating motorbikes:', error)
  } finally {
    isLoading.value = false
  }
}
</script>
<template>
  <MaintenanceLayout>
    <MotorbikeList :motorbikes="motorbikes" :isLoading="isLoading" @updateQuery="updateQuery" />
  </MaintenanceLayout>
</template>