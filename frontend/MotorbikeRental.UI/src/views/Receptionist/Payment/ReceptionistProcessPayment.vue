<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { ref, onMounted } from 'vue'
import { paymentService } from '@/api/services/paymentService'
import { useRoute, useRouter } from 'vue-router'
import ProcessPayment from '@/components/Receptionist/Payment/ProcessPayment.vue'
import { jwtDecode } from 'jwt-decode'

const route = useRoute()
const router = useRouter()
const isLoading = ref(true)
const previewPayment = ref(null)

onMounted(async () => {
  try {
    const response = await paymentService.previewPayment(route.params.contractId)
    previewPayment.value = response.data
  } catch (error) {
    console.error('Error fetching payment details:', error)
    alert('Có lỗi xảy ra khi tải thông tin thanh toán')
  } finally {
    isLoading.value = false
  }
})

async function handlePaymentSuccess(formData) {
  try {
    const employeeToken = localStorage.getItem('token')
    const employeeId = jwtDecode(employeeToken).sub
    
    const paymentData = {
      contractId: route.params.contractId,
      paymentDate: formData.paymentDate,
      paymentStatus: formData.paymentStatus,
      employeeId: parseInt(employeeId)
    }

    await paymentService.processPayment(paymentData)
    alert('Thanh toán thành công!')
    router.push({ name: 'ReceptionistListContract' })
  } catch (error) {
    console.error('Error processing payment:', error)
    alert('Có lỗi xảy ra khi xử lý thanh toán: ' + (error.response?.data?.message || error.message))
  }
}
</script>

<template>
  <ReceptionistLayout>
    <ProcessPayment 
      :payment="previewPayment" 
      :isLoading="isLoading"
      @payment-success="handlePaymentSuccess" 
    />
  </ReceptionistLayout>
</template>