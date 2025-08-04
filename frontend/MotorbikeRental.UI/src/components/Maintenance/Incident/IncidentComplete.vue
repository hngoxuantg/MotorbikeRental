<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getFullPath } from '@/utils/UrlUtils'

const route = useRoute()
const router = useRouter()

const props = defineProps({
  onComplete: {
    type: Function,
    required: true
  }
})

const incidentId = ref(null)
const incident = ref(null)
const isLoading = ref(true)
const isSubmitting = ref(false)
const errors = ref({})

const formData = ref({
  notes: '',
  resolvedDate: ''
})

const incidentTypeText = {
  0: { text: 'Hư hỏng', color: '#dc3545', icon: 'fa-wrench' },
  1: { text: 'Tai nạn', color: '#ffc107', icon: 'fa-exclamation-triangle' },
  2: { text: 'Mất cắp', color: '#6f42c1', icon: 'fa-user-ninja' }
}

const severityText = {
  0: { text: 'Nhẹ', color: '#28a745' },
  1: { text: 'Trung bình', color: '#ffc107' },
  2: { text: 'Nặng', color: '#dc3545' }
}

const resetForm = () => {
  const today = new Date().toISOString().split('T')[0]
  formData.value = {
    notes: '',
    resolvedDate: today
  }
  errors.value = {}
}

const validateForm = () => {
  errors.value = {}
  
  if (!formData.value.resolvedDate) {
    errors.value.resolvedDate = 'Vui lòng chọn ngày giải quyết'
  } else {
    const resolvedDate = new Date(formData.value.resolvedDate)
    const incidentDate = new Date(incident.value.incidentDate)
    
    if (resolvedDate < incidentDate) {
      errors.value.resolvedDate = 'Ngày giải quyết không được trước ngày xảy ra sự cố'
    }
  }
  
  return Object.keys(errors.value).length === 0
}

const handleSubmit = async () => {
  if (!validateForm()) return
  
  isSubmitting.value = true
  
  try {
    const submitData = {
      notes: formData.value.notes.trim() || null,
      resolvedDate: new Date(formData.value.resolvedDate).toISOString()
    }
    
    await props.onComplete(incidentId.value, submitData)
    
  } catch (error) {
    console.error('Error:', error)
  } finally {
    isSubmitting.value = false
  }
}

const goBack = () => {
  router.back()
}

const formatDate = (dateString) => {
  if (!dateString) return 'Chưa có'
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN', { 
    day: '2-digit', 
    month: '2-digit', 
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const formatCurrency = (value) => {
  if (!value) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(value)
}

// Emit để lấy chi tiết sự cố
const emit = defineEmits(['fetchIncident'])

onMounted(() => {
  incidentId.value = parseInt(route.params.incidentId)
  emit('fetchIncident', incidentId.value)
  resetForm()
})

// Nhận dữ liệu từ parent
defineExpose({
  setIncident: (incidentData) => {
    incident.value = incidentData
    isLoading.value = false
  }
})
</script>

<template>
  <div class="incident-handle-page">
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
              Xử lý sự cố
            </h1>
            <p class="page-subtitle">Sự cố #{{ incidentId }}</p>
          </div>
        </div>
      </div>
    </div>

    <div class="content-section">
      <div v-if="isLoading" class="loading-container">
        <div class="loading-spinner">
          <i class="fas fa-spinner fa-spin"></i>
          <span>Đang tải thông tin sự cố...</span>
        </div>
      </div>

      <div v-else-if="incident" class="incident-container">
        <!-- Chi tiết sự cố -->
        <div class="incident-details-card">
          <div class="card-header">
            <h2 class="card-title">
              <i class="fas fa-info-circle"></i>
              Chi tiết sự cố
            </h2>
            <div class="incident-badges">
              <div
                class="type-badge"
                :style="{
                  color: incidentTypeText[incident.type]?.color,
                  backgroundColor: incidentTypeText[incident.type]?.color + '20'
                }"
              >
                <i :class="['fas', incidentTypeText[incident.type]?.icon]"></i>
                {{ incidentTypeText[incident.type]?.text }}
              </div>
              <div
                class="severity-badge"
                :style="{
                  color: severityText[incident.severity]?.color,
                  backgroundColor: severityText[incident.severity]?.color + '20'
                }"
              >
                <i class="fas fa-exclamation"></i>
                {{ severityText[incident.severity]?.text }}
              </div>
            </div>
          </div>

          <div class="card-body">
            <div class="details-grid">
              <!-- Thông tin khách hàng và xe -->
              <div class="detail-section">
                <h3 class="section-title">
                  <i class="fas fa-user"></i>
                  Thông tin khách hàng & xe
                </h3>
                <div class="detail-items">
                  <div class="detail-item">
                    <span class="label">Khách hàng:</span>
                    <span class="value">{{ incident.customerName }}</span>
                  </div>
                  <div class="detail-item">
                    <span class="label">Xe máy:</span>
                    <span class="value">{{ incident.motorbikeName }}</span>
                  </div>
                  <div class="detail-item">
                    <span class="label">Mã hợp đồng:</span>
                    <span class="value">#{{ incident.contractId }}</span>
                  </div>
                </div>
                
                <div class="motorbike-image" v-if="incident.motorbikeImage">
                  <img :src="getFullPath(incident.motorbikeImage)" :alt="incident.motorbikeName" />
                </div>
              </div>

              <!-- Thông tin sự cố -->
              <div class="detail-section">
                <h3 class="section-title">
                  <i class="fas fa-calendar-alt"></i>
                  Thông tin sự cố
                </h3>
                <div class="detail-items">
                  <div class="detail-item">
                    <span class="label">Ngày xảy ra:</span>
                    <span class="value">{{ formatDate(incident.incidentDate) }}</span>
                  </div>
                  <div class="detail-item">
                    <span class="label">Người báo cáo:</span>
                    <span class="value">{{ incident.reportedByEmployeeName }}</span>
                  </div>
                  <div class="detail-item">
                    <span class="label">Chi phí thiệt hại:</span>
                    <span class="value cost">{{ formatCurrency(incident.damageCost) }}</span>
                  </div>
                  <div class="detail-item">
                    <span class="label">Ngày tạo:</span>
                    <span class="value">{{ formatDate(incident.createdAt) }}</span>
                  </div>
                </div>

                <div v-if="incident.description" class="description-section">
                  <h4>Mô tả:</h4>
                  <p>{{ incident.description }}</p>
                </div>
              </div>
            </div>

            <!-- Hình ảnh sự cố -->
            <div class="images-section" v-if="incident.images && incident.images.length > 0">
              <h3 class="section-title">
                <i class="fas fa-images"></i>
                Hình ảnh sự cố ({{ incident.images.length }})
              </h3>
              <div class="images-grid">
                <div
                  v-for="(image, index) in incident.images"
                  :key="index"
                  class="image-item"
                >
                  <img :src="getFullPath(image)" :alt="`Ảnh sự cố ${index + 1}`" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Form xử lý -->
        <div class="handle-form-card">
          <div class="card-header">
            <h2 class="card-title">
              <i class="fas fa-clipboard-check"></i>
              Hoàn tất xử lý sự cố
            </h2>
          </div>

          <div class="card-body">
            <form @submit.prevent="handleSubmit" class="handle-form">
              <div class="form-group">
                <label for="resolvedDate" class="form-label required">
                  <i class="fas fa-calendar-check"></i>
                  Ngày giải quyết
                </label>
                <input
                  id="resolvedDate"
                  v-model="formData.resolvedDate"
                  type="date"
                  class="form-input"
                  :class="{ 'error': errors.resolvedDate }"
                  required
                />
                <span v-if="errors.resolvedDate" class="error-message">
                  {{ errors.resolvedDate }}
                </span>
              </div>

              <div class="form-group">
                <label for="notes" class="form-label">
                  <i class="fas fa-sticky-note"></i>
                  Ghi chú xử lý
                </label>
                <textarea
                  id="notes"
                  v-model="formData.notes"
                  class="form-textarea"
                  rows="4"
                  placeholder="Nhập ghi chú về quá trình xử lý sự cố (tùy chọn)..."
                ></textarea>
              </div>

              <div class="form-info">
                <div class="info-box">
                  <i class="fas fa-info-circle"></i>
                  <div class="info-content">
                    <h4>Lưu ý:</h4>
                    <ul>
                      <li>Ngày giải quyết không được trước ngày xảy ra sự cố</li>
                      <li>Ghi chú giúp theo dõi quá trình xử lý</li>
                      <li>Sau khi hoàn tất, sự cố sẽ được đánh dấu đã giải quyết</li>
                    </ul>
                  </div>
                </div>
              </div>

              <div class="form-actions">
                <button
                  type="button"
                  @click="goBack"
                  class="btn btn-secondary"
                  :disabled="isSubmitting"
                >
                  <i class="fas fa-times"></i>
                  Hủy bỏ
                </button>
                
                <button
                  type="button"
                  @click="resetForm"
                  class="btn btn-outline"
                  :disabled="isSubmitting"
                >
                  <i class="fas fa-undo"></i>
                  Đặt lại
                </button>

                <button
                  type="submit"
                  class="btn btn-success"
                  :disabled="isSubmitting"
                >
                  <i class="fas fa-spinner fa-spin" v-if="isSubmitting"></i>
                  <i class="fas fa-check" v-else></i>
                  {{ isSubmitting ? 'Đang xử lý...' : 'Hoàn tất xử lý' }}
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
.incident-handle-page {
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
  color: #e74c3c;
}

.page-subtitle {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.content-section {
  padding: 30px 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.loading-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 400px;
}

.loading-spinner {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 15px;
  color: #666;
}

.loading-spinner i {
  font-size: 40px;
  color: #e74c3c;
}

.incident-container {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 30px;
}

.incident-details-card, .handle-form-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.card-header {
  background: #f8f9fa;
  padding: 20px 25px;
  border-bottom: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 15px;
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
  color: #e74c3c;
}

.incident-badges {
  display: flex;
  gap: 10px;
}

.type-badge, .severity-badge {
  padding: 6px 12px;
  border-radius: 15px;
  font-size: 12px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 5px;
}

.card-body {
  padding: 25px;
}

.details-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 30px;
  margin-bottom: 30px;
}

.detail-section {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.section-title {
  margin: 0;
  color: #333;
  font-size: 16px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 8px;
  padding-bottom: 10px;
  border-bottom: 2px solid #e9ecef;
}

.section-title i {
  color: #666;
}

.detail-items {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.detail-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
}

.detail-item .label {
  color: #666;
  font-weight: 500;
  min-width: 120px;
}

.detail-item .value {
  color: #333;
  font-weight: 600;
  text-align: right;
}

.value.cost {
  color: #e74c3c;
}

.motorbike-image {
  margin-top: 15px;
}

.motorbike-image img {
  width: 100%;
  max-width: 200px;
  height: 120px;
  object-fit: cover;
  border-radius: 6px;
  border: 1px solid #ddd;
}

.description-section {
  margin-top: 15px;
  padding: 15px;
  background: #f8f9fa;
  border-radius: 6px;
}

.description-section h4 {
  margin: 0 0 8px 0;
  color: #333;
  font-size: 14px;
}

.description-section p {
  margin: 0;
  color: #666;
  line-height: 1.5;
}

.images-section {
  border-top: 1px solid #e9ecef;
  padding-top: 25px;
}

.images-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
  gap: 15px;
}

.image-item img {
  width: 100%;
  height: 120px;
  object-fit: cover;
  border-radius: 6px;
  border: 1px solid #ddd;
  transition: transform 0.2s;
  cursor: pointer;
}

.image-item img:hover {
  transform: scale(1.05);
}

.handle-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
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

.form-input, .form-textarea {
  padding: 12px 16px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.2s, box-shadow 0.2s;
  font-family: inherit;
}

.form-input:focus, .form-textarea:focus {
  outline: none;
  border-color: #e74c3c;
  box-shadow: 0 0 0 3px rgba(231, 76, 60, 0.1);
}

.form-input.error, .form-textarea.error {
  border-color: #dc3545;
}

.form-textarea {
  resize: vertical;
  min-height: 100px;
}

.error-message {
  color: #dc3545;
  font-size: 12px;
  font-weight: 500;
}

.form-info {
  margin: 15px 0;
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

.btn-success {
  background: #28a745;
  color: white;
}

.btn-success:hover:not(:disabled) {
  background: #218838;
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

@media (max-width: 992px) {
  .incident-container {
    grid-template-columns: 1fr;
    gap: 20px;
  }
  
  .details-grid {
    grid-template-columns: 1fr;
    gap: 20px;
  }
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
  
  .images-grid {
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 10px;
  }
}
</style>