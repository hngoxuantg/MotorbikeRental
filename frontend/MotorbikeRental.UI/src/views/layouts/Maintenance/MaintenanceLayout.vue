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
    '/maintenance': 'Dashboard',
    '/maintenance/motorbikes': 'Quản lý xe cần bảo dưỡng',
    '/maintenance/schedule': 'Lịch bảo dưỡng',
    '/maintenance/repairs': 'Hoàn tất bảo dưỡng',
    '/maintenance/history': 'Lịch sử bảo dưỡng',
    '/maintenance/reports': 'Báo cáo bảo dưỡng',
    '/maintenance/incidents': 'Sửa chữa xe',
  }
  return names[route.path] || 'Bảo dưỡng'
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
  <div class="maintenance-layout">
    <!-- Header -->
    <header class="maintenance-header">
      <div class="header-left">
        <div class="logo">
          <span class="logo-text">MotorRental</span>
          <span class="logo-badge">BẢO DƯỠNG</span>
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

    <div class="maintenance-body">
      <!-- Sidebar -->
      <aside class="maintenance-sidebar">
        <nav class="sidebar-nav">
          <div class="nav-section">
            <div class="nav-section-title">TỔNG QUAN</div>

            <router-link to="/maintenance" class="nav-item" :class="{ active: isActive('/maintenance') }">
              <i class="fas fa-tachometer-alt nav-icon"></i>
              <span class="nav-text">Dashboard</span>
            </router-link>

            <router-link
              to="/maintenance/motorbikes"
              class="nav-item"
              :class="{ active: isActive('/maintenance/motorbikes') }"
            >
              <i class="fas fa-motorcycle nav-icon"></i>
              <span class="nav-text">Xe cần bảo dưỡng</span>
            </router-link>

            <router-link
              to="/maintenance/schedule"
              class="nav-item"
              :class="{ active: isActive('/maintenance/schedule') }"
            >
              <i class="fas fa-calendar-alt nav-icon"></i>
              <span class="nav-text">Lịch bảo dưỡng</span>
            </router-link>
          </div>

          <div class="nav-section">
            <div class="nav-section-title">SỬA CHỮA & BẢO DƯỠNG</div>

            <router-link
              to="/maintenance/repairs"
              class="nav-item"
              :class="{ active: isActive('/maintenance/repairs') }"
            >
              <i class="fas fa-tools nav-icon"></i>
              <span class="nav-text">Hoàn tất bảo dưỡng</span>
            </router-link>

            <router-link
              to="/maintenance/history"
              class="nav-item"
              :class="{ active: isActive('/maintenance/history') }"
            >
              <i class="fas fa-history nav-icon"></i>
              <span class="nav-text">Lịch sử bảo dưỡng</span>
            </router-link>

            <router-link
              to="/maintenance/incidents"
              class="nav-item"
              :class="{ active: isActive('/maintenance/incidents') }"
            >
              <i class="fas fa-boxes nav-icon"></i>
              <span class="nav-text">Sửa chữa xe</span>
            </router-link>
          </div>

          <div class="nav-section">
            <div class="nav-section-title">BÁO CÁO</div>

            <router-link
              to="/maintenance/reports"
              class="nav-item"
              :class="{ active: isActive('/maintenance/reports') }"
            >
              <i class="fas fa-chart-bar nav-icon"></i>
              <span class="nav-text">Báo cáo bảo dưỡng</span>
            </router-link>
          </div>
        </nav>
      </aside>

      <!-- Main Content -->
      <main class="maintenance-content">
        <!-- Breadcrumb -->
        <div class="breadcrumb-container">
          <div class="breadcrumb">
            <span class="breadcrumb-item">Bảo dưỡng</span>
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

.maintenance-layout {
  min-height: 100vh;
  background: #f5f5f5;
  font-family: Arial, sans-serif;
  width: 100%;
}

/* Header */
.maintenance-header {
  background: #3498db;
  color: white;
  padding: 0 15px;
  height: 55px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 1px solid #2980b9;
  position: sticky;
  top: 0;
  z-index: 1000;
  width: 100%;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
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
  background: #2980b9;
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
  width: 35px;
  height: 35px;
  border-radius: 50%;
  overflow: hidden;
  border: 2px solid #2980b9;
  background: #2980b9;
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
  color: #a8d5f2;
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
  transition: background 0.2s;
}

.logout-btn:hover {
  background: #c0392b;
}

/* Body */
.maintenance-body {
  display: flex;
  min-height: calc(100vh - 55px);
  width: 100%;
}

/* Sidebar */
.maintenance-sidebar {
  width: 280px;
  background: white;
  border-right: 1px solid #ddd;
  overflow-y: auto;
}

.sidebar-nav {
  padding: 20px 0;
}

.nav-section {
  margin-bottom: 25px;
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
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 20px;
  color: #333;
  text-decoration: none;
  font-size: 14px;
  border-left: 3px solid transparent;
  transition: all 0.2s;
}

.nav-item:hover {
  background: #ebf3fd;
  color: #3498db;
  border-left-color: #85c1e9;
}

.nav-item.active {
  background: #d6eaf8;
  color: #3498db;
  border-left-color: #3498db;
  font-weight: bold;
}

.nav-icon {
  width: 16px;
  font-size: 14px;
  color: #666;
}

.nav-item:hover .nav-icon,
.nav-item.active .nav-icon {
  color: #3498db;
}

.nav-text {
  flex: 1;
}

/* Content */
.maintenance-content {
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
  color: #3498db;
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
  .maintenance-sidebar {
    width: 250px;
  }
}

@media (max-width: 768px) {
  .maintenance-header {
    padding: 0 12px;
    height: 50px;
  }

  .user-info {
    display: none;
  }

  .maintenance-sidebar {
    width: 220px;
  }

  .content-area {
    padding: 0;
  }

  .nav-item {
    padding: 10px 16px;
    font-size: 13px;
  }

  .nav-text {
    font-size: 13px;
  }

  .maintenance-body {
    min-height: calc(100vh - 50px);
  }
}

@media (max-width: 576px) {
  .maintenance-sidebar {
    position: fixed;
    left: -280px;
    top: 50px;
    height: calc(100vh - 50px);
    z-index: 999;
    transition: left 0.3s;
  }

  .maintenance-sidebar.open {
    left: 0;
  }
}
</style>