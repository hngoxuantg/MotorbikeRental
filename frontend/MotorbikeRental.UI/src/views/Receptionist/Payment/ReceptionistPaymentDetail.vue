<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { ref, onMounted } from 'vue'
import { paymentService } from '@/api/services/paymentService'
import { useRoute, useRouter } from 'vue-router'
import { jwtDecode } from 'jwt-decode'
import PaymentDetail from '@/components/Receptionist/Payment/PaymentDetail.vue' 

const route = useRoute()
const router = useRouter()
const payment = ref(null)
const isLoading = ref(true)
const contractId = route.params.id

onMounted(async () => {
  try {
    isLoading.value = true
    const response = await paymentService.getById(contractId)
    payment.value = response.data
  } catch (error) {
    console.error('Error fetching payment details:', error)
  } finally {
    isLoading.value = false
  }
})
function goBack() {
  window.history.back()
}
function goIncident(){
  router.push({ name: 'ReceptionistIncidentList', params: { contractId: contractId } })
}
</script>
<template>
    <ReceptionistLayout>
        <PaymentDetail 
            :payment="payment" 
            :isLoading="isLoading" 
            @goBack="goBack" 
            @goIncident="goIncident"
        />
    </ReceptionistLayout>
</template>