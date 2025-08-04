<script setup>
import EmployeeCreateUser from '../../../components/Admin/Employees/EmployeeCreateUser.vue'
import { employeeService } from '../../../api/services/employeeService'
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter();
const roles = ref([])
const employeeId = Number(route.params.id)
const form = ref({
  employeeId: employeeId,
  userName: '',
  roleId: null,
  password: '',
  confirmPassword: '',
  phoneNumber: '',
  email: '',
})
onMounted(async () => {
  try {
    roles.value = await employeeService.getRoles()
  } catch (error) {
    console.error('Error fetching roles:', error)
  }
})
async function handleSubmit(params) {
  try {
    await employeeService.createUserCredential(employeeId, params)
    alert('Tạo tài khoản người dùng cho nhân viên thành công!')
    router.push('/admin/employees')
  } catch (error) {
    console.error('Error creating user credential:', error)
    alert('Đã xảy ra lỗi khi tạo tài khoản người dùng. Vui lòng thử lại sau.')
  }
}
</script>
<template>
  <AdminLayout>
    <EmployeeCreateUser :roles="roles" :form="form" @submit="handleSubmit" />
  </AdminLayout>
</template>
<script></script>
