<script setup>
import { ref, reactive } from 'vue'

const props = defineProps({
  isLoading: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['submit'])

const form = reactive({
  incidentDate: '',
  type: 0,
  severity: 1,
  description: '',
  damageCost: 0,
  resolvedDate: null,
  images: []
})

const errors = ref({})
const fileInput = ref(null)

const incidentTypes = [
  { value: 0, label: 'Tai nạn giao thông' },
  { value: 1, label: 'Hư hỏng kỹ thuật' },
  { value: 2, label: 'Can thiệp của cơ quan chức năng' },
  { value: 3, label: 'Trộm' },
  { value: 4, label: 'Khác' },
]

const severityLevels = [
  { value: 1, label: 'Nhẹ' },
  { value: 2, label: 'Trung bình' },
  { value: 3, label: 'Cao' },
  { value: 4, label: 'Nghiêm trọng' },
]

function fillCurrentDateTime() {
  const now = new Date()
  const year = now.getFullYear()
  const month = String(now.getMonth() + 1).padStart(2, '0')
  const day = String(now.getDate()).padStart(2, '0')
  const hours = String(now.getHours()).padStart(2, '0')
  const minutes = String(now.getMinutes()).padStart(2, '0')

  form.incidentDate = `${year}-${month}-${day}T${hours}:${minutes}`
}

function convertToUTC(localDateTimeString) {
  if (!localDateTimeString) return null
  const localDate = new Date(localDateTimeString)
  return localDate.toISOString()
}

function createImagePreview(file) {
  if (!file) return ''
  
  if (window.URL && window.URL.createObjectURL) {
    return window.URL.createObjectURL(file)
  } else if (window.webkitURL && window.webkitURL.createObjectURL) {
    return window.webkitURL.createObjectURL(file)
  } else {
    return ''
  }
}

function handleFileChange(event) {
  const files = Array.from(event.target.files)
  if (files.length > 5) {
    alert('Chỉ được chọn tối đa 5 ảnh')
    event.target.value = ''
    return
  }
  
  for (let file of files) {
    if (file.size > 5 * 1024 * 1024) {
      alert('Kích thước ảnh không được vượt quá 5MB')
      event.target.value = ''
      return
    }
    
    if (!file.type.startsWith('image/')) {
      alert('Chỉ được chọn file ảnh')
      event.target.value = ''
      return
    }
  }
  form.images = [...files] 
}

function removeImage(index) {
  const newImages = Array.from(form.images)
  newImages.splice(index, 1)
  form.images = newImages
  
  if (fileInput.value) {
    fileInput.value.value = ''
  }
}

function handleSubmit() {
  errors.value = {}

  if (!form.incidentDate) {
    errors.value.incidentDate = 'Vui lòng chọn ngày xảy ra sự cố'
    return
  }

  if (form.damageCost < 0) {
    errors.value.damageCost = 'Chi phí thiệt hại không thể âm'
    return
  }

  const submitData = {
    incidentDate: convertToUTC(form.incidentDate),
    type: form.type,
    severity: form.severity,
    description: form.description.trim() || null,
    damageCost: form.damageCost,
    resolvedDate: form.resolvedDate ? convertToUTC(form.resolvedDate) : null,
    images: form.images
  }

  emit('submit', submitData)
}

function formatPrice(price) {
  if (!price) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN').format(price) + ' VNĐ'
}
</script>

<template>
  <div class="create-incident-page">
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner">
        <i class="fas fa-spinner fa-spin"></i>
        <span>Đang xử lý...</span>
      </div>
    </div>

    <div v-else class="page-content">
      <div class="page-header">
        <h2 class="page-title">
          <i class="fas fa-exclamation-triangle"></i>
          Thêm sự cố mới
        </h2>
      </div>

      <div class="form-container">
        <div class="form-panel">
          <div class="panel-header">
            <i class="fas fa-clipboard-list"></i>
            <span>Thông tin sự cố</span>
          </div>
          <div class="panel-body">
            <form @submit.prevent="handleSubmit" class="incident-form">
              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-calendar-exclamation"></i>
                  Ngày xảy ra sự cố
                </label>
                <div class="datetime-input-group">
                  <input
                    v-model="form.incidentDate"
                    type="datetime-local"
                    class="form-input"
                    :class="{ error: errors.incidentDate }"
                    required
                  />
                  <button type="button" @click="fillCurrentDateTime" class="btn-now">
                    <i class="fas fa-clock"></i>
                    Bây giờ
                  </button>
                </div>
                <span v-if="errors.incidentDate" class="error-message">
                  {{ errors.incidentDate }}
                </span>
              </div>

              <div class="form-row">
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-tags"></i>
                    Loại sự cố
                  </label>
                  <select v-model="form.type" class="form-input" required>
                    <option v-for="type in incidentTypes" :key="type.value" :value="type.value">
                      {{ type.label }}
                    </option>
                  </select>
                </div>

                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-thermometer-half"></i>
                    Mức độ nghiêm trọng
                  </label>
                  <select v-model="form.severity" class="form-input" required>
                    <option v-for="level in severityLevels" :key="level.value" :value="level.value">
                      {{ level.label }}
                    </option>
                  </select>
                </div>
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-camera"></i>
                  Ảnh minh chứng
                </label>
                <div class="image-upload-section">
                  <input
                    ref="fileInput"
                    type="file"
                    multiple
                    accept="image/*"
                    @change="handleFileChange"
                    class="file-input"
                    id="imageUpload"
                  />
                  <label for="imageUpload" class="upload-button">
                    <i class="fas fa-plus"></i>
                    Chọn ảnh (tối đa 5 ảnh)
                  </label>
                  <small class="form-hint">
                    <i class="fas fa-info-circle"></i>
                    Chấp nhận: JPG, PNG, GIF. Tối đa 5MB/ảnh
                  </small>
                </div>

                <div v-if="form.images.length > 0" class="image-preview-container">
                  <div class="preview-label">
                    <i class="fas fa-images"></i>
                    Ảnh đã chọn ({{ form.images.length }}/5):
                  </div>
                  <div class="image-preview-grid">
                    <div
                      v-for="(image, index) in form.images"
                      :key="index"
                      class="image-preview-item"
                    >
                      <img 
                        v-if="createImagePreview(image)"
                        :src="createImagePreview(image)" 
                        :alt="`Preview ${index + 1}`" 
                      />
                      <div v-else class="image-placeholder">
                        <i class="fas fa-image"></i>
                        <span>{{ image.name }}</span>
                      </div>
                      <div class="image-info">
                        <span class="image-name">{{ image.name }}</span>
                        <span class="image-size">{{ (image.size / 1024 / 1024).toFixed(2) }}MB</span>
                      </div>
                      <button
                        type="button"
                        @click="removeImage(index)"
                        class="remove-image-btn"
                        title="Xóa ảnh"
                      >
                        <i class="fas fa-times"></i>
                      </button>
                    </div>
                  </div>
                </div>
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-align-left"></i>
                  Mô tả chi tiết sự cố (tùy chọn)
                </label>
                <textarea
                  v-model="form.description"
                  class="form-input form-textarea"
                  rows="3"
                  placeholder="Mô tả chi tiết về sự cố (không bắt buộc)..."
                ></textarea>
                <small class="form-hint">
                  <i class="fas fa-info-circle"></i>
                  Có thể để trống nếu ảnh đã đủ thông tin
                </small>
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-money-bill-wave"></i>
                  Chi phí thiệt hại (VNĐ)
                </label>
                <input
                  v-model.number="form.damageCost"
                  type="number"
                  class="form-input amount-input"
                  :class="{ error: errors.damageCost }"
                  placeholder="Nhập chi phí thiệt hại..."
                  min="0"
                  step="1000"
                />
                <small v-if="form.damageCost > 0" class="amount-preview">
                  <i class="fas fa-info-circle"></i>
                  {{ formatPrice(form.damageCost) }}
                </small>
                <span v-if="errors.damageCost" class="error-message">
                  {{ errors.damageCost }}
                </span>
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="fas fa-calendar-check"></i>
                  Ngày giải quyết (tùy chọn)
                </label>
                <input v-model="form.resolvedDate" type="datetime-local" class="form-input" />
                <small class="form-hint">
                  <i class="fas fa-info-circle"></i>
                  Để trống nếu chưa giải quyết
                </small>
              </div>

              <div class="form-actions">
                <button type="submit" :disabled="isLoading" class="submit-button">
                  <i class="fas fa-save"></i>
                  {{ isLoading ? 'Đang xử lý...' : 'Tạo sự cố' }}
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
.create-incident-page {
  background: #f5f5f5;
  min-height: 100vh;
  font-family: Arial, sans-serif;
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
  color: #ffc107;
}

.page-header {
  background: #fff;
  border-bottom: 1px solid #ddd;
  padding: 20px;
  text-align: center;
}

.page-title {
  margin: 0;
  color: #333;
  font-size: 22px;
  font-weight: bold;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
}

.page-title i {
  color: #ffc107;
}

.page-content {
  padding: 20px;
}

.form-container {
  max-width: 700px;
  margin: 0 auto;
}

.form-panel {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
}

.panel-header {
  background: #f8f9fa;
  border-bottom: 1px solid #ddd;
  padding: 15px 20px;
  font-weight: bold;
  color: #333;
  display: flex;
  align-items: center;
  gap: 8px;
  border-radius: 8px 8px 0 0;
}

.panel-header i {
  color: #ffc107;
}

.panel-body {
  padding: 20px;
}

.incident-form {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.form-label {
  font-weight: 600;
  color: #333;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 6px;
}

.form-label i {
  color: #ffc107;
  width: 14px;
}

.datetime-input-group {
  display: flex;
  gap: 8px;
}

.form-input {
  padding: 10px 12px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
  background: #fff;
  flex: 1;
}

.form-input:focus {
  outline: none;
  border-color: #ffc107;
}

.form-input.error {
  border-color: #dc3545;
}

.form-textarea {
  resize: vertical;
  min-height: 80px;
}

.amount-input {
  font-weight: 600;
  color: #007bff;
}

.btn-now {
  padding: 10px 14px;
  background: #6c757d;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 13px;
  white-space: nowrap;
}

.btn-now:hover {
  background: #5a6268;
}

.image-upload-section {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.file-input {
  display: none;
}

.upload-button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 12px 20px;
  border: 2px dashed #ccc;
  border-radius: 4px;
  background: #f8f9fa;
  color: #666;
  cursor: pointer;
  transition: all 0.2s;
  font-weight: 600;
}

.upload-button:hover {
  border-color: #ffc107;
  background: #fff3cd;
  color: #856404;
}

.image-preview-container {
  margin-top: 10px;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background: #f8f9fa;
}

.preview-label {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
  font-weight: 600;
  color: #333;
  font-size: 13px;
}

.preview-label i {
  color: #ffc107;
}

.image-preview-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 1fr));
  gap: 8px;
}

.image-preview-item {
  position: relative;
  border: 1px solid #ddd;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
}

.image-preview-item img {
  width: 100%;
  height: 60px;
  object-fit: cover;
  display: block;
}

.image-placeholder {
  width: 100%;
  height: 60px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: #f8f9fa;
  color: #666;
  font-size: 12px;
  text-align: center;
  padding: 8px;
}

.image-placeholder i {
  font-size: 16px;
  margin-bottom: 4px;
}

.image-placeholder span {
  font-size: 8px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  width: 100%;
}

.image-info {
  padding: 6px;
  display: flex;
  flex-direction: column;
  gap: 1px;
}

.image-name {
  font-size: 10px;
  color: #333;
  font-weight: 600;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.image-size {
  font-size: 9px;
  color: #666;
}

.remove-image-btn {
  position: absolute;
  top: 3px;
  right: 3px;
  width: 18px;
  height: 18px;
  border: none;
  border-radius: 50%;
  background: rgba(220, 53, 69, 0.9);
  color: white;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 10px;
}

.remove-image-btn:hover {
  background: #dc3545;
}

.form-hint, .amount-preview {
  color: #666;
  font-size: 12px;
  display: flex;
  align-items: center;
  gap: 4px;
  margin-top: 2px;
}

.amount-preview {
  color: #007bff;
  font-weight: 600;
}

.error-message {
  color: #dc3545;
  font-size: 12px;
  margin-top: 2px;
}

.form-actions {
  margin-top: 20px;
  padding-top: 15px;
  border-top: 1px solid #eee;
  display: flex;
  justify-content: center;
}

.submit-button {
  background: #ffc107;
  color: #212529;
  border: none;
  padding: 12px 25px;
  border-radius: 4px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  min-width: 180px;
  justify-content: center;
}

.submit-button:hover:not(:disabled) {
  background: #e0a800;
}

.submit-button:disabled {
  background: #6c757d;
  cursor: not-allowed;
}

@media (max-width: 768px) {
  .page-content {
    padding: 15px;
  }
  
  .panel-body {
    padding: 15px;
  }
  
  .form-row {
    grid-template-columns: 1fr;
    gap: 12px;
  }
  
  .datetime-input-group {
    flex-direction: column;
  }
  
  .submit-button {
    width: 100%;
  }
  
  .image-preview-grid {
    grid-template-columns: repeat(auto-fill, minmax(80px, 1fr));
    gap: 6px;
  }
  
  .image-preview-item img {
    height: 50px;
  }
  
  .image-placeholder {
    height: 50px;
  }
}
</style>