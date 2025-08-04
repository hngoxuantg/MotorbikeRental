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

const emit = defineEmits(['updateQuery', 'completeMaintenance'])

const filters = ref({
  PageNumber: 1,
  PageSize: 12
})

const pageSizeOptions = [8, 12, 16, 20]

const totalPages = computed(() => {
  if (!props.motorbikes.totalCount) return 1
  return Math.ceil(props.motorbikes.totalCount / filters.value.PageSize)
})

const formatDate = (dateString) => {
  if (!dateString) return 'Chưa có'
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN')
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

const completeMaintenance = (motorbikeId) => {
  router.push({ name: 'MaintenanceCompleteRepairs', params: { motorbikeId } })
}

onMounted(() => {
  applyFilters()
})
</script>

<template>
  <div class="maintenance-repairs-page">
    <div class="page-header">
      <div class="header-content">
        <h1 class="page-title">
          <i class="fas fa-cog"></i>
          Xe đang bảo dưỡng
        </h1>
        <div class="header-stats">
          <div class="stat-item">
            <span class="stat-number">{{ motorbikes.totalCount || 0 }}</span>
            <span class="stat-label">Xe đang bảo dưỡng</span>
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

      <div v-else-if="motorbikes.data && motorbikes.data.length > 0" class="repairs-container">
        <div class="repairs-grid">
          <div
            v-for="motorbike in motorbikes.data"
            :key="motorbike.motorbikeId"
            class="repair-card"
          >
            <div class="card-header">
              <div class="bike-info">
                <h3 class="bike-name">{{ motorbike.motorbikeName }}</h3>
                <span class="license-plate">{{ motorbike.licensePlate }}</span>
              </div>
              <div class="bike-id">#{{ motorbike.motorbikeId }}</div>
            </div>

            <div class="card-body">
              <div class="bike-image">
                <img
                  :src="motorbike.imageUrl ? getFullPath(motorbike.imageUrl) : '/placeholder-bike.jpg'"
                  :alt="motorbike.motorbikeName"
                />
              </div>

              <div class="bike-details">
                <div class="detail-item">
                  <span class="label">Dung tích:</span>
                  <span class="value">{{ motorbike.engineCapacity }}cc</span>
                </div>
                <div class="detail-item">
                  <span class="label">Số lần bảo dưỡng:</span>
                  <span class="value count">{{ motorbike.maintenanceCount }} lần</span>
                </div>
                <div class="detail-item">
                  <span class="label">Bảo dưỡng cuối:</span>
                  <span class="value">{{ formatDate(motorbike.lastMaintenanceDate) }}</span>
                </div>
              </div>

              <div class="status-badge">
                <i class="fas fa-cog fa-spin"></i>
                Đang bảo dưỡng
              </div>
            </div>

            <div class="card-footer">
              <button
                @click="completeMaintenance(motorbike.motorbikeId)"
                class="complete-btn"
              >
                <i class="fas fa-check"></i>
                Hoàn tất bảo dưỡng
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
              <label>Hiển thị:</label>
              <select v-model="filters.PageSize" @change="changePageSize" class="page-size-select">
                <option v-for="size in pageSizeOptions" :key="size" :value="size">
                  {{ size }}
                </option>
              </select>
              <span>xe</span>
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
        <i class="fas fa-cog"></i>
        <h3>Không có xe nào đang bảo dưỡng</h3>
        <p>Hiện tại không có xe máy nào đang trong quá trình bảo dưỡng.</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.maintenance-repairs-page {
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
  color: #ff6b35;
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
  font-size: 20px;
  font-weight: bold;
  color: #ff6b35;
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
  color: #ff6b35;
}

.repairs-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
  margin-bottom: 25px;
}

.repair-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
  border-left: 4px solid #ff6b35;
}

.repair-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
}

.card-header {
  background: #fff3f0;
  padding: 15px 20px;
  border-bottom: 1px solid #f0f0f0;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.bike-info {
  flex: 1;
}

.bike-name {
  margin: 0 0 5px 0;
  font-size: 16px;
  font-weight: bold;
  color: #333;
}

.license-plate {
  color: #666;
  font-weight: 600;
  font-family: monospace;
  font-size: 13px;
}

.bike-id {
  color: #ff6b35;
  font-weight: bold;
  font-size: 14px;
  font-family: monospace;
}

.card-body {
  padding: 20px;
}

.bike-image {
  text-align: center;
  margin-bottom: 15px;
}

.bike-image img {
  width: 100px;
  height: 75px;
  object-fit: cover;
  border-radius: 6px;
  border: 1px solid #ddd;
}

.bike-details {
  margin-bottom: 15px;
}

.detail-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
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

.value.count {
  color: #ff6b35;
}

.status-badge {
  background: #fff3f0;
  color: #ff6b35;
  padding: 8px 12px;
  border-radius: 15px;
  font-size: 12px;
  font-weight: 600;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  border: 1px solid #ffccbc;
}

.card-footer {
  padding: 15px 20px;
  background: #f8f9fa;
  border-top: 1px solid #e9ecef;
}

.complete-btn {
  width: 100%;
  padding: 10px 16px;
  background: #28a745;
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

.complete-btn:hover {
  background: #218838;
  transform: translateY(-1px);
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
  border-color: #ff6b35;
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
  border-color: #ff6b35;
}

.page-number.active {
  background: #ff6b35;
  color: white;
  border-color: #ff6b35;
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
  .repairs-grid {
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
}
</style>