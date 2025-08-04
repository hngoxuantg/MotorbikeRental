<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import CreateContract from '../../../components/Receptionist/Contract/CreateContract.vue'
import { onMounted, ref } from 'vue'
import { contractService } from '@/api/services/contractService'
import { motorbikeService } from '@/api/services/motorbikeService'
import { customerService } from '@/api/services/customerService'
import { discountService } from '@/api/services/discountService'
import { useRouter } from 'vue-router'
import { contractStorage } from '@/utils/ContractStorageUtils'
import { jwtDecode } from 'jwt-decode'

const router = useRouter()
const isLoading = ref(false)
const motorbike = ref(null)
const customer = ref(null)
const discounts = ref([])
const price = ref(null)

const motorbikeId = contractStorage.getMotorbike()
const customerId = contractStorage.getCustomer()

onMounted(async () => {
  isLoading.value = true
  try {
    if (motorbikeId) {
      motorbike.value = (await motorbikeService.getById(motorbikeId))
    }
  } catch (error) {
    console.error('Error fetching motorbike:', error)
  }
  try {
    if (customerId) {
      customer.value = (await customerService.getById(customerId)).data
    }
  } catch (error) {
    console.error('Error fetching customer:', error)
  }
  try {
    const params = {
      IsActive: true,
      CategoryId: motorbike.value.categoryId,
    }
    discounts.value = (await discountService.getAll(params)).data.data
  } catch (error) {
    console.error('Error fetching discounts:', error)
  }
  isLoading.value = false
})

async function calculatePrice(params) {
  try {
    const fullParams = {
      ...params,
      motorbikeId: motorbike.value.motorbikeId,
    }
    const result = await contractService.calculateContractPrice(fullParams)
    price.value = result.data
  } catch (error) {
    console.error('Error calculating price:', error)
    throw error
  }
}
async function createContract(params) {
  try {
    const fullParams = {
      ...params,
      customerId: customer.value.customerId,
      motorbikeId: motorbike.value.motorbikeId,
      employeeId: jwtDecode(localStorage.getItem('token')).sub,
    }
    const result = await contractService.createContract(fullParams)
    alert('Tạo hợp đồng thành công!')
    contractStorage.clear()
    router.push('/receptionist/contracts')
  } catch (error) {
    alert('Có lỗi xảy ra khi tạo hợp đồng!')
    console.error('Error creating contract:', error)
    throw error
  }
}
</script>
<template>
  <ReceptionistLayout>
    <CreateContract
      :motorbike="motorbike"
      :customer="customer"
      :discounts="discounts"
      :price="price"
      :isLoading="isLoading"
      @calculatePrice="calculatePrice"
      @createContract="createContract"
    />
    <div v-if="isLoading" class="loading">
      <p>Đang tải dữ liệu...</p>
    </div>
  </ReceptionistLayout>
</template>
<style scoped>
.loading {
  text-align: center;
  padding: 40px;
  font-size: 16px;
}
</style>"