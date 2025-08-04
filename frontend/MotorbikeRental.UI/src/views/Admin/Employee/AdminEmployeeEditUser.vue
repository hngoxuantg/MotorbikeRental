<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import EmployeeEditUser from '../../../components/Admin/Employees/EmployeeEditUser.vue'
import { onMounted, ref } from 'vue'
import { userCredentialsService } from '@/api/services/userCredentialsService'
import { useRoute, useRouter } from 'vue-router'
import { employeeService } from '../../../api/services/employeeService'

const route = useRoute()
const router = useRouter()
const userCredentials = ref(null)
const roles = ref(null)
const isLoading = ref(true)
const employeeId = route.params.id

onMounted(async () => {
  isLoading.value = true;
  try {
    const [user, fetchedRoles] = await Promise.all([
      userCredentialsService.getById(employeeId),
      employeeService.getRoles()
    ]);
    userCredentials.value = user.data;
    roles.value = fetchedRoles;
  } catch (error) {
    console.error('Error fetching data:', error);
  } finally {
    isLoading.value = false;
  }
});

async function resetEmail(params) {
  try {
    params.employeeId = employeeId;
    await userCredentialsService.editEmail(employeeId, params)
    alert('Email đã được cập nhật thành công!')
    window.location.reload()
  } catch (error) {
    console.error('Error updating email:', error)
    alert('Cập nhật email thất bại!')
  }
}
async function resetUserName(params) {
  try {
    params.employeeId = employeeId;
    await userCredentialsService.editUserName(employeeId, params)
    alert('Tên đăng nhập đã được cập nhật thành công!')
    window.location.reload()
  } catch (error) {
    console.error('Error updating username:', error)
    alert('Cập nhật tên đăng nhập thất bại!')
  }
}
async function resetPassword(params) {
  try {
    params.employeeId = employeeId;
    await userCredentialsService.editPassword(employeeId, params)
    alert('Mật khẩu đã được cập nhật thành công!')
    window.location.reload()
  } catch (error) {
    console.error('Error updating password:', error)
    alert('Cập nhật mật khẩu thất bại!')
  }
}
async function resetPhoneNumber(params) {
  try {
    params.employeeId = employeeId;
    await userCredentialsService.editPhoneNumber(employeeId, params)
    alert('Số điện thoại đã được cập nhật thành công!')
    window.location.reload()
  } catch (error) {
    console.error('Error updating phone number:', error)
    alert('Cập nhật số điện thoại thất bại!')
  }
}
async function resetRole(params) {
  try {
    params.employeeId = employeeId;
    await userCredentialsService.editRole(employeeId, params)
    alert('Vai trò đã được cập nhật thành công!')
    window.location.reload()
  } catch (error) {
    console.error('Error updating role:', error)
    alert('Cập nhật vai trò thất bại!')
  }
}
async function deleteUser() {
  try{
    await userCredentialsService.deleteUserCredentials(employeeId);
    alert('Tài khoản đã được xóa thành công!')
    router.push('/admin/employees');
  } catch (error) {
    console.error('Error deleting user:', error);
    alert('Xóa tài khoản thất bại!');
  }
}
</script>
<template>
  <AdminLayout>
    <EmployeeEditUser
        v-if="!isLoading && userCredentials"
        :user-credentials="userCredentials"
        :roles="roles"
        @reset-email="resetEmail"
        @reset-username="resetUserName"
        @reset-password="resetPassword"
        @reset-phone-number="resetPhoneNumber"
        @reset-role="resetRole"wwwwwwwwwwwwwwwwwwwwwwwwwwww
        @delete-user="deleteUser"
     />
  </AdminLayout>
</template>
