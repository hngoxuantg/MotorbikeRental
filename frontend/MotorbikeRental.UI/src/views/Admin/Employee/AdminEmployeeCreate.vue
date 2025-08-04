<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { employeeService } from '@/api/services/employeeService'
import { onMounted, reactive, ref } from 'vue'
import EmployeeCreate from '../../../components/Admin/Employees/EmployeeCreate.vue'
import { useRouter } from 'vue-router'

const route = useRouter()
const roles = ref([])
const form = reactive({
    FullName: '',
    DateOfBirth: '',
    Address: '',
    StartDate: '',
    Salary: '',
    Status: '',
    FormFile: null
})
onMounted(async () => {
    try {
        const response = await employeeService.getRoles()
        roles.value = response
    } catch (error) {
        console.error('Error fetching roles:', error)
    }
})
async function handleSubmit(params) {
    try{
        await employeeService.createEmployee(params)
        alert('Thêm nhân viên thành công!')
        goToCreateEmployee()
    }catch (error) {
        console.error('Error adding employee:', error)
        alert('Đã xảy ra lỗi khi thêm nhân viên. Vui lòng thử lại sau.')
    }
}
function goToCreateEmployee(){
  route.push('/admin/employees')
}

</script>
<template>
    <AdminLayout>
        <EmployeeCreate
            :form="form"
            :roles="roles"
            @submit="handleSubmit"
        />
    </AdminLayout> 
</template>
<style></style>