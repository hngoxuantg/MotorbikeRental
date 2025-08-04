<script setup>
import ContractDetailVue from '@/components/Receptionist/Contract/ContractDetail.vue'
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { ref, onMounted } from 'vue'
import { contractService } from '@/api/services/contractService'
import { useRoute, useRouter } from 'vue-router'
import { contractStorage } from '@/utils/ContractStorageUtils'

const contract = ref(null)
const route = useRoute()
const router = useRouter()
const isLoading = ref(true)
onMounted(async () => {
  const contractId = route.params.id
  try {
    const response = await contractService.getById(contractId)
    contract.value = response.data
  } catch (error) {
    console.error('Error fetching contract details:', error)
  } finally {
    isLoading.value = false
  }
})

function goEdit() {
  contractStorage.setContract(contract.value.contractId)
  contractStorage.setMotorbikeRental(contract.value.motorbikeId)
  router.push({
    name: 'ReceptionistUpdateBeforeContract',
    params: { id: contract.value.contractId },
  })
}
async function cancelledContract() {
  try {
    await contractService.cancelContractByCustomer(contract.value.contractId)
    alert('Hợp đồng đã được hủy')
    router.push({ name: 'ReceptionistListContract' })
  } catch (error) {
    console.error('Error cancelling contract:', error)
  }
}
async function activeContract() {
  try {
    await contractService.updateContractStatusActive(contract.value.contractId)
    alert('Hợp đồng đã được kích hoạt')
    router.push({ name: 'ReceptionistListContract' })
  } catch (error) {
    console.error('Error activating contract:', error)
    alert('Lỗi khi kích hoạt hợp đồng: Vui lòng thử lại')
  }
}
function settlementContract() {
  router.push({ name: 'ReceptionistSettlementContract', params: { id: contract.value.contractId } })
}

function addIncident(){
  router.push({ name: 'ReceptionistCreateIncident', params: { contractId: contract.value.contractId } })
}
function viewInvoice() {
  router.push({ name: 'ReceptionistPaymentDetail', params: { id: contract.value.contractId } })
}
function goToPayment() {
  router.push(`/Receptionist/payment/process/${contract.value.contractId}`)
}
</script>
<template>
  <ReceptionistLayout>
    <ContractDetailVue
      :contract="contract"
      :isLoading="isLoading"
      @edit="goEdit"
      @delete="deleteContract"
      @cancel="cancelledContract"
      @activate="activeContract"
      @settle="settlementContract"
      @goToPayment="goToPayment"
      @addIncident="addIncident"
      @viewInvoice="viewInvoice"
      @go-to-payment="goToPayment"
    />
  </ReceptionistLayout>
</template>
