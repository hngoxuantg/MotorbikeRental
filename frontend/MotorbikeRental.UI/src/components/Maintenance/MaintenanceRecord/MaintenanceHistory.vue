<script setup>
import { ref, computed, onMounted } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'

const props = defineProps({
  history: {
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
  PageSize: 10,
  IsCompleted: null,
  ToDate: '',
  EndDate: '',
  Search: ''
})

const statusOptions = [
  { value: null, label: 'Tất cả trạng thái' },
  { value: true, label: 'Đã hoàn thành' },
  { value: false, label: 'Chưa hoàn thành' }
]

const pageSizeOptions = [5, 10, 20, 50]

const totalPages = computed(() => {
  if (!props.history.totalCount) return 1
  return Math.ceil(props.history.totalCount / filters.value.PageSize)
})

const statusInfo = computed(() => {
  return (isCompleted) => {
    return isCompleted ? {
      text: 'Đã hoàn thành',
      color: '#28a745',
      bgColor: '#d4edda',
      icon: 'fa-check-circle'
    } : {
      text: 'Chưa hoàn thành',
      color: '#ffc107',
      bgColor: '#fff3cd',
      icon: 'fa-clock'
    }
  }
})

const formatPrice = (price) => {
  if (!price) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN').format(price) + ' VNĐ'
}

const formatDate = (dateString) => {
  if (!dateString || dateString === '0001-01-01T00:00:00') return 'Chưa xác định'
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN')
}

const formatDateTime = (dateString) => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleString('vi-VN')
}

const buildQueryParams = () => {
  const params = { ...filters.value }
  
  if (params.IsCompleted === null || params.IsCompleted === '') {
    delete params.IsCompleted
  }
  
  if (!params.ToDate) {
    delete params.ToDate
  } else {
    params.ToDate = new Date(params.ToDate).toISOString()
  }
  
  if (!params.EndDate) {
    delete params.EndDate
  } else {
    params.EndDate = new Date(params.EndDate).toISOString()
  }
  
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
    PageSize: 10,
    IsCompleted: null,
    ToDate: '',
    EndDate: '',
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

const handleSearchInput = (event) => {
  filters.value.Search = event.target.value
  clearTimeout(window.searchTimeout)
  window.searchTimeout = setTimeout(() => {
    filters.value.PageNumber = 1
    applyFilters()
  }, 500)
}

const handleStatusChange = () => {
  filters.value.PageNumber = 1
  applyFilters()
}

const handleDateChange = () => {
  filters.value.PageNumber = 1
  applyFilters()
}

onMounted(() => {
  applyFilters()
})
</script>

<template>
  <div class="maintenance-history-page">
    <div class="page-header">
      <div class="header-content">
        <h1 class="page-title">
          <i class="fas fa-history"></i>
          Lịch sử bảo dưỡng
        </h1>
        <div class="header-stats">
          <div class="stat-item">
            <span class="stat-number">{{ history.totalCount || 0 }}</span>
            <span class="stat-label">Tổng số bản ghi</span>
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
            placeholder="Tìm theo tên xe, mô tả..."
          />
        </div>

        <div class="filter-group">
          <label class="filter-label">Trạng thái:</label>
          <select v-model="filters.IsCompleted" @change="handleStatusChange" class="filter-select">
            <option
              v-for="option in statusOptions"
              :key="option.value"
              :value="option.value"
            >
              {{ option.label }}
            </option>
          </select>
        </div>

        <div class="filter-group">
          <label class="filter-label">Từ ngày:</label>
          <input
            v-model="filters.ToDate"
            @change="handleDateChange"
            type="date"
            class="filter-input"
          />
        </div>

        <div class="filter-group">
          <label class="filter-label">Đến ngày:</label>
          <input
            v-model="filters.EndDate"
            @change="handleDateChange"
            type="date"
            class="filter-input"
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

      <div v-else-if="history.data && history.data.length > 0" class="records-container">
        <div class="records-grid">
          <div
            v-for="record in history.data"
            :key="record.maintenanceRecordId"
            class="record-card"
          >
            <div class="card-header">
              <div class="record-id">#{{ record.maintenanceRecordId }}</div>
              <div
                class="status-badge"
                :style="{
                  color: statusInfo(record.isCompleted).color,
                  backgroundColor: statusInfo(record.isCompleted).bgColor
                }"
              >
                <i :class="['fas', statusInfo(record.isCompleted).icon]"></i>
                {{ statusInfo(record.isCompleted).text }}
              </div>
            </div>

            <div class="card-body">
              <div class="motorbike-info">
                <div class="motorbike-image">
                  <img
                    :src="record.motorbikeImage ? getFullPath(record.motorbikeImage) : '/placeholder-bike.jpg'"
                    :alt="record.motorbikeName"
                  />
                </div>
                <div class="motorbike-details">
                  <h3 class="motorbike-name">{{ record.motorbikeName }}</h3>
                  <p class="motorbike-id">Mã xe: #{{ record.motorbikeId }}</p>
                </div>
              </div>

              <div class="maintenance-info">
                <div class="info-row">
                  <span class="label">Ngày bảo dưỡng:</span>
                  <span class="value">{{ formatDate(record.maintenanceDate) }}</span>
                </div>
                <div class="info-row">
                  <span class="label">Mô tả:</span>
                  <span class="value description">{{ record.description || 'Không có mô tả' }}</span>
                </div>
                <div class="info-row">
                  <span class="label">Chi phí:</span>
                  <span class="value cost">{{ formatPrice(record.cost) }}</span>
                </div>
                <div class="info-row">
                  <span class="label">Nhân viên:</span>
                  <span class="value">{{ record.employeeName }}</span>
                </div>
                <div class="info-row">
                  <span class="label">Ngày tạo:</span>
                  <span class="value">{{ formatDateTime(record.createdAt) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="pagination-section">
          <div class="pagination-info">
            <span>Hiển thị {{ (filters.PageNumber - 1) * filters.PageSize + 1 }} - 
            {{ Math.min(filters.PageNumber * filters.PageSize, history.totalCount) }} 
            trong tổng số {{ history.totalCount }} bản ghi</span>
          </div>

          <div class="pagination-controls">
            <div class="page-size-control">
              <label>Hiển thị:</label>
              <select v-model="filters.PageSize" @change="changePageSize" class="page-size-select">
                <option v-for="size in pageSizeOptions" :key="size" :value="size">
                  {{ size }}
                </option>
              </select>
              <span>bản ghi</span>
            </div>

            <div class="page-navigation">
              <button
                @click="changePage(filters.PageNumber - 1)"
                :disabled="filters.PageNumber <= 1"
                class="page-btn"
              >
                <i class="fas fa-chevron-left"></i>
                Trước
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
                Sau
                <i class="fas fa-chevron-right"></i>
              </button>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="no-data">
        <i class="fas fa-history"></i>
        <h3>Không có dữ liệu bảo dưỡng</h3>
        <p>Chưa có bản ghi bảo dưỡng nào được tìm thấy.</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.maintenance-history-page {
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

.stat-number {
  display: block;
  font-size: 24px;
  font-weight: bold;
  color: #3498db;
}

.stat-label {
  font-size: 14px;
  color: #666;
}

.filters-section {
  background: white;
  border-bottom: 1px solid #ddd;
  padding: 20px;
}

.filters-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
  align-items: end;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.filter-label {
  font-weight: 600;
  color: #333;
  font-size: 14px;
}

.filter-input, .filter-select {
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.filter-input:focus, .filter-select:focus {
  outline: none;
  border-color: #3498db;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

.filter-actions {
  display: flex;
  gap: 10px;
}

.btn {
  padding: 10px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 6px;
  transition: all 0.2s;
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

.records-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

.record-card {
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
}

.record-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.card-header {
  background: #f8f9fa;
  padding: 15px 20px;
  border-bottom: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.record-id {
  font-weight: bold;
  color: #3498db;
  font-size: 16px;
}

.status-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 6px;
}

.card-body {
  padding: 20px;
}

.motorbike-info {
  display: flex;
  gap: 15px;
  margin-bottom: 20px;
  padding-bottom: 15px;
  border-bottom: 1px solid #f0f0f0;
}

.motorbike-image {
  width: 80px;
  height: 60px;
  border-radius: 6px;
  overflow: hidden;
}

.motorbike-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.motorbike-details {
  flex: 1;
}

.motorbike-name {
  margin: 0 0 5px 0;
  font-size: 16px;
  font-weight: bold;
  color: #333;
}

.motorbike-id {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.maintenance-info {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.info-row {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 10px;
}

.info-row .label {
  color: #666;
  font-weight: 500;
  font-size: 14px;
  min-width: 120px;
}

.info-row .value {
  color: #333;
  font-weight: 600;
  font-size: 14px;
  text-align: right;
  flex: 1;
}

.value.description {
  font-style: italic;
  color: #666;
}

.value.cost {
  color: #e74c3c;
  font-family: monospace;
}

.pagination-section {
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 15px;
}

.pagination-info {
  color: #666;
  font-size: 14px;
}

.pagination-controls {
  display: flex;
  align-items: center;
  gap: 20px;
}

.page-size-control {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
  color: #666;
}

.page-size-select {
  padding: 6px 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.page-navigation {
  display: flex;
  align-items: center;
  gap: 10px;
}

.page-btn {
  padding: 8px 12px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 6px;
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
  gap: 5px;
}

.page-number {
  width: 36px;
  height: 36px;
  border: 1px solid #ddd;
  background: white;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 14px;
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
    grid-template-columns: 1fr;
  }
  
  .records-grid {
    grid-template-columns: 1fr;
  }
  
  .pagination-section {
    flex-direction: column;
    align-items: stretch;
  }
  
  .pagination-controls {
    flex-direction: column;
    gap: 15px;
  }
  
  .motorbike-info {
    flex-direction: column;
    gap: 10px;
  }
  
  .info-row {
    flex-direction: column;
    align-items: flex-start;
    gap: 5px;
  }
  
  .info-row .value {
    text-align: left;
  }
}
</style>