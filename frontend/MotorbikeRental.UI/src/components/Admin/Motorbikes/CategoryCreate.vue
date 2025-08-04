<script setup>
import { ref, reactive } from 'vue'

const emit = defineEmits(['submit', 'back'])

// Form data
const form = reactive({
  categoryName: '',
  depositAmount: ''
})

// Validation
const errors = ref({})
const isSubmitting = ref(false)

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

function handleSubmit() {
  if (!validateForm()) return
  
  isSubmitting.value = true
  emit('submit', {
    categoryName: form.categoryName,
    depositAmount: parseFloat(form.depositAmount)
  })
  
  setTimeout(() => {
    isSubmitting.value = false
  }, 1000)
}

function resetForm() {
  form.categoryName = ''
  form.depositAmount = ''
  errors.value = {}
}

function handleBack() {
  emit('back')
}
</script>

<template>
  <div class="category-create-container">
    <!-- Header -->
    <div class="form-header">
      <h1>Thêm loại xe mới</h1>
      <button @click="handleBack" class="btn-back">
        ← Quay lại
      </button>
    </div>

    <!-- Form -->
    <div class="form-content">
      <form @submit.prevent="handleSubmit" class="category-form">
        <div class="form-group">
          <label for="categoryName">
            Tên loại xe <span class="required">*</span>
          </label>
          <input
            id="categoryName"
            v-model="form.categoryName"
            type="text"
            class="form-input"
            :class="{ 'error': errors.categoryName }"
            placeholder="Ví dụ: Xe số, Xe ga, Xe côn..."
          />
          <span v-if="errors.categoryName" class="error-message">{{ errors.categoryName }}</span>
        </div>

        <div class="form-group">
          <label for="depositAmount">
            Tiền cọc (VND) <span class="required">*</span>
          </label>
          <input
            id="depositAmount"
            v-model="form.depositAmount"
            type="number"
            class="form-input"
            :class="{ 'error': errors.depositAmount }"
            placeholder="Ví dụ: 500000"
            min="0"
            step="1000"
          />
          <span v-if="errors.depositAmount" class="error-message">{{ errors.depositAmount }}</span>
        </div>

        <div class="form-actions">
          <button
            type="button"
            @click="resetForm"
            class="btn-secondary"
            :disabled="isSubmitting"
          >
            Làm mới
          </button>
          <button
            type="submit"
            class="btn-primary"
            :disabled="isSubmitting"
          >
            {{ isSubmitting ? 'Đang thêm...' : 'Thêm loại xe' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.category-create-container {
  padding: 20px;
}

.form-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.form-header h1 {
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

.form-content {
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 20px;
}

.category-form {
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
  font-size: 14px;
  font-weight: 500;
  color: #333;
}

.required {
  color: #dc3545;
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
  font-size: 14px;
  cursor: pointer;
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
  .category-create-container {
    padding: 10px;
  }

  .form-header {
    flex-direction: column;
    gap: 16px;
  }

  .form-actions {
    flex-direction: column;
  }

  .btn-primary,
  .btn-secondary {
    width: 100%;
  }
}
</style>