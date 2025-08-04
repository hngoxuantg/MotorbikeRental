<script setup>
import MaintenanceLayout from '@/views/layouts/Maintenance/MaintenanceLayout.vue'
import MaintenanceHistory from '@/components/Maintenance/MaintenanceRecord/MaintenanceHistory.vue'
import { computed, onMounted, ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { maintenanceService } from '@/api/services/maintenanceService'

const router = useRouter()
const route = useRoute()

const history = ref([])
const isLoading = ref(true)

onMounted(async () => {
  try {
    const response = await maintenanceService.getHistory()
    history.value = response.data
  } catch (error) {
    console.error('Error fetching maintenance history:', error)
  } finally {
    isLoading.value = false
  }
})

const updateQuery = async (newQuery) => {
  Object.assign(history.value, newQuery)
  isLoading.value = true
  try {
    const response = await maintenanceService.getHistory(newQuery)
    history.value = response.data
  } catch (error) {
    console.error('Error updating query:', error)
  } finally {
    isLoading.value = false
  }
}
</script>
<template>
  <MaintenanceLayout>
    <MaintenanceHistory :history="history" :isLoading="isLoading" @updateQuery="updateQuery"/>
  </MaintenanceLayout>
</template>