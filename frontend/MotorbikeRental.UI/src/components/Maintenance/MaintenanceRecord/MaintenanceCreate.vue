<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

const emit = defineEmits(['submit'])

const motorbikeId = ref(null)
const isLoading = ref(false)
const errors = ref({})

const formData = ref({
  maintenanceDate: ''
})

const resetForm = () => {
  const today = new Date().toISOString().split('T')[0]
  formData.value = {
    maintenanceDate: today
  }
  errors.value = {}
}

const validateForm = () => {
  errors.value = {}
  
  if (!formData.value.maintenanceDate) {
    errors.value.maintenanceDate = 'Vui lòng chọn ngày bảo dưỡng'
  }
  
  const selectedDate = new Date(formData.value.maintenanceDate)
  const today = new Date()
  today.setHours(0, 0, 0, 0)
  
  if (selectedDate < today) {
    errors.value.maintenanceDate = 'Ngày bảo dưỡng không được là ngày trong quá khứ'
  }
  
  return Object.keys(errors.value).length === 0
}

const handleSubmit = async () => {
  if (!validateForm()) return
  
  isLoading.value = true
  
  try {
    // Chỉ emit date string thôi
    emit('submit', formData.value.maintenanceDate)
    
  } catch (error) {
    console.error('Error:', error)
  } finally {
    isLoading.value = false
  }
}

const goBack = () => {
  router.back()
}

onMounted(() => {
  motorbikeId.value = parseInt(route.params.motorbikeId)
  resetForm()
})
</script>

<template>
  <div class="maintenance-create-page">
    <div class="page-header">
      <div class="header-content">
        <div class="header-left">
          <button @click="goBack" class="back-btn">
            <i class="fas fa-arrow-left"></i>
            Quay lại
          </button>
          <div class="header-title">
            <h1 class="page-title">
              <i class="fas fa-tools"></i>
              Tạo phiên bảo dưỡng
            </h1>
            <p class="page-subtitle">Xe #{{ motorbikeId }}</p>
          </div>
        </div>
      </div>
    </div>

    <div class="content-section">
      <div class="form-container">
        <div class="form-card">
          <div class="card-header">
            <h2 class="card-title">
              <i class="fas fa-calendar-plus"></i>
              Thông tin bảo dưỡng
            </h2>
          </div>

          <div class="card-body">
            <form @submit.prevent="handleSubmit" class="maintenance-form">
              <div class="form-group">
                <label for="maintenanceDate" class="form-label required">
                  <i class="fas fa-calendar-alt"></i>
                  Ngày bảo dưỡng
                </label>
                <input
                  id="maintenanceDate"
                  v-model="formData.maintenanceDate"
                  type="date"
                  class="form-input"
                  :class="{ 'error': errors.maintenanceDate }"
                  required
                />
                <span v-if="errors.maintenanceDate" class="error-message">
                  {{ errors.maintenanceDate }}
                </span>
              </div>

              <div class="form-info">
                <div class="info-box">
                  <i class="fas fa-info-circle"></i>
                  <div class="info-content">
                    <h4>Lưu ý:</h4>
                    <ul>
                      <li>Ngày bảo dưỡng không được là ngày trong quá khứ</li>
                      <li>Sau khi tạo, xe sẽ được chuyển sang trạng thái bảo dưỡng</li>
                      <li>Nhân viên thực hiện sẽ được ghi nhận tự động</li>
                    </ul>
                  </div>
                </div>
              </div>

              <div class="form-actions">
                <button
                  type="button"
                  @click="goBack"
                  class="btn btn-secondary"
                  :disabled="isLoading"
                >
                  <i class="fas fa-times"></i>
                  Hủy bỏ
                </button>
                
                <button
                  type="button"
                  @click="resetForm"
                  class="btn btn-outline"
                  :disabled="isLoading"
                >
                  <i class="fas fa-undo"></i>
                  Đặt lại
                </button>

                <button
                  type="submit"
                  class="btn btn-primary"
                  :disabled="isLoading"
                >
                  <i class="fas fa-spinner fa-spin" v-if="isLoading"></i>
                  <i class="fas fa-plus" v-else></i>
                  {{ isLoading ? 'Đang tạo...' : 'Tạo phiên bảo dưỡng' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.maintenance-create-page {
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
  max-width: 1200px;
  margin: 0 auto;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 20px;
}

.back-btn {
  padding: 10px 16px;
  background: #6c757d;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.2s;
}

.back-btn:hover {
  background: #5a6268;
}

.header-title {
  flex: 1;
}

.page-title {
  margin: 0 0 5px 0;
  color: #333;
  font-size: 24px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 12px;
}

.page-title i {
  color: #3498db;
}

.page-subtitle {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.content-section {
  padding: 30px 20px;
  max-width: 800px;
  margin: 0 auto;
}

.form-container {
  display: flex;
  justify-content: center;
}

.form-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  width: 100%;
  max-width: 600px;
}

.card-header {
  background: #f8f9fa;
  padding: 20px 25px;
  border-bottom: 1px solid #e9ecef;
}

.card-title {
  margin: 0;
  color: #333;
  font-size: 18px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 10px;
}

.card-title i {
  color: #3498db;
}

.card-body {
  padding: 25px;
}

.maintenance-form {
  display: flex;
  flex-direction: column;
  gap: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-label {
  font-weight: 600;
  color: #333;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 8px;
}

.form-label.required::after {
  content: '*';
  color: #dc3545;
  margin-left: 4px;
}

.form-label i {
  color: #666;
  width: 16px;
}

.form-input {
  padding: 12px 16px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.form-input:focus {
  outline: none;
  border-color: #3498db;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.form-input.error {
  border-color: #dc3545;
}

.error-message {
  color: #dc3545;
  font-size: 12px;
  font-weight: 500;
}

.form-info {
  margin: 20px 0;
}

.info-box {
  background: #e3f2fd;
  border: 1px solid #bbdefb;
  border-radius: 6px;
  padding: 16px;
  display: flex;
  gap: 12px;
}

.info-box i {
  color: #1976d2;
  font-size: 18px;
  margin-top: 2px;
}

.info-content h4 {
  margin: 0 0 8px 0;
  color: #1976d2;
  font-size: 14px;
}

.info-content ul {
  margin: 0;
  padding-left: 16px;
  color: #333;
}

.info-content li {
  font-size: 13px;
  margin-bottom: 4px;
}

.form-actions {
  display: flex;
  gap: 12px;
  justify-content: flex-end;
  margin-top: 20px;
  padding-top: 20px;
  border-top: 1px solid #e9ecef;
}

.btn {
  padding: 12px 20px;
  border-radius: 6px;
  font-weight: 600;
  font-size: 14px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.2s;
  border: none;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background: #3498db;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #2980b9;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover:not(:disabled) {
  background: #5a6268;
}

.btn-outline {
  background: white;
  color: #6c757d;
  border: 1px solid #6c757d;
}

.btn-outline:hover:not(:disabled) {
  background: #6c757d;
  color: white;
}

@media (max-width: 768px) {
  .header-left {
    flex-direction: column;
    align-items: flex-start;
    gap: 15px;
  }
  
  .content-section {
    padding: 20px 15px;
  }
  
  .card-body {
    padding: 20px;
  }
  
  .form-actions {
    flex-direction: column;
  }
  
  .btn {
    justify-content: center;
  }
}
</style>