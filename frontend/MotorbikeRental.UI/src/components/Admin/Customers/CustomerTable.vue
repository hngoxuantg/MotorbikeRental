<script setup>
import { ref, watch, computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const props = defineProps({
  customers: {
    type: Array,
    required: true,
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
    }),
  },
})

const emit = defineEmits(['update-query'])

const genderOptions = {
  0: 'Nam',
  1: 'Nữ',
  2: 'Khác',
}

const searchValue = ref(props.query.Search || '')

let searchTimeout = null
watch(searchValue, (newVal) => {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    emit('update-query', {
      Search: newVal,
      PageNumber: 1,
    })
  }, 300)
})

const totalPages = computed(() => Math.ceil(props.totalCount / props.query.PageSize))

function changePage(page) {
  if (page >= 1 && page <= totalPages.value) {
    emit('update-query', { PageNumber: page })
  }
}

function clearSearch() {
  searchValue.value = ''
}

function formatDate(dateString) {
  return new Date(dateString).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
  })
}

function getGenderText(gender) {
  return genderOptions[gender] || 'Không xác định'
}

function getGenderBadgeClass(gender) {
  switch (gender) {
    case 0:
      return 'badge-male'
    case 1:
      return 'badge-female'
    default:
      return 'badge-other'
  }
}

function goToDetail(customerId) {
  router.push(`/admin/customer/detail/${customerId}`)
}
</script>

<template>
  <div class="customer-container">
    <!-- Header -->
    <div class="header">
      <h1>Danh sách khách hàng</h1>
      <div class="header-stats">
        <span class="stat-item">Tổng: {{ totalCount }} khách hàng</span>
      </div>
    </div>

    <!-- Search Section -->
    <div class="search-section">
      <div class="search-wrapper">
        <input
          v-model="searchValue"
          class="search-input"
          placeholder="Tìm kiếm khách hàng theo tên hoặc số điện thoại..."
          type="text"
        />
        <button v-if="searchValue" @click="clearSearch" class="clear-btn">
          ×
        </button>
      </div>
      <div v-if="searchValue" class="search-info">
        Tìm kiếm: "{{ searchValue }}" - {{ totalCount }} kết quả
      </div>
    </div>

    <!-- Table Info -->
    <div class="table-info" v-if="!isLoading && totalCount > 0">
      Hiển thị {{ (props.query.PageNumber - 1) * props.query.PageSize + 1 }} - 
      {{ Math.min(props.query.PageNumber * props.query.PageSize, totalCount) }} 
      trong {{ totalCount }} khách hàng
    </div>

    <!-- Content -->
    <div class="content">
      <!-- Loading State -->
      <div v-if="isLoading" class="loading">
        <p>Đang tải dữ liệu...</p>
      </div>

      <!-- Table -->
      <div v-else-if="customers && customers.length > 0" class="table-container">
        <table class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Họ tên</th>
              <th>Giới tính</th>
              <th>Số điện thoại</th>
              <th>Lượt thuê</th>
              <th>Ngày tạo</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="customer in customers" :key="customer.customerId">
              <td>
                <span class="customer-id">#{{ customer.customerId }}</span>
              </td>
              <td>
                <span class="customer-name">{{ customer.fullName }}</span>
              </td>
              <td>
                <span class="gender-badge" :class="getGenderBadgeClass(customer.gender)">
                  {{ getGenderText(customer.gender) }}
                </span>
              </td>
              <td>
                <a :href="`tel:${customer.phoneNumber}`" class="phone-link">
                  {{ customer.phoneNumber }}
                </a>
              </td>
              <td>
                <span class="rental-count">{{ customer.rentalCount }}</span>
              </td>
              <td>
                <span class="date-text">{{ formatDate(customer.createAt) }}</span>
              </td>
              <td>
                <button class="btn-detail" @click="goToDetail(customer.customerId)">
                  Chi tiết
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Empty State -->
      <div v-else class="empty-state">
        <h3>{{ searchValue ? 'Không tìm thấy kết quả' : 'Chưa có khách hàng nào' }}</h3>
        <p v-if="searchValue">Không tìm thấy khách hàng nào với từ khóa "{{ searchValue }}"</p>
        <p v-else>Chưa có khách hàng nào được đăng ký trong hệ thống</p>
        <button v-if="searchValue" @click="clearSearch" class="btn-primary">
          Xóa tìm kiếm
        </button>
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
.customer-container {
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

.header-stats {
  display: flex;
  gap: 20px;
}

.stat-item {
  font-size: 14px;
  color: #666;
  font-weight: 500;
}

.search-section {
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.search-wrapper {
  position: relative;
  margin-bottom: 8px;
}

.search-input {
  width: 100%;
  padding: 8px 12px;
  padding-right: 40px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.search-input:focus {
  outline: none;
  border-color: #007bff;
}

.clear-btn {
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
  font-size: 14px;
}

.clear-btn:hover {
  background: #5a6268;
}

.search-info {
  font-size: 14px;
  color: #666;
  font-style: italic;
}

.table-info {
  padding: 12px 20px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  margin-bottom: 20px;
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
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #666;
}

.table-container {
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
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
}

.table tr:hover {
  background: #f8f9fa;
}

.customer-id {
  font-weight: 600;
  color: #6366f1;
}

.customer-name {
  font-weight: 500;
  color: #333;
}

.gender-badge {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.badge-male {
  background: #e3f2fd;
  color: #1565c0;
}

.badge-female {
  background: #fce4ec;
  color: #ad1457;
}

.badge-other {
  background: #f5f5f5;
  color: #666;
}

.phone-link {
  color: #28a745;
  text-decoration: none;
}

.phone-link:hover {
  text-decoration: underline;
}

.rental-count {
  font-weight: 600;
  color: #28a745;
}

.date-text {
  color: #666;
  font-size: 13px;
}

.btn-detail {
  padding: 6px 12px;
  border: 1px solid #ddd;
  background: white;
  color: #007bff;
  border-radius: 4px;
  font-size: 13px;
  cursor: pointer;
}

.btn-detail:hover {
  background: #f8f9fa;
  border-color: #007bff;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #666;
}

.empty-state h3 {
  margin: 0 0 8px 0;
  color: #333;
}

.btn-primary {
  background: #007bff;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-primary:hover {
  background: #0056b3;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 12px;
  margin-top: 20px;
  padding: 20px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
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
  .customer-container {
    padding: 10px;
  }

  .header {
    flex-direction: column;
    gap: 16px;
  }

  .search-section {
    padding: 16px;
  }

  .table-info {
    padding: 12px 16px;
  }

  .pagination {
    padding: 16px;
  }
}
</style>