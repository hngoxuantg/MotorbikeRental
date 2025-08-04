<script setup>
import MaintenanceLayout from '@/views/layouts/Maintenance/MaintenanceLayout.vue'
import IncidentComplete from '@/components/Maintenance/Incident/IncidentComplete.vue'
import { onMounted, ref } from 'vue'
import { incidentService } from '@/api/services/incidentService'
import { useRoute, useRouter } from 'vue-router'

const router = useRouter()
const incidentHandleRef = ref(null)

const fetchIncident = async (incidentId) => {
  try {
    const response = await incidentService.getIncidentById(incidentId)
    incidentHandleRef.value?.setIncident(response.data)
  } catch (error) {
    console.error('Error fetching incident:', error)
    alert('Lỗi khi tải thông tin sự cố')
  }
}

const completeIncident = async (incidentId, formData) => {
  try {
    const fullData = {
      ...formData,
      incidentId: incidentId,
    }
    await incidentService.complete(incidentId, fullData)
    alert('Sự cố đã được xử lý thành công!')
    router.push('/maintenance/incidents')
  } catch (error) {
    console.error('Error completing incident:', error)
    alert('Lỗi khi xử lý sự cố: ' + (error.response?.data?.message || 'Vui lòng thử lại'))
  }
}
</script>
<template>
    <MaintenanceLayout>
        <IncidentComplete ref="incidentHandleRef"
      :onComplete="completeIncident"
      @fetchIncident="fetchIncident" />
    </MaintenanceLayout>
</template>