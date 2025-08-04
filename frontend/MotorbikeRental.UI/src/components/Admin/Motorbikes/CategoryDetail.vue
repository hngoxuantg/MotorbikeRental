<script setup>
import { ref, reactive, watch } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const props = defineProps({
  category: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['update', 'delete', 'back'])

// Form data
const form = reactive({
  categoryName: '',
  depositAmount: ''
})

// Edit mode
const isEditing = ref(false)
const errors = ref({})
const isSubmitting = ref(false)

// Watch for category changes
watch(() => props.category, (newCategory) => {
  if (newCategory) {
    form.categoryName = newCategory.categoryName
    form.depositAmount = newCategory.depositAmount
  }
}, { immediate: true })

function validateForm() {
  errors.value = {}
  
  if (!form.categoryName.trim()) {
    errors.value.categoryName = 'Tên loại xe là bắt buộc'
  }
  
  if (!form.depositAmount || form.depositAmount <= 0) {
    errors.value.depositAmount = 'Tiền cọc phải lớn hơn 0'
  }
  
  return Object.keys(errors.value).length === 0
}

function toggleEdit() {
  isEditing.value = !isEditing.value
  if (!isEditing.value) {
    // Reset form if canceling
    form.categoryName = props.category.categoryName
    form.depositAmount = props.category.depositAmount
    errors.value = {}
  }
}

function saveCategory() {
  if (!validateForm()) return
  
  isSubmitting.value = true
  emit('update', {
    categoryId: props.category.categoryId,
    categoryName: form.categoryName,
    depositAmount: parseFloat(form.depositAmount)
  })
  
  setTimeout(() => {
    isSubmitting.value = false
    isEditing.value = false
  }, 1000)
}

function deleteCategory() {
  if (confirm('Bạn có chắc chắn muốn xóa loại xe này không? Hành động này không thể hoàn tác!')) {
    emit('delete', props.category.categoryId)
  }
}

function handleBack() {
  emit('back')
}

function formatCurrency(amount) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(amount)
}
</script>

<template>
  <div class="category-detail-container" v-if="category">
    <!-- Header -->
    <div class="detail-header">
      <div class="header-left">
        <h1>Chi tiết loại xe</h1>
        <p>Thông tin loại xe {{ category.categoryName }}</p>
      </div>
      <div class="header-actions">
        <button @click="handleBack" class="btn-back">
          ← Quay lại
        </button>
        <button v-if="!isEditing" @click="toggleEdit" class="btn-edit">
          Chỉnh sửa
        </button>
        <button v-if="isEditing" @click="saveCategory" class="btn-save" :disabled="isSubmitting">
          {{ isSubmitting ? 'Đang lưu...' : 'Lưu' }}
        </button>
        <button v-if="isEditing" @click="toggleEdit" class="btn-cancel">
          Hủy
        </button>
        <button @click="deleteCategory" class="btn-delete">
          Xóa
        </button>
      </div>
    </div>

    <!-- Content -->
    <div class="detail-content">
      <div class="info-section">
        <h2>Thông tin loại xe</h2>
        <div class="info-form">
          <div class="form-group">
            <label>Tên loại xe</label>
            <input 
              v-if="isEditing"
              v-model="form.categoryName"
              type="text"
              class="form-input"
              :class="{ 'error': errors.categoryName }"
              placeholder="Nhập tên loại xe"
            />
            <span v-else class="info-value">{{ category.categoryName }}</span>
            <span v-if="errors.categoryName" class="error-message">{{ errors.categoryName }}</span>
          </div>

          <div class="form-group">
            <label>Tiền cọc</label>
            <input 
              v-if="isEditing"
              v-model="form.depositAmount"
              type="number"
              class="form-input"
              :class="{ 'error': errors.depositAmount }"
              placeholder="Nhập tiền cọc"
              min="0"
              step="1000"
            />
            <span v-else class="info-value amount">{{ formatCurrency(category.depositAmount) }}</span>
            <span v-if="errors.depositAmount" class="error-message">{{ errors.depositAmount }}</span>
          </div>

          <div class="form-group">
            <label>ID loại xe</label>
            <span class="info-value category-id">#{{ category.categoryId }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.category-detail-container {
  padding: 20px;
}

.detail-header {
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

.detail-content {
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 20px;
}

.info-section h2 {
  margin: 0 0 20px 0;
  font-size: 18px;
  color: #333;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
}

.info-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group label {
  font-weight: 500;
  color: #333;
  font-size: 14px;
}

.info-value {
  padding: 8px 0;
  color: #333;
  font-size: 14px;
  border-bottom: 1px solid #f0f0f0;
}

.info-value.amount {
  color: #28a745;
  font-weight: 600;
  font-size: 16px;
}

.info-value.category-id {
  color: #6366f1;
  font-weight: 600;
}

.form-input {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.form-input:focus {
  outline: none;
  border-color: #007bff;
}

.form-input.error {
  border-color: #dc3545;
}

.error-message {
  font-size: 12px;
  color: #dc3545;
}

@media (max-width: 768px) {
  .category-detail-container {
    padding: 10px;
  }

  .detail-header {
    flex-direction: column;
    gap: 16px;
  }

  .header-actions {
    flex-wrap: wrap;
    justify-content: center;
  }
}
</style>