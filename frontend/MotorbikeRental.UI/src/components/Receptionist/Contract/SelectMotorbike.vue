<script setup>
import { ref, watch, computed, nextTick } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'
import { useRouter } from 'vue-router'
import { contractStorage } from '@/utils/ContractStorageUtils.js'

const props = defineProps({
  motorbikes: { type: Array, required: true },
  categories: { type: Array, required: true },
  brands: { type: Array, required: true },
  motorbikeStatuses: { type: Array, required: true },
  totalMotorbikes: { type: Number, required: true },
  query: { type: Object, required: true },
})

const emit = defineEmits(['updateQuery'])
const router = useRouter()

const searchQuery = ref('')
const selectedCategory = ref('')
const selectedBrand = ref('')
const selectedStatus = ref('')

let isUpdating = false

watch(() => props.query, (newQuery) => {
  isUpdating = true
  searchQuery.value = newQuery.Search || ''
  selectedCategory.value = newQuery.CategoryId || ''
  selectedBrand.value = newQuery.Brand || ''
  selectedStatus.value = newQuery.Status !== null && newQuery.Status !== undefined ? String(newQuery.Status) : ''
  setTimeout(() => { isUpdating = false }, 0)
}, { immediate: true })

watch([searchQuery, selectedCategory, selectedBrand, selectedStatus], () => {
  if (isUpdating) return
  
  const categoryId = selectedCategory.value || null
  const brand = selectedBrand.value || null
  const status = selectedStatus.value === '' ? null : parseInt(selectedStatus.value)
  const search = searchQuery.value || null
  
  emit('updateQuery', {
    CategoryId: categoryId,
    Brand: brand,
    Status: status,
    Search: search,
    PageNumber: 1
  })
})

const totalPages = computed(() => Math.ceil(props.totalMotorbikes / 12))

function goToPage(page) {
  if (page >= 1 && page <= totalPages.value) {
    const categoryId = selectedCategory.value || null
    const brand = selectedBrand.value || null
    const status = selectedStatus.value === '' ? null : parseInt(selectedStatus.value)
    const search = searchQuery.value || null
    
    emit('updateQuery', {
      CategoryId: categoryId,
      Brand: brand,
      Status: status,
      Search: search,
      PageNumber: page
    })
  }
}

function clearFilters() {
  searchQuery.value = ''
  selectedCategory.value = ''
  selectedBrand.value = ''
  selectedStatus.value = ''
}

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function getStatusText(status) {
  const statusMap = {
    0: 'Có sẵn',
    1: 'Đang thuê',
    2: 'Bảo trì',
    3: 'Không hoạt động',
    4: 'Đã hư',
    5: 'Chờ xử lý'
  }
  return statusMap[status] || 'Không xác định'
}

function getStatusClass(status) {
  const classMap = {
    0: 'status-available',
    1: 'status-rented',
    2: 'status-maintenance',
    3: 'status-inactive',
    4: 'status-broken',
    5: 'status-pending'
  }
  return classMap[status] || 'status-unknown'
}

function goToDetail(id) {
  router.push(`/Receptionist/motorbike/detail/${id}`)
}

function goToCreateContract(motorbikeId) {
  contractStorage.setMotorbike(motorbikeId)
  router.push(`/receptionist/contract/create`)
}
</script>

<template>
  <div class="motorbike-container">
    <div class="filters">
      <input 
        v-model="searchQuery" 
        type="text" 
        placeholder="Tìm kiếm xe..."
        class="search-input"
      >
      
      <select v-model="selectedCategory" class="filter-select">
        <option value="">Tất cả loại xe</option>
        <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
          {{ cat.categoryName }}
        </option>
      </select>
      
      <select v-model="selectedBrand" class="filter-select">
        <option value="">Tất cả thương hiệu</option>
        <option v-for="brand in brands" :key="brand" :value="brand">
          {{ brand }}
        </option>
      </select>
      <button @click="clearFilters" class="clear-btn">
        Xóa bộ lọc
      </button>
    </div>

    <div v-if="motorbikes.length === 0" class="empty">
      <p>Không tìm thấy xe nào</p>
    </div>

    <div v-else class="motorbikes-grid">
      <div v-for="motorbike in motorbikes" :key="motorbike.motorbikeId" class="motorbike-card">
        <div class="card-image">
          <img 
            :src="motorbike.imageUrl ? getFullPath(motorbike.imageUrl) : '/placeholder-bike.jpg'"
            :alt="motorbike.motorbikeName"
          >
          <span class="status-badge" :class="getStatusClass(motorbike.status)">
            {{ getStatusText(motorbike.status) }}
          </span>
        </div>
        
        <div class="card-content">
          <div class="card-header">
            <h3>{{ motorbike.motorbikeName }}</h3>
            <span class="bike-id">#{{ motorbike.motorbikeId }}</span>
          </div>
          
          <div class="bike-info">
            <div class="info-row">
              <span>Loại:</span>
              <span>{{ motorbike.categoryName }}</span>
            </div>
            <div class="info-row">
              <span>Hãng:</span>
              <span>{{ motorbike.brand }}</span>
            </div>
          </div>
          
          <div class="pricing">
            <div class="price-row">
              <span>Giá theo giờ:</span>
              <span>{{ formatPrice(motorbike.hourlyRate) }}</span>
            </div>
            <div class="price-row">
              <span>Giá theo ngày:</span>
              <span>{{ formatPrice(motorbike.dailyRate) }}</span>
            </div>
          </div>
          
          <div class="card-actions">
            <button @click="goToDetail(motorbike.motorbikeId)" class="detail-btn">
              Chi tiết
            </button>
            <button @click="goToCreateContract(motorbike.motorbikeId)" class="create-contract-btn">
              Tạo hợp đồng
            </button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="totalPages > 1" class="pagination">
      <button 
        @click="goToPage(props.query.PageNumber - 1)" 
        :disabled="props.query.PageNumber === 1"
        class="page-btn"
      >
        ‹
      </button>
      
      <span class="page-info">
        {{ props.query.PageNumber }} / {{ totalPages }}
      </span>
      
      <button 
        @click="goToPage(props.query.PageNumber + 1)" 
        :disabled="props.query.PageNumber >= totalPages"
        class="page-btn"
      >
        ›
      </button>
    </div>
  </div>
</template>

<style scoped>
.motorbike-container {
  padding: 20px;
}

.filters {
  display: flex;
  gap: 12px;
  margin-bottom: 24px;
  padding: 20px;
  background: white;
  border-radius: 8px;
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

.clear-btn {
  padding: 8px 16px;
  background: #f8f9fa;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.clear-btn:hover {
  background: #e9ecef;
}

.empty {
  text-align: center;
  padding: 40px;
  background: white;
  border-radius: 8px;
}

.motorbikes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 20px;
}

.motorbike-card {
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  transition: transform 0.2s;
}

.motorbike-card:hover {
  transform: translateY(-2px);
}

.card-image {
  position: relative;
  height: 200px;
  overflow: hidden;
}

.card-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.status-badge {
  position: absolute;
  top: 8px;
  right: 8px;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 500;
}

.status-available {
  background: #d4edda;
  color: #155724;
}

.status-rented {
  background: #f8d7da;
  color: #721c24;
}

.status-maintenance {
  background: #fff3cd;
  color: #856404;
}

.status-inactive,
.status-broken,
.status-unknown {
  background: #e2e3e5;
  color: #383d41;
}

.status-pending {
  background: #cce5ff;
  color: #004085;
}

.card-content {
  padding: 16px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
}

.card-header h3 {
  margin: 0;
  font-size: 16px;
  color: #333;
}

.bike-id {
  background: #f8f9fa;
  color: #666;
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 12px;
}

.bike-info {
  margin-bottom: 12px;
}

.info-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 4px;
  font-size: 14px;
}

.info-row span:first-child {
  color: #666;
}

.info-row span:last-child {
  color: #333;
  font-weight: 500;
}

.pricing {
  background: #f8f9fa;
  padding: 12px;
  border-radius: 4px;
  margin-bottom: 12px;
}

.price-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 4px;
  font-size: 14px;
}

.price-row:last-child {
  margin-bottom: 0;
}

.price-row span:first-child {
  color: #666;
}

.price-row span:last-child {
  color: #28a745;
  font-weight: 600;
}

.card-actions {
  display: flex;
  gap: 8px;
}

.detail-btn {
  flex: 1;
  padding: 6px 12px;
  background: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 13px;
}

.detail-btn:hover {
  background: #0056b3;
}

.create-contract-btn {
  flex: 1;
  padding: 6px 12px;
  background: #28a745;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 13px;
}

.create-contract-btn:hover {
  background: #218838;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 12px;
  margin-top: 24px;
  padding: 20px;
  background: white;
  border-radius: 8px;
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
  .motorbike-container {
    padding: 10px;
  }
  
  .filters {
    flex-direction: column;
  }
  
  .search-input {
    min-width: auto;
  }
  
  .motorbikes-grid {
    grid-template-columns: 1fr;
  }
}
</style>