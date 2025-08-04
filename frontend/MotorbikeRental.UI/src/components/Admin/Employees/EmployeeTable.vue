<script setup>
import { ref, watch } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'
import { useRouter } from 'vue-router'
import defaultAvatar from '@/assets/image.png'

const route = useRouter()
const props = defineProps({
  employees: {
    type: Array,
    required: true,
  },
  roles: {
    type: Array,
    required: true,
  },
  totalCount: {
    type: Number,
    default: 0,
  },
  query: {
    type: Object,
    required: true,
  },
})

const emit = defineEmits(['update-query', 'create-employee', 'create-account'])

const searchQuery = ref(props.query.Search || '')
const selectedRole = ref(props.query.RoleId || '')
const selectedStatus = ref(props.query.Status || '')

const statusOptions = {
  0: 'Đang làm',
  1: 'Xin nghỉ',
  2: 'Đã nghỉ',
}

let isUpdating = false

watch(() => props.query, (newQuery) => {
  isUpdating = true
  searchQuery.value = newQuery.Search || ''
  selectedRole.value = newQuery.RoleId || ''
  selectedStatus.value = newQuery.Status || ''
  setTimeout(() => { isUpdating = false }, 0)
}, { immediate: true })

watch([searchQuery, selectedRole, selectedStatus], () => {
  if (isUpdating) return
  
  emit('update-query', {
    Search: searchQuery.value || null,
    RoleId: selectedRole.value || null,
    Status: selectedStatus.value || null,
    PageNumber: 1,
    PageSize: 12
  })
})

function changePage(page) {
  emit('update-query', {
    Search: searchQuery.value || null,
    RoleId: selectedRole.value || null,
    Status: selectedStatus.value || null,
    PageNumber: page,
    PageSize: 12
  })
}

function clearFilters() {
  searchQuery.value = ''
  selectedRole.value = ''
  selectedStatus.value = ''
}

function createEmployee() {
  route.push('/admin/employees/create')
}

function createAccount(employeeId) {
  route.push('/admin/employees/create-user/' + employeeId)
}

function editEmployee(id) {
  route.push(`/admin/employees/edit/${id}`)
}

function formatSalary(salary) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
  }).format(salary)
}

function formatDateTime(dateStr) {
  if (!dateStr) return ''
  const date = new Date(dateStr)
  if (isNaN(date)) return ''
  return date.toLocaleDateString('vi-VN')
}

function getStatusText(status) {
  return statusOptions[status] || 'Không xác định'
}

function getStatusClass(status) {
  const classMap = {
    0: 'status-active',
    1: 'status-leave',
    2: 'status-inactive'
  }
  return classMap[status] || 'status-unknown'
}

const totalPages = Math.ceil(props.totalCount / 12)
</script>

<template>
  <div class="employee-container">
    <div class="header">
      <h1>Quản lý nhân viên</h1>
      <button @click="createEmployee" class="btn-primary">
        Thêm nhân viên
      </button>
    </div>

    <div class="filters">
      <input 
        v-model="searchQuery"
        type="text"
        placeholder="Tìm kiếm nhân viên..."
        class="search-input"
      />
      
      <select v-model="selectedRole" class="filter-select">
        <option value="">Tất cả quyền hạn</option>
        <option v-for="role in roles" :key="role.id" :value="role.id">
          {{ role.roleName }}
        </option>
      </select>
      
      <select v-model="selectedStatus" class="filter-select">
        <option value="">Tất cả trạng thái</option>
        <option v-for="(label, code) in statusOptions" :key="code" :value="code">
          {{ label }}
        </option>
      </select>
      
      <button @click="clearFilters" class="btn-clear">
        Xóa bộ lọc
      </button>
    </div>

    <div v-if="employees.length === 0" class="empty">
      <p>Không tìm thấy nhân viên nào</p>
    </div>

    <div v-else class="employees-grid">
      <div v-for="emp in employees" :key="emp.employeeId" class="employee-card">
        <div class="card-header">
          <div class="employee-avatar">
            <img
              :src="emp.avatar ? getFullPath(emp.avatar) : defaultAvatar"
              :alt="emp.fullName"
            />
          </div>
          <div class="employee-info">
            <h3>{{ emp.fullName }}</h3>
            <div class="employee-role">
              <span v-if="emp.roleName" class="role-badge">
                {{ emp.roleName }}
              </span>
              <span v-else class="role-badge no-account">
                Chưa có tài khoản
              </span>
            </div>
          </div>
        </div>

        <div class="card-body">
          <div class="info-row">
            <span class="label">Lương:</span>
            <span class="value">{{ formatSalary(emp.salary) }}</span>
          </div>
          <div class="info-row">
            <span class="label">Trạng thái:</span>
            <span class="value" :class="getStatusClass(emp.status)">
              {{ getStatusText(emp.status) }}
            </span>
          </div>
          <div v-if="emp.lastLogin" class="info-row">
            <span class="label">Đăng nhập cuối:</span>
            <span class="value">{{ formatDateTime(emp.lastLogin) }}</span>
          </div>
        </div>

        <div class="card-footer">
          <div class="actions">
            <button v-if="!emp.roleName" @click="createAccount(emp.employeeId)" class="btn-account">
              Tạo tài khoản
            </button>
            <button @click="editEmployee(emp.employeeId)" class="btn-edit">
              Chỉnh sửa
            </button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="totalPages > 1" class="pagination">
      <button 
        @click="changePage(props.query.PageNumber - 1)"
        :disabled="props.query.PageNumber === 1"
        class="page-btn"
      >
        ‹
      </button>
      
      <span class="page-info">
        {{ props.query.PageNumber }} / {{ totalPages }}
      </span>
      
      <button 
        @click="changePage(props.query.PageNumber + 1)"
        :disabled="props.query.PageNumber >= totalPages"
        class="page-btn"
      >
        ›
      </button>
    </div>
  </div>
</template>

<style scoped>
.employee-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.header h1 {
  margin: 0;
  font-size: 24px;
  color: #333;
}

.btn-primary {
  background: #007bff;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.btn-primary:hover {
  background: #0056b3;
}

.filters {
  display: flex;
  gap: 12px;
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 4px;
  border: 1px solid #ddd;
  flex-wrap: wrap;
}

.search-input {
  flex: 1;
  min-width: 200px;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.search-input:focus {
  outline: none;
  border-color: #007bff;
}

.filter-select {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  background: white;
}

.filter-select:focus {
  outline: none;
  border-color: #007bff;
}

.btn-clear {
  padding: 8px 16px;
  background: #f8f9fa;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.btn-clear:hover {
  background: #e9ecef;
}

.empty {
  text-align: center;
  padding: 40px;
  background: white;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.employees-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
}

.employee-card {
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  overflow: hidden;
}

.card-header {
  padding: 16px;
  background: #f8f9fa;
  border-bottom: 1px solid #ddd;
  display: flex;
  align-items: center;
  gap: 12px;
}

.employee-avatar img {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  object-fit: cover;
  border: 1px solid #ddd;
}

.employee-info {
  flex: 1;
}

.employee-info h3 {
  margin: 0 0 8px 0;
  font-size: 16px;
  color: #333;
}

.role-badge {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
  background: #e9ecef;
  color: #495057;
}

.role-badge.no-account {
  background: #f8d7da;
  color: #721c24;
}

.card-body {
  padding: 16px;
}

.info-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8px;
  font-size: 14px;
}

.info-row:last-child {
  margin-bottom: 0;
}

.label {
  color: #666;
}

.value {
  color: #333;
  font-weight: 500;
}

.status-active {
  color: #28a745;
}

.status-leave {
  color: #ffc107;
}

.status-inactive {
  color: #dc3545;
}

.card-footer {
  padding: 16px;
  background: #f8f9fa;
  border-top: 1px solid #ddd;
}

.actions {
  display: flex;
  gap: 8px;
  justify-content: flex-end;
}

.btn-account {
  background: #28a745;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 13px;
}

.btn-account:hover {
  background: #218838;
}

.btn-edit {
  background: #6c757d;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 13px;
}

.btn-edit:hover {
  background: #5a6268;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 12px;
  margin-top: 20px;
  padding: 20px;
  background: white;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.page-btn {
  padding: 8px 12px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.page-btn:hover:not(:disabled) {
  background: #f8f9fa;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-info {
  font-size: 14px;
  color: #666;
}

@media (max-width: 768px) {
  .employee-container {
    padding: 10px;
  }
  
  .header {
    flex-direction: column;
    gap: 12px;
  }
  
  .filters {
    flex-direction: column;
  }
  
  .search-input {
    min-width: auto;
  }
  
  .employees-grid {
    grid-template-columns: 1fr;
  }
}
</style>