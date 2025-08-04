<script setup>
import MaintenanceLayout from '@/views/layouts/Maintenance/MaintenanceLayout.vue'
import MaintenanceRepairs from '@/components/Maintenance/MaintenanceRecord/MaintenanceRepairs.vue'
import { onMounted, ref } from 'vue'
import { maintenanceService } from '@/api/services/maintenanceService'
const motorbikes = ref([])
const isLoading = ref(true)
onMounted(async () => {
  try {
    const params = {
        Status: 2,
    }
    const response = await maintenanceService.getMotorbikes(params)
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
    const fullParams = {
      ...newQuery,
      Status: 2, 
    }
    const response = await maintenanceService.getMotorbikes(fullParams)
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
    <MaintenanceRepairs :motorbikes="motorbikes" :isLoading="isLoading" @updateQuery="updateQuery" />
  </MaintenanceLayout>
</template>