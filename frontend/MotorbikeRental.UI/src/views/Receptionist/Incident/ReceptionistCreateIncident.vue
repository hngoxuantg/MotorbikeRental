<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import CreateIncident from '../../../components/Receptionist/Incident/CreateIncident.vue'
import { incidentService } from '@/api/services/incidentService'
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { jwtDecode } from 'jwt-decode'

const route = useRoute()
const router = useRouter()
const isLoading = ref(false)

const contractId = parseInt(route.params.contractId)
const employeeId = parseInt(jwtDecode(localStorage.getItem('token')).sub)

// ✅ SỬA: Function đơn giản chỉ nhận data và gửi API
async function handleSubmit(incidentData) {
  try {
    isLoading.value = true
    
    // ✅ Debug: Log data nhận được từ component con
    console.log('📤 Data received from CreateIncident:', incidentData)
    console.log('📤 Images:', incidentData.images)
    console.log('📤 Images length:', incidentData.images?.length)
    
    // ✅ Thêm contractId và employeeId vào data
    const submitData = {
      ...incidentData,
      contractId: contractId,
      reportedByEmployeeId: employeeId
    }
    
    console.log('📤 Final submit data:', submitData)
    
    // ✅ Gửi API
    const response = await incidentService.create(submitData)
    
    console.log('✅ Success response:', response)
    
    router.push(`/receptionist/contract/detail/${contractId}`)
    
  } catch (error) {
    console.error('❌ Error creating incident:', error)
    alert('Có lỗi xảy ra khi tạo sự cố: ' + (error.response?.data?.message || error.message))
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <ReceptionistLayout>
    <CreateIncident :isLoading="isLoading" @submit="handleSubmit" />
  </ReceptionistLayout>
</template>