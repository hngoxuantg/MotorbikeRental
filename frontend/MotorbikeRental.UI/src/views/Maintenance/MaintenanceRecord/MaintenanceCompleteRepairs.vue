<script setup>
import MaintenanceLayout from '@/views/layouts/Maintenance/MaintenanceLayout.vue'
import MaintenanceCompleteRepairs from '@/components/Maintenance/MaintenanceRecord/MaintenanceCompleteRepairs.vue'
import { maintenanceService } from '@/api/services/maintenanceService'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

async function completeMaintenance(formData) {
  try {
    const motorbikeId = parseInt(route.params.motorbikeId)
    const fullData = {
      ...formData,
      motorbikeId: motorbikeId,
    }
    await maintenanceService.completeMaintenance(fullData)
    alert('Bảo dưỡng đã được hoàn tất thành công')
    router.push('/maintenance/repairs') 
  } catch (error) {
    console.error('Error completing maintenance:',  error)
    alert('Lỗi khi hoàn tất bảo dưỡng: ' + (error.response?.data?.message || 'Vui lòng thử lại'))
  }
}
</script>

<template>
  <MaintenanceLayout>
    <MaintenanceCompleteRepairs :onComplete="completeMaintenance"/>
  </MaintenanceLayout>
</template>