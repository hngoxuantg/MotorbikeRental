<script setup>
import { ref, computed, onMounted } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'

const props = defineProps({
  incidents: {
    type: Object,
    default: () => ({ data: [], totalCount: 0 })
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['updateQuery', 'handleIncident'])

const filters = ref({
  PageNumber: 1,
  PageSize: 12
})

const pageSizeOptions = [8, 12, 16, 20]

const totalPages = computed(() => {
  if (!props.incidents.totalCount) return 1
  return Math.ceil(props.incidents.totalCount / filters.value.PageSize)
})

const incidentTypeText = computed(() => {
  const typeMap = {
    0: { text: 'Hư hỏng', color: '#dc3545', bgColor: '#f8d7da', icon: 'fa-wrench' },
    1: { text: 'Tai nạn', color: '#ffc107', bgColor: '#fff3cd', icon: 'fa-exclamation-triangle' },
    2: { text: 'Mất cắp', color: '#6f42c1', bgColor: '#e7d9f3', icon: 'fa-user-ninja' }
  }
  return (type) => typeMap[type] || { text: 'Không xác định', color: '#6c757d', bgColor: '#f8f9fa', icon: 'fa-question' }
})

const incidentStatus = computed(() => {
  return (incident) => {
    if (incident.resolvedDate) {
      return {
        text: 'Đã xử lý',
        color: '#28a745',
        bgColor: '#d4edda',
        icon: 'fa-check-circle'
      }
    } else {
      return {
        text: 'Chưa xử lý',
        color: '#dc3545',
        bgColor: '#f8d7da',
        icon: 'fa-clock'
      }
    }
  }
})

const formatDate = (dateString) => {
  if (!dateString || dateString === '0001-01-01T00:00:00') return 'Chưa có'
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN')
}

const formatCurrency = (value) => {
  if (!value) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(value)
}

const applyFilters = () => {
  emit('updateQuery', filters.value)
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

const handleIncident = (incidentId) => {
  emit('handleIncident', incidentId)
}

onMounted(() => {
  applyFilters()
})
</script>

<template>
  <div class="incident-list-page">
    <div class="page-header">
      <div class="header-content">
        <h1 class="page-title">
          <i class="fas fa-exclamation-circle"></i>
          Danh sách sự cố
        </h1>
        <div class="header-stats">
          <div class="stat-item">
            <span class="stat-number">{{ incidents.totalCount || 0 }}</span>
            <span class="stat-label">Tổng sự cố</span>
          </div>
          <div class="stat-item urgent">
            <span class="stat-number">
              {{ incidents.data ? incidents.data.filter(i => !i.resolvedDate).length : 0 }}
            </span>
            <span class="stat-label">Chưa xử lý</span>
          </div>
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

      <div v-else-if="incidents.data && incidents.data.length > 0" class="incidents-container">
        <div class="incidents-grid">
          <div
            v-for="incident in incidents.data"
            :key="incident.incidentId"
            class="incident-card"
            :class="{ 'unresolved': !incident.resolvedDate }"
          >
            <div class="card-header">
              <div class="incident-info">
                <h3 class="incident-id">Sự cố #{{ incident.incidentId }}</h3>
                <div class="incident-badges">
                  <div
                    class="type-badge"
                    :style="{
                      color: incidentTypeText(incident.type).color,
                      backgroundColor: incidentTypeText(incident.type).bgColor
                    }"
                  >
                    <i :class="['fas', incidentTypeText(incident.type).icon]"></i>
                    {{ incidentTypeText(incident.type).text }}
                  </div>
                  <div
                    class="status-badge"
                    :style="{
                      color: incidentStatus(incident).color,
                      backgroundColor: incidentStatus(incident).bgColor
                    }"
                  >
                    <i :class="['fas', incidentStatus(incident).icon]"></i>
                    {{ incidentStatus(incident).text }}
                  </div>
                </div>
              </div>
            </div>

            <div class="card-body">
              <div class="incident-images" v-if="incident.images && incident.images.length > 0">
                <div class="images-grid">
                  <img
                    v-for="(image, index) in incident.images.slice(0, 3)"
                    :key="index"
                    :src="getFullPath(image)"
                    :alt="`Ảnh sự cố ${index + 1}`"
                    class="incident-image"
                  />
                  <div
                    v-if="incident.images.length > 3"
                    class="more-images"
                  >
                    +{{ incident.images.length - 3 }}
                  </div>
                </div>
              </div>
              
              <div v-else class="no-images">
                <i class="fas fa-image"></i>
                <span>Không có hình ảnh</span>
              </div>

              <div class="incident-details">
                <div class="detail-item">
                  <span class="label">Chi phí thiệt hại:</span>
                  <span class="value cost">{{ formatCurrency(incident.damageCost) }}</span>
                </div>
                <div class="detail-item">
                  <span class="label">Ngày xử lý:</span>
                  <span class="value">{{ formatDate(incident.resolvedDate) }}</span>
                </div>
              </div>
            </div>

            <div class="card-footer">
              <button
                v-if="!incident.resolvedDate"
                @click="handleIncident(incident.incidentId)"
                class="handle-btn"
              >
                <i class="fas fa-tools"></i>
                Xử lý sự cố
              </button>
              <button
                v-else
                class="view-btn"
                disabled
              >
                <i class="fas fa-check"></i>
                Đã xử lý
              </button>
            </div>
          </div>
        </div>

        <div class="pagination-section">
          <div class="pagination-info">
            <span>{{ (filters.PageNumber - 1) * filters.PageSize + 1 }} - 
            {{ Math.min(filters.PageNumber * filters.PageSize, incidents.totalCount) }} 
            / {{ incidents.totalCount }} sự cố</span>
          </div>

          <div class="pagination-controls">
            <div class="page-size-control">
              <label>Hiển thị:</label>
              <select v-model="filters.PageSize" @change="changePageSize" class="page-size-select">
                <option v-for="size in pageSizeOptions" :key="size" :value="size">
                  {{ size }}
                </option>
              </select>
              <span>sự cố</span>
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
        <i class="fas fa-exclamation-circle"></i>
        <h3>Không có sự cố nào</h3>
        <p>Hiện tại không có sự cố nào được ghi nhận.</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.incident-list-page {
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
  color: #e74c3c;
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
  color: #e74c3c;
}

.stat-label {
  font-size: 12px;
  color: #666;
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
  color: #e74c3c;
}

.incidents-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 20px;
  margin-bottom: 25px;
}

.incident-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
}

.incident-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
}

.incident-card.unresolved {
  border-left: 4px solid #dc3545;
}

.card-header {
  background: #f8f9fa;
  padding: 15px 20px;
  border-bottom: 1px solid #e9ecef;
}

.incident-info {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.incident-id {
  margin: 0;
  font-size: 16px;
  font-weight: bold;
  color: #333;
}

.incident-badges {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.type-badge, .status-badge {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 11px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 4px;
}

.card-body {
  padding: 20px;
}

.incident-images {
  margin-bottom: 15px;
}

.images-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 8px;
  position: relative;
}

.incident-image {
  width: 100%;
  height: 80px;
  object-fit: cover;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.more-images {
  position: absolute;
  top: 0;
  right: 0;
  width: 100%;
  height: 80px;
  background: rgba(0, 0, 0, 0.7);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  border-radius: 4px;
}

.no-images {
  background: #f8f9fa;
  border: 2px dashed #ddd;
  padding: 30px;
  text-align: center;
  color: #666;
  border-radius: 4px;
  margin-bottom: 15px;
}

.no-images i {
  font-size: 24px;
  margin-bottom: 8px;
  display: block;
}

.incident-details {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.detail-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 13px;
}

.detail-item .label {
  color: #666;
  font-weight: 500;
}

.detail-item .value {
  color: #333;
  font-weight: 600;
}

.value.cost {
  color: #e74c3c;
}

.card-footer {
  padding: 15px 20px;
  background: #f8f9fa;
  border-top: 1px solid #e9ecef;
}

.handle-btn {
  width: 100%;
  padding: 10px 16px;
  background: #e74c3c;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
  font-size: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: all 0.2s;
}

.handle-btn:hover {
  background: #c0392b;
  transform: translateY(-1px);
}

.view-btn {
  width: 100%;
  padding: 10px 16px;
  background: #28a745;
  color: white;
  border: none;
  border-radius: 6px;
  font-weight: 600;
  font-size: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  opacity: 0.7;
  cursor: not-allowed;
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

.page-size-control {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13px;
  color: #666;
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
  border-color: #e74c3c;
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
  border-color: #e74c3c;
}

.page-number.active {
  background: #e74c3c;
  color: white;
  border-color: #e74c3c;
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
  .incidents-grid {
    grid-template-columns: 1fr;
    gap: 15px;
  }
  
  .pagination-section {
    flex-direction: column;
    align-items: stretch;
  }
  
  .pagination-controls {
    justify-content: center;
    gap: 10px;
  }
  
  .incident-badges {
    justify-content: center;
  }
}
</style>