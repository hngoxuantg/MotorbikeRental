<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import SettlementContract from '@/components/Receptionist/Contract/SettlementContract.vue'
import { contractService } from '@/api/services/contractService'

const route = useRoute()
const contract = ref(null)
const isLoading = ref(true)
const router = useRouter()

// Fetch contract data
const fetchContract = async () => {
  const contractId = route.params.id
  try {
    isLoading.value = true
    const response = await contractService.getById(contractId)
    contract.value = response.data
  } catch (error) {
    console.error('Error fetching contract details:', error)
    alert('Lỗi khi tải thông tin hợp đồng')
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchContract()
})

async function handleSubmit(params) {
  try {
    isLoading.value = true
    
    const fullParams = {
      ...params,
      contractId: parseInt(route.params.id),
    }
    
    console.log('Submitting settlement:', fullParams)
    
    await contractService.contractSettlement(fullParams, route.params.id)
    
    alert('Thanh lý hợp đồng thành công!')
    
    await fetchContract()
    
  } catch (error) {
    console.error('Error settling contract:', error)
    alert('Lỗi khi thanh lý hợp đồng: ' + (error.message || 'Vui lòng thử lại'))
  } finally {
    isLoading.value = false
  }
}

function handleGoBack() {
  router.push({ name: 'ReceptionistListContract' })
}
</script>

<template>
  <ReceptionistLayout>
    <SettlementContract 
      :contract="contract" 
      :isLoading="isLoading" 
      @submit="handleSubmit" 
      @goBack="handleGoBack"
    />
  </ReceptionistLayout>
</template>