<script setup>
import { computed, onMounted, ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { employeeService } from '../../../api/services/employeeService'
import defaultAvatar from '@/assets/image.png'
import { jwtDecode } from 'jwt-decode'
import { getFullPath } from '@/utils/UrlUtils'

const router = useRouter()
const route = useRoute()

const currentPageName = computed(() => {
  const names = {
    '/admin': 'Dashboard',
    '/admin/motorbikes': 'Quản lý xe máy',
    '/Admin/Index': 'Quản lý xe máy',
    '/admin/categories': 'Quản lý loại xe',
    '/admin/employees': 'Quản lý nhân viên',
    '/admin/customers': 'Quản lý khách hàng',
    '/admin/statistics': 'Báo cáo thống kê',
    '/admin/contracts': 'Lịch sử thuê xe',
    '/admin/discounts': 'Quản lý giảm giá',
  }
  return names[route.path] || 'Admin'
})

const token = localStorage.getItem('token')
let userId = null
if (token) {
  const decodedToken = jwtDecode(token)
  userId = decodedToken.sub
}

const employee = ref(null)

onMounted(async () => {
  if (userId) {
    try {
      const response = await employeeService.getEmployeeById(userId)
      employee.value = response.data
    } catch (error) {
      console.error('Error fetching employee:', error)
    }
  }
})

const logout = () => {
  if (confirm('Bạn có chắc muốn đăng xuất?')) {
    localStorage.removeItem('token')
    router.push({ name: 'Login' })
  }
}

const isActive = (path) => {
  return route.path === path
}
</script>

<template>
  <div class="admin-layout">
    <!-- Header -->
    <header class="admin-header">
      <div class="header-left">
        <div class="logo">
          <span class="logo-text">MotorRental</span>
          <span class="logo-badge">ADMIN</span>
        </div>
      </div>

      <div class="header-right">
        <div class="user-menu">
          <div class="user-avatar">
            <img
              v-if="employee && employee.avatar"
              :src="getFullPath(employee.avatar)"
              alt="Avatar"
              class="avatar-img"
            />
            <img v-else :src="defaultAvatar" alt="Default Avatar" class="avatar-img" />
          </div>
          <div class="user-info" v-if="employee">
            <span class="user-name">{{ employee.fullName }}</span>
            <span class="user-role">{{ employee.roleName }}</span>
          </div>
          <button @click="logout" class="logout-btn">
            Đăng xuất
          </button>
        </div>
      </div>
    </header>

    <div class="admin-body">
      <!-- Sidebar -->
      <aside class="admin-sidebar">
        <nav class="sidebar-nav">
          <div class="nav-section">
            <div class="nav-section-title">MENU CHÍNH</div>

            <router-link to="/admin" class="nav-item" :class="{ active: isActive('/admin') }">
              <span class="nav-text">Dashboard</span>
            </router-link>

            <router-link
              to="/Admin/Index"
              class="nav-item"
              :class="{ active: isActive('/Admin/Index') }"
            >
              <span class="nav-text">Quản lý xe máy</span>
            </router-link>

            <router-link
              to="/admin/categories"
              class="nav-item"
              :class="{ active: isActive('/admin/categories') }"
            >
              <span class="nav-text">Quản lý loại xe</span>
            </router-link>

            <router-link
              to="/admin/discounts"
              class="nav-item"
              :class="{ active: isActive('/admin/discounts') }"
            >
              <span class="nav-text">Quản lý giảm giá</span>
            </router-link>

            <router-link
              to="/admin/employees"
              class="nav-item"
              :class="{ active: isActive('/admin/employees') }"
            >
              <span class="nav-text">Quản lý nhân viên</span>
            </router-link>

            <router-link
              to="/admin/customers"
              class="nav-item"
              :class="{ active: isActive('/admin/customers') }"
            >
              <span class="nav-text">Quản lý khách hàng</span>
            </router-link>
          </div>

          <div class="nav-section">
            <div class="nav-section-title">BÁO CÁO</div>

            <router-link
              to="/admin/statistics"
              class="nav-item"
              :class="{ active: isActive('/admin/statistics') }"
            >
              <span class="nav-text">Báo cáo thống kê</span>
            </router-link>

            <router-link
              to="/admin/contracts"
              class="nav-item"
              :class="{ active: isActive('/admin/contracts') }"
            >
              <span class="nav-text">Lịch sử thuê xe</span>
            </router-link>
          </div>
        </nav>
      </aside>

      <!-- Main Content -->
      <main class="admin-content">
        <!-- Breadcrumb -->
        <div class="breadcrumb-container">
          <div class="breadcrumb">
            <span class="breadcrumb-item">Admin</span>
            <span class="breadcrumb-separator">></span>
            <span class="breadcrumb-item current">{{ currentPageName }}</span>
          </div>
        </div>

        <!-- Content -->
        <div class="content-area">
          <slot />
        </div>
      </main>
    </div>
  </div>
</template>

<style scoped>
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

.admin-layout {
  min-height: 100vh;
  background: #f5f5f5;
  font-family: Arial, sans-serif;
  width: 100%;
}

/* Header */
.admin-header {
  background: #2c3e50;
  color: white;
  padding: 0 20px;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 1px solid #34495e;
  position: sticky;
  top: 0;
  z-index: 1000;
  width: 100%;
}

.logo {
  display: flex;
  align-items: center;
  gap: 12px;
}

.logo-text {
  font-size: 20px;
  font-weight: bold;
  color: white;
}

.logo-badge {
  background: #34495e;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: bold;
}

.user-menu {
  display: flex;
  align-items: center;
  gap: 12px;
}

.user-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  overflow: hidden;
  border: 2px solid #34495e;
  background: #34495e;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.user-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
  min-width: 120px;
}

.user-name {
  font-weight: bold;
  font-size: 14px;
  color: white;
}

.user-role {
  font-size: 12px;
  color: #bdc3c7;
}

.logout-btn {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  font-weight: bold;
}

.logout-btn:hover {
  background: #c0392b;
}

/* Body */
.admin-body {
  display: flex;
  min-height: calc(100vh - 60px);
  width: 100%;
}

/* Sidebar */
.admin-sidebar {
  width: 250px;
  background: white;
  border-right: 1px solid #ddd;
  overflow-y: auto;
}

.sidebar-nav {
  padding: 20px 0;
}

.nav-section {
  margin-bottom: 20px;
}

.nav-section-title {
  padding: 8px 20px;
  font-size: 12px;
  font-weight: bold;
  text-transform: uppercase;
  color: #666;
  border-bottom: 1px solid #f0f0f0;
  margin-bottom: 8px;
  background: #f8f9fa;
}

.nav-item {
  display: block;
  padding: 12px 20px;
  color: #333;
  text-decoration: none;
  font-size: 14px;
  border-left: 3px solid transparent;
  transition: all 0.2s;
}

.nav-item:hover {
  background: #f8f9fa;
  color: #2c3e50;
  border-left-color: #bdc3c7;
}

.nav-item.active {
  background: #e8f4f8;
  color: #2c3e50;
  border-left-color: #3498db;
  font-weight: bold;
}

.nav-text {
  display: block;
}

/* Content */
.admin-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  background: #f5f5f5;
}

.breadcrumb-container {
  background: white;
  border-bottom: 1px solid #ddd;
  padding: 12px 20px;
}

.breadcrumb {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
}

.breadcrumb-item {
  color: #666;
}

.breadcrumb-item.current {
  color: #333;
  font-weight: bold;
}

.breadcrumb-separator {
  color: #bdc3c7;
}

.content-area {
  flex: 1;
  padding: 0;
  background: #f5f5f5;
}

/* Responsive */
@media (max-width: 1024px) {
  .admin-sidebar {
    width: 220px;
  }
}

@media (max-width: 768px) {
  .admin-header {
    padding: 0 16px;
  }

  .user-info {
    display: none;
  }

  .admin-sidebar {
    width: 200px;
  }

  .content-area {
    padding: 0;
  }
}
</style>