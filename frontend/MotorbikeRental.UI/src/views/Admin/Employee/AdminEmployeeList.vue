<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { employeeService } from '@/api/services/employeeService'
import { onMounted } from 'vue'
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import EmployeeTable from '@/components/Admin/Employees/EmployeeTable.vue'

const query = ref({
  Search: '',
  PageNumber: 1,
  PageSize: 12,
  RoleId: '',
  Status: '',
})
const roles = ref([])
const employees = ref([])
const totalCount = ref(0)

onMounted(async () => {
  try {
    roles.value = await employeeService.getRoles()
    const response = await employeeService.getAll(query.value)
    employees.value = response.data.data
    totalCount.value = response.data.totalCount
  } catch (error) {
    console.error('Error fetching roles:', error)
  }
})
const updateQuery = async (newQuery) => {
  Object.assign(query.value, newQuery)
  try {
    const newResponse = await employeeService.getAll(query.value)
    employees.value = newResponse.data.data
    totalCount.value = newResponse.data.totalCount
  } catch (error) {
    console.error('Error updating query:', error)
  }
}

</script>

<template>
  <AdminLayout>
    <EmployeeTable
      :employees="employees"
      :roles="roles"
      :totalCount="totalCount"
      :query="query"
      @update-query="updateQuery"
      @delete-employee="deleteEmployee"
    />
  </AdminLayout>
</template>
