<script setup>
import { ref, onMounted } from 'vue'
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { useRoute, useRouter } from 'vue-router'
import { incidentService } from '@/api/services/incidentService'
import DetailIncident from '@/components/Receptionist/Incident/DetailIncident.vue'

const incident = ref(null)
const route = useRoute()
const router = useRouter()
const isLoading = ref(true)
onMounted(async () => {
  const contractId = route.params.contractId
  try {
    isLoading.value = true
    const response = await incidentService.getByContractId(contractId)
    incident.value = response.data
  } catch (error) {
    console.error('Error fetching incident details:', error)
  } finally {
    isLoading.value = false
  }
})
</script>
<template>
    <AdminLayout>
        <DetailIncident :incident="incident" :isLoading="isLoading" />
    </AdminLayout>
</template>