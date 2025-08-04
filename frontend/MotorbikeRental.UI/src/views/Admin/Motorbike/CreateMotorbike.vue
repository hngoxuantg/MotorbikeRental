<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import CreateMotorbikeForm from '@/components/Admin/Motorbikes/CreateMotorbikeForm.vue' 
import { motorbikeService } from '@/api/services/motorbikeService';
import { onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { categoryService } from '@/api/services/categoryService'



const router = useRouter()
const form = reactive({
  MotorbikeName: '',
  CategoryId: '',
  HourlyRate: '',
  DailyRate: '',
  LicensePlate: '',
  Brand: '',
  Year: '',
  Color: '',
  EngineCapacity: '',
  ChassisNumber: '',
  EngineNumber: '',
  Description: '',
  MotorbikeConditionStatus: '',
  Mileage: '',
  FormFile : null
})
const categories = ref([])
onMounted(async () => {
  try {
    const res = await categoryService.getAll()
    categories.value = res.data // lấy mảng data từ API
  } catch (error) {
    console.error('Lỗi lấy danh sách loại xe:', error)
  }
})
function goToCreateMotorbike(){
  router.push('/admin/index')
}
async function handleSubmit(formData) {
  try {
    await motorbikeService.createMotorbike(formData)
    alert('Thêm xe máy thành công!')
    goToCreateMotorbike()
  } catch (error) {
    console.error('Error adding motorbike:', error)
    alert('Đã xảy ra lỗi khi thêm xe máy. Vui lòng thử lại sau.')
  }
}
</script>

<template>
  <AdminLayout>
    <CreateMotorbikeForm  :form="form" :categories="categories" @submit="handleSubmit" />
  </AdminLayout>
</template>