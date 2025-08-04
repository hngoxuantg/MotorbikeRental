<script setup>
import { ref, reactive } from 'vue'

const props = defineProps({
  categories: {
    type: Array,
    required: true,
    default: () => []
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['submit', 'back'])

const form = reactive({
  name: '',
  categoryId: [],
  description: '',
  value: '',
  startDate: '',
  endDate: '',
  isActive: true
})

const errors = ref({})

function validateForm() {
  errors.value = {}
  
  if (!form.name.trim()) {
    errors.value.name = 'Tên mã giảm giá là bắt buộc'
  }
  
  if (form.categoryId.length === 0) {
    errors.value.categoryId = 'Vui lòng chọn ít nhất một loại xe'
  }
  
  if (!form.value || form.value <= 0 || form.value > 100) {
    errors.value.value = 'Giá trị giảm giá phải từ 1-100%'
  }
  
  if (!form.startDate) {
    errors.value.startDate = 'Ngày bắt đầu là bắt buộc'
  }
  
  if (!form.endDate) {
    errors.value.endDate = 'Ngày kết thúc là bắt buộc'
  }
  
  if (form.startDate && form.endDate && new Date(form.startDate) >= new Date(form.endDate)) {
    errors.value.endDate = 'Ngày kết thúc phải sau ngày bắt đầu'
  }
  
  return Object.keys(errors.value).length === 0
}

function handleSubmit() {
  if (!validateForm()) return
  
  const submitData = {
    name: form.name,
    categoryId: form.categoryId,
    description: form.description,
    value: parseFloat(form.value),
    startDate: form.startDate,
    endDate: form.endDate,
    isActive: form.isActive
  }
  
  emit('submit', submitData)
}

function handleBack() {
  emit('back')
}

function resetForm() {
  form.name = ''
  form.categoryId = []
  form.description = ''
  form.value = ''
  form.startDate = ''
  form.endDate = ''
  form.isActive = true
  errors.value = {}
}

function toggleCategory(categoryId) {
  const index = form.categoryId.indexOf(categoryId)
  if (index > -1) {
    form.categoryId.splice(index, 1)
  } else {
    form.categoryId.push(categoryId)
  }
}
</script>

<template>
  <div class="discount-create-container">
    <!-- Header -->
    <div class="header">
      <h1>Tạo mã giảm giá mới</h1>
      <button @click="handleBack" class="btn-back">
        ← Quay lại
      </button>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form @submit.prevent="handleSubmit" class="form">
        <div class="form-section">
          <h2>Thông tin cơ bản</h2>
          
          <div class="form-group">
            <label>Tên mã giảm giá *</label>
            <input
              v-model="form.name"
              type="text"
              :class="{ 'error': errors.name }"
              placeholder="Nhập tên mã giảm giá"
            />
            <span v-if="errors.name" class="error-message">{{ errors.name }}</span>
          </div>

          <div class="form-group">
            <label>Giá trị giảm giá (%) *</label>
            <input
              v-model="form.value"
              type="number"
              :class="{ 'error': errors.value }"
              placeholder="Nhập % giảm giá"
              min="1"
              max="100"
            />
            <span v-if="errors.value" class="error-message">{{ errors.value }}</span>
          </div>

          <div class="form-group">
            <label>Trạng thái</label>
            <select v-model="form.isActive">
              <option :value="true">Hoạt động</option>
              <option :value="false">Không hoạt động</option>
            </select>
          </div>
        </div>

        <div class="form-section">
          <h2>Thời gian áp dụng</h2>
          
          <div class="form-row">
            <div class="form-group">
              <label>Ngày bắt đầu *</label>
              <input
                v-model="form.startDate"
                type="date"
                :class="{ 'error': errors.startDate }"
              />
              <span v-if="errors.startDate" class="error-message">{{ errors.startDate }}</span>
            </div>

            <div class="form-group">
              <label>Ngày kết thúc *</label>
              <input
                v-model="form.endDate"
                type="date"
                :class="{ 'error': errors.endDate }"
              />
              <span v-if="errors.endDate" class="error-message">{{ errors.endDate }}</span>
            </div>
          </div>
        </div>

        <div class="form-section">
          <h2>Loại xe áp dụng</h2>
          
          <div class="category-grid">
            <div
              v-for="category in categories"
              :key="category.categoryId"
              class="category-item"
              :class="{ 'selected': form.categoryId.includes(category.categoryId) }"
              @click="toggleCategory(category.categoryId)"
            >
              <input
                type="checkbox"
                :checked="form.categoryId.includes(category.categoryId)"
                @change="toggleCategory(category.categoryId)"
              />
              <span>{{ category.categoryName }}</span>
            </div>
          </div>
          <span v-if="errors.categoryId" class="error-message">{{ errors.categoryId }}</span>
        </div>

        <div class="form-section">
          <h2>Mô tả</h2>
          
          <div class="form-group">
            <textarea
              v-model="form.description"
              placeholder="Nhập mô tả mã giảm giá (không bắt buộc)"
              rows="3"
            ></textarea>
          </div>
        </div>

        <!-- Actions -->
        <div class="form-actions">
          <button
            type="button"
            @click="resetForm"
            class="btn-secondary"
            :disabled="isLoading"
          >
            Làm mới
          </button>
          <button
            type="submit"
            class="btn-primary"
            :disabled="isLoading"
          >
            {{ isLoading ? 'Đang tạo...' : 'Tạo mã giảm giá' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.discount-create-container {
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

.btn-back {
  background: #6c757d;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.btn-back:hover {
  background: #5a6268;
}

.form-container {
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 20px;
}

.form-section {
  margin-bottom: 30px;
  padding-bottom: 20px;
  border-bottom: 1px solid #eee;
}

.form-section:last-child {
  border-bottom: none;
}

.form-section h2 {
  margin: 0 0 15px 0;
  font-size: 18px;
  color: #333;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group label {
  font-size: 14px;
  font-weight: 500;
  color: #333;
}

.form-group input,
.form-group select,
.form-group textarea {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #007bff;
}

.form-group input.error,
.form-group select.error,
.form-group textarea.error {
  border-color: #dc3545;
}

.form-group textarea {
  resize: vertical;
  min-height: 80px;
}

.error-message {
  font-size: 12px;
  color: #dc3545;
}

.category-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 10px;
}

.category-item {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
}

.category-item:hover {
  background: #f8f9fa;
  border-color: #007bff;
}

.category-item.selected {
  border-color: #007bff;
  background: #e3f2fd;
}

.form-actions {
  display: flex;
  justify-content: center;
  gap: 16px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

.btn-primary,
.btn-secondary {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
  min-width: 120px;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #0056b3;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover:not(:disabled) {
  background: #5a6268;
}

.btn-primary:disabled,
.btn-secondary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

@media (max-width: 768px) {
  .discount-create-container {
    padding: 10px;
  }

  .header {
    flex-direction: column;
    gap: 16px;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .category-grid {
    grid-template-columns: 1fr;
  }

  .form-actions {
    flex-direction: column;
  }
}
</style>