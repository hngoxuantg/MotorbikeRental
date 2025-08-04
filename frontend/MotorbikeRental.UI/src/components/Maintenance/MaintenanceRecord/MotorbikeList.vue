<script setup>
import { ref, computed, onMounted } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'
import { useRouter } from 'vue-router'

const router = useRouter()
const props = defineProps({
  motorbikes: {
    type: Object,
    default: () => ({ data: [], totalCount: 0 })
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['updateQuery'])

const filters = ref({
  PageNumber: 1,
  PageSize: 12,
  Search: ''
})

const pageSizeOptions = [8, 12, 20, 30]

const totalPages = computed(() => {
  if (!props.motorbikes.totalCount) return 1
  return Math.ceil(props.motorbikes.totalCount / filters.value.PageSize)
})

const maintenanceStatus = computed(() => {
  return (motorbike) => {
    const isUnderMaintenance = motorbike.maintenanceCount > 0 && !motorbike.nextMaintenanceDate
    
    if (isUnderMaintenance) {
      return {
        text: 'Đang bảo dưỡng',
        color: '#6c757d',
        bgColor: '#f8f9fa',
        icon: 'fa-cog',
        priority: 'maintenance'
      }
    }
    
    if (motorbike.maintenanceCount === 0) {
      return {
        text: 'Chưa bảo dưỡng',
        color: '#dc3545',
        bgColor: '#f8d7da',
        icon: 'fa-exclamation-triangle',
        priority: 'high'
      }
    } else if (motorbike.daysUntilNextMaintenance <= 7) {
      return {
        text: 'Cần bảo dưỡng gấp',
        color: '#fd7e14',
        bgColor: '#fff3cd',
        icon: 'fa-clock',
        priority: 'urgent'
      }
    } else if (motorbike.daysUntilNextMaintenance <= 30) {
      return {
        text: 'Sắp đến hạn',
        color: '#ffc107',
        bgColor: '#fff3cd',
        icon: 'fa-calendar-alt',
        priority: 'medium'
      }
    } else {
      return {
        text: 'Bình thường',
        color: '#28a745',
        bgColor: '#d4edda',
        icon: 'fa-check-circle',
        priority: 'low'
      }
    }
  }
})

const motorbikeStatusText = computed(() => {
  const statusMap = {
    0: { text: 'Có sẵn', color: '#28a745', bgColor: '#d4edda' },      
    1: { text: 'Đang thuê', color: '#ffc107', bgColor: '#fff3cd' },    
    2: { text: 'Bảo dưỡng', color: '#6c757d', bgColor: '#f8f9fa' },   
    3: { text: 'Đã đặt trước', color: '#17a2b8', bgColor: '#d1ecf1' },
    4: { text: 'Ngừng hoạt động', color: '#6c757d', bgColor: '#e2e6ea' }, 
    5: { text: 'Hư hỏng', color: '#dc3545', bgColor: '#f8d7da' }       
  }
  return (status) => statusMap[status] || { text: 'Không xác định', color: '#6c757d', bgColor: '#f8f9fa' }
})

const getActionButtons = computed(() => {
  return (motorbike) => {
    const buttons = []
    
    const isUnderMaintenance = motorbike.maintenanceCount > 0 && !motorbike.nextMaintenanceDate
    
    if (isUnderMaintenance) {
      buttons.push({
        type: 'history',
        text: 'Lịch sử',
        icon: 'fa-history',
        class: 'secondary',
        action: () => viewMaintenanceHistory(motorbike.motorbikeId)
      })
      return buttons
    }
    
    switch (motorbike.motorbikeStatus) {
      case 0: 
        buttons.push({
          type: 'maintenance',
          text: 'Bảo dưỡng',
          icon: 'fa-tools',
          class: 'primary',
          action: () => createMaintenanceRecord(motorbike.motorbikeId)
        })
        break
      
      case 1: 
        break
      
      case 2: 
        buttons.push({
          type: 'complete',
          text: 'Hoàn thành',
          icon: 'fa-check',
          class: 'success',
          action: () => completeMaintenance(motorbike.motorbikeId)
        })
        break
      
      case 3: 
      case 4: 
        break
      
      case 5: 
        buttons.push({
          type: 'repair',
          text: 'Sửa chữa',
          icon: 'fa-wrench',
          class: 'warning',
          action: () => repairMotorbike(motorbike.motorbikeId)
        })
        break
    }
    
    buttons.push({
      type: 'history',
      text: 'Lịch sử',
      icon: 'fa-history',
      class: 'secondary',
      action: () => viewMaintenanceHistory(motorbike.motorbikeId)
    })
    
    return buttons
  }
})

const formatDate = (dateString) => {
  if (!dateString) return 'Chưa có'
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' })
}

const buildQueryParams = () => {
  const params = { ...filters.value }
  
  if (!params.Search || params.Search.trim() === '') {
    delete params.Search
  }
  
  return params
}

const applyFilters = () => {
  const queryParams = buildQueryParams()
  emit('updateQuery', queryParams)
}

const resetFilters = () => {
  filters.value = {
    PageNumber: 1,
    PageSize: 12,
    Search: ''
  }
  applyFilters()
}

const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    filters.value.PageNumber = page
    applyFilters()
  }
}

const changePageSize = () => {
  filters.value.PageNumber = 1
  applyFilters()
}

const handleSearchInput = () => {
  clearTimeout(window.searchTimeout)
  window.searchTimeout = setTimeout(() => {
    filters.value.PageNumber = 1
    applyFilters()
  }, 500)
}

const createMaintenanceRecord = (motorbikeId) => {
    router.push( 'createMaintenanceRecord/' + motorbikeId)
}

const completeMaintenance = (motorbikeId) => {
  console.log('Complete maintenance for motorbike:', motorbikeId)
}

const repairMotorbike = (motorbikeId) => {
  console.log('Repair motorbike:', motorbikeId)
}

const viewMaintenanceHistory = (motorbikeId) => {
  console.log('View maintenance history for motorbike:', motorbikeId)
}

onMounted(() => {
  applyFilters()
})
</script>

<template>
  <div class="motorbike-list-page">
    <div class="page-header">
      <div class="header-content">
        <h1 class="page-title">
          <i class="fas fa-motorcycle"></i>
          Quản lý xe cần bảo dưỡng
        </h1>
        <div class="header-stats">
          <div class="stat-item">
            <span class="stat-number">{{ motorbikes.totalCount || 0 }}</span>
            <span class="stat-label">Tổng số xe</span>
          </div>
          <div class="stat-item urgent">
            <span class="stat-number">
              {{ motorbikes.data ? motorbikes.data.filter(m => m.maintenanceCount === 0).length : 0 }}
            </span>
            <span class="stat-label">Chưa bảo dưỡng</span>
          </div>
        </div>
      </div>
    </div>

    <div class="filters-section">
      <div class="filters-container">
        <div class="filter-group">
          <label class="filter-label">Tìm kiếm:</label>
          <input
            v-model="filters.Search"
            @input="handleSearchInput"
            type="text"
            class="filter-input"
            placeholder="Tìm theo tên xe hoặc biển số..."
          />
        </div>

        <div class="filter-actions">
          <button @click="resetFilters" class="btn btn-secondary">
            <i class="fas fa-undo"></i>
            Đặt lại
          </button>
        </div>
      </div>
    </div>

    <div class="content-section">
      <div v-if="isLoading" class="loading-container">
        <div class="loading-spinner">
          <i class="fas fa-spinner fa-spin"></i>
          <span>Đang tải dữ liệu...</span>
        </div>
      </div>

      <div v-else-if="motorbikes.data && motorbikes.data.length > 0" class="motorbikes-container">
        <div class="motorbikes-grid">
          <div
            v-for="motorbike in motorbikes.data"
            :key="motorbike.motorbikeId"
            class="motorbike-card"
            :class="{ 'urgent-maintenance': motorbike.maintenanceCount === 0 }"
          >
            <div class="card-header">
              <div class="bike-title">
                <h4 class="motorbike-name">{{ motorbike.motorbikeName }}</h4>
                <span class="license-plate">{{ motorbike.licensePlate }}</span>
              </div>
              <div class="motorbike-id">#{{ motorbike.motorbikeId }}</div>
            </div>

            <div class="card-body">
              <div class="bike-image">
                <img
                  :src="motorbike.imageUrl ? getFullPath(motorbike.imageUrl) : '/placeholder-bike.jpg'"
                  :alt="motorbike.motorbikeName"
                />
              </div>

              <div class="bike-status">
                <div
                  class="status-badge"
                  :style="{
                    color: motorbikeStatusText(motorbike.motorbikeStatus).color,
                    backgroundColor: motorbikeStatusText(motorbike.motorbikeStatus).bgColor
                  }"
                >
                  {{ motorbikeStatusText(motorbike.motorbikeStatus).text }}
                </div>
                <div
                  class="maintenance-badge"
                  :style="{
                    color: maintenanceStatus(motorbike).color,
                    backgroundColor: maintenanceStatus(motorbike).bgColor
                  }"
                >
                  <i :class="['fas', maintenanceStatus(motorbike).icon]"></i>
                  {{ maintenanceStatus(motorbike).text }}
                </div>
              </div>

              <div class="bike-info">
                <div class="info-item">
                  <span class="label">Dung tích:</span>
                  <span class="value">{{ motorbike.engineCapacity }}cc</span>
                </div>
                <div class="info-item">
                  <span class="label">Bảo dưỡng:</span>
                  <span class="value count">{{ motorbike.maintenanceCount }} lần</span>
                </div>
                <div class="info-item">
                  <span class="label">Lần cuối:</span>
                  <span class="value">{{ formatDate(motorbike.lastMaintenanceDate) }}</span>
                </div>
                <div class="info-item">
                  <span class="label">Tiếp theo:</span>
                  <span class="value">{{ formatDate(motorbike.nextMaintenanceDate) }}</span>
                </div>
                <div v-if="motorbike.daysUntilNextMaintenance > 0" class="info-item">
                  <span class="label">Còn lại:</span>
                  <span class="value days">{{ motorbike.daysUntilNextMaintenance }} ngày</span>
                </div>
              </div>
            </div>

            <div class="card-footer">
              <button
                v-for="button in getActionButtons(motorbike)"
                :key="button.type"
                @click="button.action"
                :class="['action-btn', button.class]"
              >
                <i :class="['fas', button.icon]"></i>
                {{ button.text }}
              </button>
            </div>
          </div>
        </div>

        <div class="pagination-section">
          <div class="pagination-info">
            <span>{{ (filters.PageNumber - 1) * filters.PageSize + 1 }} - 
            {{ Math.min(filters.PageNumber * filters.PageSize, motorbikes.totalCount) }} 
            / {{ motorbikes.totalCount }} xe</span>
          </div>

          <div class="pagination-controls">
            <div class="page-size-control">
              <select v-model="filters.PageSize" @change="changePageSize" class="page-size-select">
                <option v-for="size in pageSizeOptions" :key="size" :value="size">
                  {{ size }}
                </option>
              </select>
            </div>

            <div class="page-navigation">
              <button
                @click="changePage(filters.PageNumber - 1)"
                :disabled="filters.PageNumber <= 1"
                class="page-btn"
              >
                <i class="fas fa-chevron-left"></i>
              </button>

              <div class="page-numbers">
                <button
                  v-for="page in Math.min(5, totalPages)"
                  :key="page"
                  @click="changePage(page)"
                  :class="['page-number', { active: page === filters.PageNumber }]"
                >
                  {{ page }}
                </button>
              </div>

              <button
                @click="changePage(filters.PageNumber + 1)"
                :disabled="filters.PageNumber >= totalPages"
                class="page-btn"
              >
                <i class="fas fa-chevron-right"></i>
              </button>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="no-data">
        <i class="fas fa-motorcycle"></i>
        <h3>Không có xe máy nào</h3>
        <p>Chưa có xe máy nào cần bảo dưỡng được tìm thấy.</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.motorbike-list-page {
  background: #f5f5f5;
  min-height: 100vh;
  font-family: Arial, sans-serif;
}

.page-header {
  background: white;
  border-bottom: 1px solid #ddd;
  padding: 20px;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.page-title {
  margin: 0;
  color: #333;
  font-size: 24px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 12px;
}

.page-title i {
  color: #3498db;
}

.header-stats {
  display: flex;
  gap: 20px;
}

.stat-item {
  text-align: center;
}

.stat-item.urgent .stat-number {
  color: #dc3545;
}

.stat-number {
  display: block;
  font-size: 20px;
  font-weight: bold;
  color: #3498db;
}

.stat-label {
  font-size: 12px;
  color: #666;
}

.filters-section {
  background: white;
  border-bottom: 1px solid #ddd;
  padding: 15px 20px;
}

.filters-container {
  display: flex;
  justify-content: space-between;
  align-items: end;
  gap: 20px;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
  flex: 1;
  max-width: 300px;
}

.filter-label {
  font-weight: 600;
  color: #333;
  font-size: 13px;
}

.filter-input {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.filter-input:focus {
  outline: none;
  border-color: #3498db;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

.btn {
  padding: 8px 14px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 6px;
  transition: all 0.2s;
  font-size: 13px;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #5a6268;
}

.content-section {
  padding: 20px;
}

.loading-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 300px;
}

.loading-spinner {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 15px;
  color: #666;
}

.loading-spinner i {
  font-size: 40px;
  color: #3498db;
}

.motorbikes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 16px;
  margin-bottom: 25px;
}

.motorbike-card {
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
  transition: transform 0.2s, box-shadow 0.2s;
}

.motorbike-card:hover {
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.12);
}

.motorbike-card.urgent-maintenance {
  border-left: 3px solid #dc3545;
}

.card-header {
  background: #f8f9fa;
  padding: 12px 15px;
  border-bottom: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.bike-title {
  flex: 1;
}

.motorbike-name {
  margin: 0 0 3px 0;
  font-size: 15px;
  font-weight: bold;
  color: #333;
  line-height: 1.2;
}

.license-plate {
  color: #666;
  font-weight: 600;
  font-family: monospace;
  font-size: 12px;
}

.motorbike-id {
  color: #3498db;
  font-weight: bold;
  font-size: 13px;
  font-family: monospace;
}

.card-body {
  padding: 15px;
}

.bike-image {
  text-align: center;
  margin-bottom: 12px;
}

.bike-image img {
  width: 80px;
  height: 60px;
  object-fit: cover;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.bike-status {
  display: flex;
  gap: 6px;
  margin-bottom: 12px;
  flex-wrap: wrap;
}

.status-badge, .maintenance-badge {
  padding: 3px 8px;
  border-radius: 10px;
  font-size: 11px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 4px;
  line-height: 1;
}

.bike-info {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.info-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 12px;
}

.info-item .label {
  color: #666;
  font-weight: 500;
}

.info-item .value {
  color: #333;
  font-weight: 600;
}

.value.count {
  color: #3498db;
}

.value.days {
  color: #ffc107;
  font-weight: bold;
}

.card-footer {
  padding: 12px 15px;
  background: #f8f9fa;
  border-top: 1px solid #e9ecef;
  display: flex;
  gap: 8px;
}

.action-btn {
  flex: 1;
  padding: 8px 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 5px;
  transition: all 0.2s;
  font-size: 12px;
}

.action-btn.primary {
  background: #3498db;
  color: white;
}

.action-btn.primary:hover {
  background: #2980b9;
}

.action-btn.success {
  background: #28a745;
  color: white;
}

.action-btn.success:hover {
  background: #218838;
}

.action-btn.warning {
  background: #ffc107;
  color: #212529;
}

.action-btn.warning:hover {
  background: #e0a800;
}

.action-btn.secondary {
  background: #6c757d;
  color: white;
}

.action-btn.secondary:hover {
  background: #5a6268;
}

.pagination-section {
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 15px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 15px;
}

.pagination-info {
  color: #666;
  font-size: 13px;
}

.pagination-controls {
  display: flex;
  align-items: center;
  gap: 15px;
}

.page-size-select {
  padding: 5px 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 13px;
}

.page-navigation {
  display: flex;
  align-items: center;
  gap: 8px;
}

.page-btn {
  width: 32px;
  height: 32px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
}

.page-btn:hover:not(:disabled) {
  background: #f8f9fa;
  border-color: #3498db;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-numbers {
  display: flex;
  gap: 4px;
}

.page-number {
  width: 32px;
  height: 32px;
  border: 1px solid #ddd;
  background: white;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 13px;
  transition: all 0.2s;
}

.page-number:hover {
  background: #f8f9fa;
  border-color: #3498db;
}

.page-number.active {
  background: #3498db;
  color: white;
  border-color: #3498db;
}

.no-data {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 300px;
  color: #666;
  gap: 15px;
  text-align: center;
}

.no-data i {
  font-size: 60px;
  color: #ccc;
}

.no-data h3 {
  margin: 0;
  color: #999;
}

.no-data p {
  margin: 0;
  color: #bbb;
}

@media (max-width: 768px) {
  .filters-container {
    flex-direction: column;
    align-items: stretch;
  }
  
  .motorbikes-grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    gap: 12px;
  }
  
  .pagination-section {
    flex-direction: column;
    align-items: stretch;
  }
  
  .pagination-controls {
    justify-content: center;
    gap: 10px;
  }
}

@media (max-width: 480px) {
  .motorbikes-grid {
    grid-template-columns: 1fr;
  }
  
  .bike-status {
    justify-content: center;
  }
  
  .card-footer {
    flex-direction: column;
    gap: 6px;
  }
}
</style>