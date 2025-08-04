<script setup>
import { ref, watch, computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const props = defineProps({
  discounts: {
    type: Array,
    required: true,
    default: () => []
  },
  isLoading: {
    type: Boolean,
    default: false
  },
  totalCount: {
    type: Number,
    default: 0
  },
  query: {
    type: Object,
    default: () => ({
      Search: '',
      PageNumber: 1,
      PageSize: 12,
      FilterStartDate: null,
      FilterEndDate: null,
      IsActive: null
    })
  }
})

const emit = defineEmits(['updateQuery', 'create-discount'])

// Form state
const searchValue = ref(props.query.Search || '')
const filterStartDate = ref(props.query.FilterStartDate || '')
const filterEndDate = ref(props.query.FilterEndDate || '')
const filterIsActive = ref(props.query.IsActive !== null ? props.query.IsActive : '')

// Search debounce
let searchTimeout = null
watch(searchValue, (newVal) => {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    emit('updateQuery', { Search: newVal, PageNumber: 1 })
  }, 300)
})

// Filter watchers
watch(filterStartDate, (newVal) => {
  emit('updateQuery', { FilterStartDate: newVal || null, PageNumber: 1 })
})

watch(filterEndDate, (newVal) => {
  emit('updateQuery', { FilterEndDate: newVal || null, PageNumber: 1 })
})

watch(filterIsActive, (newVal) => {
  emit('updateQuery', { IsActive: newVal === '' ? null : newVal === 'true', PageNumber: 1 })
})

// Computed
const totalPages = computed(() => Math.ceil(props.totalCount / props.query.PageSize))
const showingFrom = computed(() => (props.query.PageNumber - 1) * props.query.PageSize + 1)
const showingTo = computed(() => Math.min(props.query.PageNumber * props.query.PageSize, props.totalCount))

// Methods
function changePage(page) {
  if (page >= 1 && page <= totalPages.value) {
    emit('updateQuery', { PageNumber: page })
  }
}

function changePageSize(size) {
  emit('updateQuery', { PageSize: parseInt(size), PageNumber: 1 })
}

function clearSearch() {
  searchValue.value = ''
}

function clearFilters() {
  searchValue.value = ''
  filterStartDate.value = ''
  filterEndDate.value = ''
  filterIsActive.value = ''
  emit('updateQuery', {
    Search: '',
    FilterStartDate: null,
    FilterEndDate: null,
    IsActive: null,
    PageNumber: 1
  })
}

function createDiscount() {
  emit('create-discount')
}

function goToDetail(discountId) {
  router.push(`/admin/discount/${discountId}`)
}

function formatDate(dateString) {
  return new Date(dateString).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit'
  })
}

function getStatusBadgeClass(isActive) {
  return isActive ? 'status-active' : 'status-inactive'
}

function getStatusText(isActive) {
  return isActive ? 'Hoạt động' : 'Không hoạt động'
}
</script>

<template>
  <div class="discount-container">
    <!-- Header -->
    <div class="page-header">
      <div class="header-left">
        <h2 class="page-title">Quản lý mã giảm giá</h2>
        <span class="total-info">Tổng: {{ totalCount }} mã giảm giá</span>
      </div>
      <div class="header-right">
        <button class="btn btn-primary" @click="createDiscount">
          Tạo mã giảm giá
        </button>
      </div>
    </div>

    <!-- Toolbar -->
    <div class="toolbar">
      <div class="toolbar-left">
        <div class="search-box">
          <input
            v-model="searchValue"
            class="search-input"
            placeholder="Tìm kiếm theo tên..."
            type="text"
          />
          <button v-if="searchValue" @click="clearSearch" class="clear-btn">×</button>
        </div>
      </div>
      
      <div class="toolbar-right">
        <select v-model="filterIsActive" class="filter-select">
          <option value="">Tất cả trạng thái</option>
          <option value="true">Hoạt động</option>
          <option value="false">Không hoạt động</option>
        </select>
        
        <input
          v-model="filterStartDate"
          type="date"
          class="filter-input"
          title="Ngày bắt đầu"
        />
        
        <input
          v-model="filterEndDate"
          type="date"
          class="filter-input"
          title="Ngày kết thúc"
        />
        
        <select
          :value="props.query.PageSize"
          @change="changePageSize($event.target.value)"
          class="filter-select"
        >
          <option :value="10">10/trang</option>
          <option :value="20">20/trang</option>
          <option :value="50">50/trang</option>
        </select>
        
        <button @click="clearFilters" class="btn btn-secondary">
          Xóa bộ lọc
        </button>
      </div>
    </div>

    <!-- Status info -->
    <div class="status-info" v-if="!isLoading && totalCount > 0">
      Hiển thị {{ showingFrom }} - {{ showingTo }} trong {{ totalCount }} kết quả
    </div>

    <!-- Content -->
    <div class="content">
      <!-- Loading -->
      <div v-if="isLoading" class="loading">
        <div class="loading-text">Đang tải dữ liệu...</div>
      </div>

      <!-- Table -->
      <div v-else-if="discounts && discounts.length > 0" class="table-wrapper">
        <table class="data-table">
          <thead>
            <tr>
              <th width="80">ID</th>
              <th>Tên mã giảm giá</th>
              <th width="150">Loại xe</th>
              <th width="80">Giá trị</th>
              <th width="120">Ngày bắt đầu</th>
              <th width="120">Ngày kết thúc</th>
              <th width="100">Trạng thái</th>
              <th width="100">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="discount in discounts" :key="discount.discountId">
              <td>
                <span class="id-text">#{{ discount.discountId }}</span>
              </td>
              <td>
                <div class="name-cell">{{ discount.name }}</div>
              </td>
              <td>
                <div class="category-tags">
                  <span 
                    v-for="category in discount.categoryNames" 
                    :key="category"
                    class="category-tag"
                  >
                    {{ category }}
                  </span>
                </div>
              </td>
              <td class="text-center">
                <span class="value-text">{{ discount.value }}%</span>
              </td>
              <td class="text-center">
                <span class="date-text">{{ formatDate(discount.startDate) }}</span>
              </td>
              <td class="text-center">
                <span class="date-text">{{ formatDate(discount.endDate) }}</span>
              </td>
              <td class="text-center">
                <span class="status-badge" :class="getStatusBadgeClass(discount.isActive)">
                  {{ getStatusText(discount.isActive) }}
                </span>
              </td>
              <td class="text-center">
                <button class="btn btn-sm" @click="goToDetail(discount.discountId)">
                  Chi tiết
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Empty State -->
      <div v-else class="empty-state">
        <div class="empty-text">
          {{ searchValue ? 'Không tìm thấy kết quả' : 'Chưa có mã giảm giá nào' }}
        </div>
        <div v-if="searchValue" class="empty-subtext">
          Không tìm thấy mã giảm giá nào với từ khóa "{{ searchValue }}"
        </div>
        <button v-if="searchValue" @click="clearSearch" class="btn btn-primary">
          Xóa tìm kiếm
        </button>
      </div>
    </div>

    <!-- Pagination -->
    <div v-if="totalPages > 1" class="pagination">
      <button
        :disabled="props.query.PageNumber === 1"
        @click="changePage(1)"
        class="page-btn"
      >
        Đầu
      </button>
      <button
        :disabled="props.query.PageNumber === 1"
        @click="changePage(props.query.PageNumber - 1)"
        class="page-btn"
      >
        Trước
      </button>

      <span class="page-info">
        Trang {{ props.query.PageNumber }} / {{ totalPages }}
      </span>

      <button
        :disabled="props.query.PageNumber >= totalPages"
        @click="changePage(props.query.PageNumber + 1)"
        class="page-btn"
      >
        Sau
      </button>
      <button
        :disabled="props.query.PageNumber >= totalPages"
        @click="changePage(totalPages)"
        class="page-btn"
      >
        Cuối
      </button>
    </div>
  </div>
</template>

<style scoped>
.discount-container {
  background: white;
  margin: 20px;
  border: 1px solid #ddd;
}

/* Header */
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 20px;
  background: #f8f9fa;
  border-bottom: 1px solid #ddd;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 15px;
}

.page-title {
  margin: 0;
  font-size: 18px;
  font-weight: bold;
  color: #333;
}

.total-info {
  font-size: 13px;
  color: #666;
  background: #e9ecef;
  padding: 4px 8px;
  border-radius: 3px;
}

/* Toolbar */
.toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 20px;
  background: #fff;
  border-bottom: 1px solid #eee;
}

.toolbar-left {
  flex: 1;
}

.search-box {
  position: relative;
  max-width: 300px;
}

.search-input {
  width: 100%;
  padding: 6px 30px 6px 10px;
  border: 1px solid #ccc;
  border-radius: 3px;
  font-size: 13px;
}

.clear-btn {
  position: absolute;
  right: 5px;
  top: 50%;
  transform: translateY(-50%);
  background: #999;
  color: white;
  border: none;
  border-radius: 50%;
  width: 18px;
  height: 18px;
  cursor: pointer;
  font-size: 12px;
}

.toolbar-right {
  display: flex;
  align-items: center;
  gap: 10px;
}

.filter-input,
.filter-select {
  padding: 6px 8px;
  border: 1px solid #ccc;
  border-radius: 3px;
  font-size: 13px;
  min-width: 100px;
}

/* Status info */
.status-info {
  padding: 8px 20px;
  background: #f8f9fa;
  border-bottom: 1px solid #eee;
  font-size: 13px;
  color: #666;
}

/* Content */
.content {
  min-height: 400px;
}

.loading {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 200px;
}

.loading-text {
  color: #666;
  font-size: 14px;
}

/* Table */
.table-wrapper {
  overflow-x: auto;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
}

.data-table th {
  background: #f1f3f4;
  padding: 10px 8px;
  text-align: left;
  font-weight: bold;
  color: #333;
  border-bottom: 2px solid #ddd;
  border-right: 1px solid #ddd;
}

.data-table th:last-child {
  border-right: none;
}

.data-table td {
  padding: 8px;
  border-bottom: 1px solid #eee;
  border-right: 1px solid #f0f0f0;
  vertical-align: middle;
}

.data-table td:last-child {
  border-right: none;
}

.data-table tr:hover {
  background: #f8f9fa;
}

.text-center {
  text-align: center;
}

.id-text {
  font-weight: bold;
  color: #007bff;
}

.name-cell {
  font-weight: 500;
  color: #333;
}

.category-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 2px;
}

.category-tag {
  background: #e3f2fd;
  color: #1976d2;
  padding: 2px 6px;
  border-radius: 2px;
  font-size: 11px;
  font-weight: 500;
}

.value-text {
  font-weight: bold;
  color: #28a745;
}

.date-text {
  color: #666;
  font-size: 12px;
}

.status-badge {
  padding: 3px 8px;
  border-radius: 3px;
  font-size: 11px;
  font-weight: bold;
}

.status-active {
  background: #d4edda;
  color: #155724;
}

.status-inactive {
  background: #f8d7da;
  color: #721c24;
}

/* Buttons */
.btn {
  padding: 6px 12px;
  border: 1px solid #ccc;
  border-radius: 3px;
  background: white;
  color: #333;
  cursor: pointer;
  font-size: 13px;
  font-weight: normal;
}

.btn:hover {
  background: #f8f9fa;
}

.btn-primary {
  background: #007bff;
  border-color: #007bff;
  color: white;
}

.btn-primary:hover {
  background: #0056b3;
  border-color: #0056b3;
}

.btn-secondary {
  background: #6c757d;
  border-color: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #545b62;
  border-color: #545b62;
}

.btn-sm {
  padding: 4px 8px;
  font-size: 12px;
}

/* Empty State */
.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #666;
}

.empty-text {
  font-size: 16px;
  font-weight: bold;
  margin-bottom: 8px;
}

.empty-subtext {
  font-size: 14px;
  margin-bottom: 20px;
}

/* Pagination */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 5px;
  padding: 15px 20px;
  background: #f8f9fa;
  border-top: 1px solid #ddd;
}

.page-btn {
  padding: 5px 10px;
  border: 1px solid #ccc;
  background: white;
  color: #333;
  cursor: pointer;
  font-size: 13px;
  border-radius: 3px;
}

.page-btn:hover:not(:disabled) {
  background: #f8f9fa;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-info {
  margin: 0 15px;
  font-size: 13px;
  color: #666;
  font-weight: bold;
}

/* Responsive */
@media (max-width: 768px) {
  .discount-container {
    margin: 10px;
  }

  .page-header {
    flex-direction: column;
    gap: 10px;
    align-items: stretch;
  }

  .toolbar {
    flex-direction: column;
    gap: 10px;
    align-items: stretch;
  }

  .toolbar-right {
    flex-wrap: wrap;
  }

  .search-box {
    max-width: none;
  }

  .table-wrapper {
    font-size: 12px;
  }

  .data-table th,
  .data-table td {
    padding: 6px 4px;
  }
}
</style>