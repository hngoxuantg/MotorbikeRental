<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import ContractList from '../../../components/Receptionist/Contract/ContractList.vue'
import { ref, onMounted } from 'vue'
import { contractService } from '@/api/services/contractService'
import { useRouter } from 'vue-router'

const router = useRouter()
const contracts = ref([])
const query = ref({
  Search: '',
  PageNumber: 1,
  PageSize: 12,
  Status: '',
  Search: '',
  FromDate: '',
  ToDate: '',
})
const totalCount = ref(0)
const isLoading = ref(true)
onMounted(async () => {
  try {
    const response = await contractService.getAll(query.value)
    totalCount.value = response.data.totalCount
    contracts.value = response.data.data
    console.log('Contracts fetched successfully:', response.data.data)
  } catch (error) {
    console.error('Error fetching contracts:', error)
  } finally {
    isLoading.value = false
  }
})
const updateQuery = async (newQuery) => {
  Object.assign(query.value, newQuery)
  isLoading.value = true
  try {
    const response = await contractService.getAll(query.value)
    totalCount.value = response.data.totalCount
    contracts.value = response.data.data
  } catch (error) {
    console.error('Error updating query:', error)
  } finally {
    isLoading.value = false
  }
}
function goToDetail(contractId) {
  router.push(`/receptionist/contract/detail/${contractId}`)
}
</script>
<template>
  <ReceptionistLayout>
    <ContractList
      :contracts="contracts"
      :query="query"
      :totalCount="totalCount"
      :isLoading="isLoading"
      @update-query="updateQuery"
      @go-to-detail="goToDetail"
    />
  </ReceptionistLayout>
</template>