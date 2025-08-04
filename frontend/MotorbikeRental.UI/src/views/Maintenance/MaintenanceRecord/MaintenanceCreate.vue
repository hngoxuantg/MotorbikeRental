<script setup>
import MaintenanceLayout from '@/views/layouts/Maintenance/MaintenanceLayout.vue'
import MaintenanceCreate from '@/components/Maintenance/MaintenanceRecord/MaintenanceCreate.vue'
import { useRoute, useRouter } from 'vue-router'
import { maintenanceService } from '@/api/services/maintenanceService'
import { jwtDecode } from 'jwt-decode'

const route = useRoute()
const router = useRouter()

async function createMaintenanceRecord(dateString) {
  try {
    const token = localStorage.getItem('token')
    const employeeId = parseInt(jwtDecode(token).sub)
    const motorbikeId = parseInt(route.params.motorbikeId)
    
    const fullData = {
      motorbikeId: motorbikeId,
      maintenanceDate: new Date(dateString).toISOString(),
      employeeId: employeeId
    }
    
    await maintenanceService.createMaintenanceRecord(fullData)
    alert('Bảo dưỡng đã được tạo thành công')
    router.push('/maintenance/motorbikes')
  } catch (error) {
    console.error('Error creating maintenance record:', error)
    alert('Lỗi khi tạo bảo dưỡng: Vui lòng thử lại')
  }
}
</script>

<template>
  <MaintenanceLayout>
    <MaintenanceCreate @submit="createMaintenanceRecord" />
  </MaintenanceLayout>
</template>