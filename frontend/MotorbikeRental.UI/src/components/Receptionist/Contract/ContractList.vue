<script setup>
import { ref, watch, computed } from 'vue'
import { useRouter } from 'vue-router'
import { getFullPath } from '@/utils/UrlUtils'

const router = useRouter()

const props = defineProps({
  contracts: {
    type: Array,
    required: true,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  totalCount: {
    type: Number,
    default: 0,
  },
  query: {
    type: Object,
    default: () => ({
      Search: '',
      PageNumber: 1,
      PageSize: 12,
      Status: '',
      FromDate: '',
      ToDate: '',
    }),
  },
})

const emit = defineEmits(['update-query'])

const searchValue = ref(props.query.Search || '')
const filterStatus = ref(props.query.Status || '')
const filterFromDate = ref(props.query.FromDate || '')
const filterToDate = ref(props.query.ToDate || '')

let searchTimeout = null
watch(searchValue, (newVal) => {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    emit('update-query', { Search: newVal, PageNumber: 1 })
  }, 300)
})

watch(filterStatus, (newVal) => {
  emit('update-query', { Status: newVal, PageNumber: 1 })
})

watch(filterFromDate, (newVal) => {
  emit('update-query', { FromDate: newVal, PageNumber: 1 })
})

watch(filterToDate, (newVal) => {
  emit('update-query', { ToDate: newVal, PageNumber: 1 })
})

const totalPages = computed(() => Math.ceil(props.totalCount / props.query.PageSize))

function changePage(page) {
  if (page >= 1 && page <= totalPages.value) {
    emit('update-query', { PageNumber: page })
  }
}

function clearFilters() {
  searchValue.value = ''
  filterStatus.value = ''
  filterFromDate.value = ''
  filterToDate.value = ''
  emit('update-query', {
    Search: '',
    Status: '',
    FromDate: '',
    ToDate: '',
    PageNumber: 1,
  })
}

function goToDetail(contractId) {
  emit('go-to-detail', contractId)
}

function formatDate(dateString) {
  if (!dateString) return ''
  return new Date(dateString).toLocaleDateString('vi-VN')
}

function formatCurrency(amount) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
  }).format(amount)
}

function getStatusText(status) {
  const statusMap = {
    0: 'Đang thuê',
    1: 'Đã trả xe',
    2: 'Đã hủy',
    3: 'Đang hẹn',
    4: 'Đang xử lý sự cố',
  }
  return statusMap[status] || 'Không xác định'
}

function getStatusClass(status) {
  const classMap = {
    0: 'status-pending',
    1: 'status-confirmed',
    2: 'status-renting',
    3: 'status-returned',
    4: 'status-cancelled',
    5: 'status-incident',
  }
  return classMap[status] || 'status-unknown'
}

function getRentalTypeText(rentalTypeStatus) {
  const typeMap = {
    0: 'Thuê theo ngày',
    1: 'Thuê theo tháng',
  }
  return typeMap[rentalTypeStatus] || 'Không xác định'
}

function onImgError(event) {
  if (!event.target.src.endsWith('/placeholder-bike.jpg')) {
    event.target.src = '/placeholder-bike.jpg'
  }
}
</script>

<template>
  <div class="contract-container">
    <!-- Header -->
    <div class="header">
      <h1>Danh sách hợp đồng</h1>
      <div class="header-stats">
        <span>{{ totalCount }} hợp đồng</span>
      </div>
    </div>

    <!-- Filters -->
    <div class="filters">
      <!-- Search -->
      <div class="search-section">
        <div class="search-box">
          <input
            v-model="searchValue"
            class="search-input"
            placeholder="Tìm kiếm theo tên khách hàng hoặc xe máy..."
            type="text"
          />
          <button v-if="searchValue" @click="searchValue = ''" class="clear-search">×</button>
        </div>
        <div class="search-info">
          <span v-if="searchValue">
            Tìm kiếm: "{{ searchValue }}" - {{ totalCount }} kết quả
          </span>
        </div>
      </div>

      <!-- Filter Controls -->
      <div class="filter-controls">
        <div class="filter-item">
          <label>Trạng thái</label>
          <select v-model="filterStatus" class="filter-select">
            <option value="">Tất cả</option>
            <option value="0">Đang thuê</option>
            <option value="1">Đã hoàn thành</option>
            <option value="2">Đã hủy</option>
            <option value="3">Đang hẹn</option>
            <option value="4">Đang xử lý sự cố</option>
          </select>
        </div>

        <div class="filter-item">
          <label>Từ ngày</label>
          <input v-model="filterFromDate" type="date" class="filter-input" />
        </div>

        <div class="filter-item">
          <label>Đến ngày</label>
          <input v-model="filterToDate" type="date" class="filter-input" />
        </div>

        <button @click="clearFilters" class="btn-clear">Xóa bộ lọc</button>
      </div>
    </div>

    <!-- Table Info -->
    <div class="table-info" v-if="!isLoading && totalCount > 0">
      Hiển thị {{ (props.query.PageNumber - 1) * props.query.PageSize + 1 }} - 
      {{ Math.min(props.query.PageNumber * props.query.PageSize, totalCount) }} 
      trong {{ totalCount }} hợp đồng
    </div>

    <!-- Content -->
    <div class="content">
      <!-- Loading -->
      <div v-if="isLoading" class="loading">
        <p>Đang tải dữ liệu...</p>
      </div>

      <!-- Table -->
      <div v-else-if="contracts && contracts.length > 0" class="table-container">
        <table class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Khách hàng</th>
              <th>Xe máy</th>
              <th>Loại thuê</th>
              <th>Ngày thuê</th>
              <th>Ngày trả dự kiến</th>
              <th>Tổng tiền</th>
              <th>Trạng thái</th>
              <th>Thanh toán</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="contract in contracts" :key="contract.contractId">
              <td>
                <span class="contract-id">#{{ contract.contractId }}</span>
              </td>
              <td>
                <span class="customer-name">{{ contract.customerName }}</span>
              </td>
              <td>
                <div class="motorbike-info">
                  <img
                    :src="contract.imageUrlMotorbike ? getFullPath(contract.imageUrlMotorbike) : '/placeholder-bike.jpg'"
                    :alt="contract.motorbikeName"
                    @error="onImgError"
                    class="motorbike-image"
                  />
                  <span class="motorbike-name">{{ contract.motorbikeName }}</span>
                </div>
              </td>
              <td>
                <span class="rental-type">{{ getRentalTypeText(contract.rentalTypeStatus) }}</span>
              </td>
              <td>
                <span class="date">{{ formatDate(contract.rentalDate) }}</span>
              </td>
              <td>
                <span class="date">{{ formatDate(contract.expectedReturnDate) }}</span>
              </td>
              <td>
                <span class="amount">{{ formatCurrency(contract.totalAmount) }}</span>
              </td>
              <td>
                <span class="status" :class="getStatusClass(contract.status)">
                  {{ getStatusText(contract.status) }}
                </span>
              </td>
              <td>
                <span class="payment-status" :class="contract.isPaid ? 'paid' : 'unpaid'">
                  {{ contract.isPaid ? 'Đã thanh toán' : 'Chưa thanh toán' }}
                </span>
              </td>
              <td>
                <button class="btn-detail" @click="goToDetail(contract.contractId)">
                  Chi tiết
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Empty State -->
      <div v-else class="empty-state">
        <h3>{{ searchValue ? 'Không tìm thấy kết quả' : 'Chưa có hợp đồng nào' }}</h3>
        <p v-if="searchValue">Không tìm thấy hợp đồng nào với từ khóa "{{ searchValue }}"</p>
        <p v-else>Chưa có hợp đồng nào được tạo trong hệ thống</p>
      </div>
    </div>

    <!-- Pagination -->
    <div v-if="totalPages > 1" class="pagination">
      <button
        :disabled="props.query.PageNumber === 1"
        @click="changePage(props.query.PageNumber - 1)"
        class="page-btn"
      >
        ‹
      </button>
      
      <span class="page-info">
        Trang {{ props.query.PageNumber }} / {{ totalPages }}
      </span>
      
      <button
        :disabled="props.query.PageNumber >= totalPages"
        @click="changePage(props.query.PageNumber + 1)"
        class="page-btn"
      >
        ›
      </button>
    </div>
  </div>
</template>

<style scoped>
.contract-container {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  margin: 20px;
  overflow: hidden;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 24px;
  background: white;
  border-bottom: 1px solid #eee;
}

.payment-status {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.payment-status.paid {
  background: #d4edda;
  color: #155724;
}

.payment-status.unpaid {
  background: #f8d7da;
  color: #721c24;
}

.status-incident {
  background: #f8d7da;
  color: #721c24;
}
.status-incident {
  background: #f8d7da;
  color: #721c24;
}
.header h1 {
  font-size: 24px;
  font-weight: 600;
  margin: 0;
  color: #333;
}

.header-stats {
  font-size: 14px;
  color: #666;
}

.filters {
  background: #f8f9fa;
  padding: 20px 24px;
  border-bottom: 1px solid #eee;
}

.search-section {
  margin-bottom: 16px;
}

.search-box {
  position: relative;
  max-width: 400px;
}

.search-input {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.search-input:focus {
  outline: none;
  border-color: #007bff;
}

.clear-search {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: #6c757d;
  color: white;
  border: none;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  cursor: pointer;
  font-size: 12px;
}

.clear-search:hover {
  background: #5a6268;
}

.search-info {
  margin-top: 8px;
  font-size: 14px;
  color: #666;
  font-style: italic;
}

.filter-controls {
  display: flex;
  align-items: end;
  gap: 16px;
  flex-wrap: wrap;
}

.filter-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.filter-item label {
  font-size: 12px;
  color: #666;
  font-weight: 500;
}

.filter-input,
.filter-select {
  padding: 6px 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  min-width: 120px;
}

.filter-input:focus,
.filter-select:focus {
  outline: none;
  border-color: #007bff;
}

.btn-clear {
  background: #6c757d;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-clear:hover {
  background: #5a6268;
}

.table-info {
  padding: 12px 24px;
  background: white;
  border-bottom: 1px solid #f0f0f0;
  font-size: 14px;
  color: #666;
}

.content {
  min-height: 400px;
}

.loading {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 60px;
  color: #666;
}

.table-container {
  overflow-x: auto;
}

.table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

.table th {
  background: #f8f9fa;
  padding: 12px;
  text-align: left;
  font-weight: 600;
  color: #333;
  border-bottom: 1px solid #ddd;
}

.table td {
  padding: 12px;
  border-bottom: 1px solid #f0f0f0;
  vertical-align: middle;
}

.table tr:hover {
  background: #f8f9fa;
}

.contract-id {
  font-weight: 600;
  color: #6366f1;
}

.customer-name {
  font-weight: 500;
  color: #333;
}

.motorbike-info {
  display: flex;
  align-items: center;
  gap: 8px;
}

.motorbike-image {
  width: 40px;
  height: 40px;
  border-radius: 4px;
  object-fit: cover;
}

.motorbike-name {
  font-weight: 500;
  color: #333;
}

.rental-type {
  color: #666;
  font-size: 13px;
}

.date {
  color: #666;
  font-size: 13px;
}

.amount {
  font-weight: 600;
  color: #28a745;
}

.status {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.status-pending {
  background: #fff3cd;
  color: #856404;
}

.status-confirmed {
  background: #cce5ff;
  color: #0066cc;
}

.status-renting {
  background: #f8d7da;
  color: #721c24;
}

.status-returned {
  background: #d4edda;
  color: #155724;
}

.status-cancelled {
  background: #f8f9fa;
  color: #6c757d;
}

.btn-detail {
  background: white;
  border: 1px solid #ddd;
  color: #007bff;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 13px;
  cursor: pointer;
}

.btn-detail:hover {
  background: #f8f9fa;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #666;
}

.empty-state h3 {
  margin: 0 0 8px 0;
  color: #333;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 12px;
  padding: 20px 24px;
  background: #f8f9fa;
  border-top: 1px solid #eee;
}

.page-btn {
  background: white;
  border: 1px solid #ddd;
  color: #333;
  padding: 6px 10px;
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
  color: #333;
  font-weight: 500;
}

@media (max-width: 768px) {
  .contract-container {
    margin: 10px;
  }

  .header {
    flex-direction: column;
    gap: 12px;
  }

  .filter-controls {
    flex-direction: column;
    align-items: stretch;
  }

  .filter-item {
    width: 100%;
  }
}
</style>