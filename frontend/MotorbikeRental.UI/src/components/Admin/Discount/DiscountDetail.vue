<script setup>
import { ref, reactive, watch } from 'vue'

const props = defineProps({
  discount: {
    type: Object,
    required: true
  },
  categories: {
    type: Array,
    required: true,
    default: () => []
  }
})

const emit = defineEmits(['update', 'delete', 'back'])

const isEditing = ref(false)
const isSubmitting = ref(false)
const errors = ref({})

const form = reactive({
  name: '',
  description: '',
  categoryId: [],
  value: '',
  startDate: '',
  endDate: '',
  isActive: true
})

watch(() => props.discount, (newDiscount) => {
  if (newDiscount) {
    form.name = newDiscount.name
    form.description = newDiscount.description || ''
    form.categoryId = getCategoryIdsByNames(newDiscount.categoryNames)
    form.value = newDiscount.value
    form.startDate = formatDateForInput(newDiscount.startDate)
    form.endDate = formatDateForInput(newDiscount.endDate)
    form.isActive = newDiscount.isActive
  }
}, { immediate: true })

function getCategoryIdsByNames(categoryNames) {
  if (!categoryNames || !props.categories) return []
  return props.categories
    .filter(cat => categoryNames.includes(cat.categoryName))
    .map(cat => cat.categoryId)
}

function formatDateForInput(dateString) {
  if (!dateString) return ''
  return new Date(dateString).toISOString().split('T')[0]
}

function formatDateForDisplay(dateString) {
  if (!dateString) return ''
  return new Date(dateString).toLocaleDateString('vi-VN')
}

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

function toggleEdit() {
  isEditing.value = !isEditing.value
  if (!isEditing.value) {
    form.name = props.discount.name
    form.description = props.discount.description || ''
    form.categoryId = getCategoryIdsByNames(props.discount.categoryNames)
    form.value = props.discount.value
    form.startDate = formatDateForInput(props.discount.startDate)
    form.endDate = formatDateForInput(props.discount.endDate)
    form.isActive = props.discount.isActive
    errors.value = {}
  }
}

function handleSave() {
  if (!validateForm()) return
  
  isSubmitting.value = true
  
  const updateData = {
    discountId: props.discount.discountId,
    name: form.name,
    description: form.description,
    categoryId: form.categoryId,
    value: parseFloat(form.value),
    startDate: form.startDate + 'T00:00:00.000Z',
    endDate: form.endDate + 'T23:59:59.999Z',
    isActive: form.isActive
  }
  
  emit('update', updateData)
  
  setTimeout(() => {
    isSubmitting.value = false
    isEditing.value = false
  }, 1000)
}

function handleDelete() {
  if (confirm('Bạn có chắc chắn muốn xóa mã giảm giá này?')) {
    emit('delete', props.discount.discountId)
  }
}

function handleBack() {
  emit('back')
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
  <div class="discount-detail-container" v-if="discount">
    <!-- Header -->
    <div class="header">
      <div class="header-left">
        <h1>Chi tiết mã giảm giá</h1>
        <p>Quản lý thông tin mã giảm giá {{ discount.name }}</p>
      </div>
      <div class="header-actions">
        <button @click="handleBack" class="btn-back">← Quay lại</button>
        <button v-if="!isEditing" @click="toggleEdit" class="btn-edit">Chỉnh sửa</button>
        <button v-if="isEditing" @click="handleSave" class="btn-save" :disabled="isSubmitting">
          {{ isSubmitting ? 'Đang lưu...' : 'Lưu' }}
        </button>
        <button v-if="isEditing" @click="toggleEdit" class="btn-cancel">Hủy</button>
        <button @click="handleDelete" class="btn-delete">Xóa</button>
      </div>
    </div>

    <!-- Basic Info -->
    <div class="info-section">
      <h2>Thông tin cơ bản</h2>
      <div class="info-grid">
        <div class="info-item">
          <label>Tên mã giảm giá</label>
          <div v-if="isEditing">
            <input v-model="form.name" type="text" class="form-input" :class="{ 'error': errors.name }" />
            <span v-if="errors.name" class="error-message">{{ errors.name }}</span>
          </div>
          <span v-else class="info-value">{{ discount.name }}</span>
        </div>

        <div class="info-item">
          <label>ID mã giảm giá</label>
          <span class="info-value">#{{ discount.discountId }}</span>
        </div>

        <div class="info-item">
          <label>Giá trị giảm giá</label>
          <div v-if="isEditing">
            <input v-model="form.value" type="number" class="form-input" :class="{ 'error': errors.value }" min="1" max="100" />
            <span v-if="errors.value" class="error-message">{{ errors.value }}</span>
          </div>
          <span v-else class="info-value discount-value">{{ discount.value }}%</span>
        </div>

        <div class="info-item">
          <label>Trạng thái</label>
          <div v-if="isEditing">
            <select v-model="form.isActive" class="form-select">
              <option :value="true">Hoạt động</option>
              <option :value="false">Không hoạt động</option>
            </select>
          </div>
          <span v-else class="info-value">
            <span class="status-badge" :class="discount.isActive ? 'status-active' : 'status-inactive'">
              {{ discount.isActive ? 'Hoạt động' : 'Không hoạt động' }}
            </span>
          </span>
        </div>
      </div>
    </div>

    <!-- Date Range -->
    <div class="info-section">
      <h2>Thời gian áp dụng</h2>
      <div class="info-grid">
        <div class="info-item">
          <label>Ngày bắt đầu</label>
          <div v-if="isEditing">
            <input v-model="form.startDate" type="date" class="form-input" :class="{ 'error': errors.startDate }" />
            <span v-if="errors.startDate" class="error-message">{{ errors.startDate }}</span>
          </div>
          <span v-else class="info-value">{{ formatDateForDisplay(discount.startDate) }}</span>
        </div>

        <div class="info-item">
          <label>Ngày kết thúc</label>
          <div v-if="isEditing">
            <input v-model="form.endDate" type="date" class="form-input" :class="{ 'error': errors.endDate }" />
            <span v-if="errors.endDate" class="error-message">{{ errors.endDate }}</span>
          </div>
          <span v-else class="info-value">{{ formatDateForDisplay(discount.endDate) }}</span>
        </div>
      </div>
    </div>

    <!-- Categories -->
    <div class="info-section">
      <h2>Loại xe áp dụng</h2>
      <div v-if="isEditing">
        <div class="category-list">
          <div v-for="category in categories" :key="category.categoryId" class="category-item" @click="toggleCategory(category.categoryId)">
            <input type="checkbox" :checked="form.categoryId.includes(category.categoryId)" />
            <span>{{ category.categoryName }}</span>
          </div>
        </div>
        <span v-if="errors.categoryId" class="error-message">{{ errors.categoryId }}</span>
      </div>
      <div v-else class="category-tags">
        <span v-for="categoryName in discount.categoryNames" :key="categoryName" class="category-tag">
          {{ categoryName }}
        </span>
      </div>
    </div>

    <!-- Description -->
    <div class="info-section">
      <h2>Mô tả</h2>
      <div v-if="isEditing">
        <textarea v-model="form.description" class="form-textarea" placeholder="Nhập mô tả mã giảm giá" rows="3"></textarea>
      </div>
      <p v-else class="description">{{ discount.description || 'Không có mô tả' }}</p>
    </div>
  </div>
</template>

<style scoped>
.discount-detail-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.header-left h1 {
  margin: 0 0 5px 0;
  font-size: 24px;
  color: #333;
}

.header-left p {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.header-actions {
  display: flex;
  gap: 10px;
}

.btn-back,
.btn-edit,
.btn-save,
.btn-cancel,
.btn-delete {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
}

.btn-back {
  background: #6c757d;
  color: white;
}

.btn-back:hover {
  background: #5a6268;
}

.btn-edit {
  background: #007bff;
  color: white;
}

.btn-edit:hover {
  background: #0056b3;
}

.btn-save {
  background: #28a745;
  color: white;
}

.btn-save:hover:not(:disabled) {
  background: #218838;
}

.btn-save:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-cancel {
  background: #6c757d;
  color: white;
}

.btn-cancel:hover {
  background: #5a6268;
}

.btn-delete {
  background: #dc3545;
  color: white;
}

.btn-delete:hover {
  background: #c82333;
}

.info-section {
  margin-bottom: 30px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.info-section h2 {
  margin: 0 0 20px 0;
  font-size: 18px;
  color: #333;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
}

.info-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
}

.info-item {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.info-item label {
  font-weight: 500;
  color: #333;
  font-size: 14px;
}

.info-value {
  padding: 8px 0;
  font-size: 14px;
  color: #333;
  border-bottom: 1px solid #f0f0f0;
}

.discount-value {
  color: #28a745;
  font-weight: 600;
  font-size: 16px;
}

.form-input,
.form-select,
.form-textarea {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.form-input:focus,
.form-select:focus,
.form-textarea:focus {
  outline: none;
  border-color: #007bff;
}

.form-input.error,
.form-select.error,
.form-textarea.error {
  border-color: #dc3545;
}

.form-textarea {
  resize: vertical;
  min-height: 80px;
}

.error-message {
  font-size: 12px;
  color: #dc3545;
}

.category-list {
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

.category-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.category-tag {
  background: #e9ecef;
  color: #495057;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.status-badge {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.status-active {
  background: #d4edda;
  color: #155724;
}

.status-inactive {
  background: #f8d7da;
  color: #721c24;
}

.description {
  margin: 0;
  color: #333;
  line-height: 1.5;
}

@media (max-width: 768px) {
  .discount-detail-container {
    padding: 10px;
  }

  .header {
    flex-direction: column;
    gap: 15px;
  }

  .header-actions {
    flex-wrap: wrap;
    justify-content: center;
  }

  .info-grid {
    grid-template-columns: 1fr;
  }

  .category-list {
    grid-template-columns: 1fr;
  }
}
</style>