<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const props = defineProps({
  categories: {
    type: Array,
    required: true,
    default: () => []
  }
})

const emit = defineEmits(['create-category'])

function goToDetail(categoryId) {
  router.push(`/admin/category/${categoryId}`)
}

function createCategory() {
  emit('create-category')
}

function formatCurrency(amount) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(amount)
}
</script>

<template>
  <div class="category-container">
    <!-- Header -->
    <div class="header">
      <h1>Danh sách loại xe</h1>
      <div class="header-actions">
        <button @click="createCategory" class="btn-create">
          Thêm loại xe
        </button>
        <span class="total-count">{{ categories.length }} loại xe</span>
      </div>
    </div>

    <!-- Content -->
    <div class="content">
      <!-- Empty State -->
      <div v-if="categories.length === 0" class="empty-state">
        <h3>Chưa có loại xe nào</h3>
        <p>Hệ thống chưa có loại xe nào được tạo</p>
        <button @click="createCategory" class="btn-primary">
          Tạo loại xe đầu tiên
        </button>
      </div>

      <!-- Table -->
      <div v-else class="table-container">
        <table class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Tên loại xe</th>
              <th>Tiền cọc</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="category in categories" :key="category.categoryId">
              <td>
                <span class="category-id">#{{ category.categoryId }}</span>
              </td>
              <td>
                <span class="category-name">{{ category.categoryName }}</span>
              </td>
              <td>
                <span class="deposit-amount">{{ formatCurrency(category.depositAmount) }}</span>
              </td>
              <td>
                <button class="btn-detail" @click="goToDetail(category.categoryId)">
                  Chi tiết
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>
.category-container {
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

.header-actions {
  display: flex;
  align-items: center;
  gap: 16px;
}

.btn-create {
  background: #007bff;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.btn-create:hover {
  background: #0056b3;
}

.total-count {
  font-size: 14px;
  color: #666;
}

.content {
  min-height: 400px;
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
  padding: 10px 20px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  margin-top: 16px;
}

.btn-primary:hover {
  background: #0056b3;
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

.table tr:last-child td {
  border-bottom: none;
}

.category-id {
  font-weight: 600;
  color: #6366f1;
}

.category-name {
  font-weight: 500;
  color: #333;
}

.deposit-amount {
  font-weight: 600;
  color: #28a745;
}

.btn-detail {
  background: #007bff;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 13px;
  cursor: pointer;
}

.btn-detail:hover {
  background: #0056b3;
}

@media (max-width: 768px) {
  .category-container {
    padding: 10px;
  }

  .header {
    flex-direction: column;
    gap: 16px;
  }

  .header-actions {
    flex-direction: column;
    gap: 8px;
  }

  .table-container {
    overflow-x: auto;
  }

  .table {
    min-width: 500px;
  }
}
</style>