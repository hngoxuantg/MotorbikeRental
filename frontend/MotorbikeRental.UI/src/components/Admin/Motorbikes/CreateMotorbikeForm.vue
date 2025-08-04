<script setup>
import { ref } from 'vue'

const props = defineProps({
  form: Object,
  categories: { type: Array, default: () => [] },
  isLoading: { type: Boolean, default: false },
})

const emit = defineEmits(['submit'])

const previewImage = ref(null)
const fileInputRef = ref(null)

function onSubmit() {
  emit('submit', props.form)
}

function onFileChange(event) {
  const file = event.target.files[0]
  if (file) {
    props.form.FormFile = file
    // Tạo preview ảnh
    const reader = new FileReader()
    reader.onload = (e) => (previewImage.value = e.target.result)
    reader.readAsDataURL(file)
  }
}

function removeImage() {
  props.form.FormFile = null
  previewImage.value = null
  if (fileInputRef.value) {
    fileInputRef.value.value = ''
  }
}

function triggerFileInput() {
  fileInputRef.value?.click()
}



const conditionOptions = [
  { value: 0, label: 'Mới' },
  { value: 1, label: 'Như mới' },
  { value: 2, label: 'Tốt' },
  { value: 3, label: 'Khá' },
  { value: 4, label: 'Trung bình' },
]
</script>

<template>
  <div class="create-motorbike-container">
    <!-- Header -->
    <div class="page-header">
      <h1>Thêm xe máy mới</h1>
      <p>Điền thông tin chi tiết về xe máy</p>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form @submit.prevent="onSubmit" class="motorbike-form">
        <!-- Basic Information -->
        <div class="form-section">
          <h2>Thông tin cơ bản</h2>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">
                Tên xe <span class="required">*</span>
              </label>
              <input
                v-model="props.form.MotorbikeName"
                type="text"
                class="form-input"
                placeholder="Nhập tên xe máy"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Loại xe <span class="required">*</span>
              </label>
              <select v-model="props.form.CategoryId" class="form-select" required>
                <option value="">Chọn loại xe</option>
                <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
                  {{ cat.categoryName }}
                </option>
              </select>
            </div>

            <div class="form-group">
              <label class="form-label">
                Thương hiệu <span class="required">*</span>
              </label>
              <input
                v-model="props.form.Brand"
                type="text"
                class="form-input"
                placeholder="Honda, Yamaha, SYM..."
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Năm sản xuất <span class="required">*</span>
              </label>
              <input
                v-model="props.form.Year"
                type="number"
                class="form-input"
                :min="1990"
                :max="new Date().getFullYear()"
                placeholder="2023"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Màu sắc</label>
              <input
                v-model="props.form.Color"
                type="text"
                class="form-input"
                placeholder="Đen, Trắng, Đỏ..."
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Dung tích động cơ (cc) <span class="required">*</span>
              </label>
              <input
                v-model="props.form.EngineCapacity"
                type="number"
                class="form-input"
                placeholder="125"
                min="50"
                required
              />
            </div>
          </div>
        </div>

        <!-- Technical Information -->
        <div class="form-section">
          <h2>Thông tin kỹ thuật</h2>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">
                Biển số xe <span class="required">*</span>
              </label>
              <input
                v-model="props.form.LicensePlate"
                type="text"
                class="form-input"
                placeholder="30A-12345"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Số khung <span class="required">*</span>
              </label>
              <input
                v-model="props.form.ChassisNumber"
                type="text"
                class="form-input"
                placeholder="Nhập số khung xe"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Số máy <span class="required">*</span>
              </label>
              <input
                v-model="props.form.EngineNumber"
                type="text"
                class="form-input"
                placeholder="Nhập số máy"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Số km đã đi</label>
              <input
                v-model="props.form.Mileage"
                type="number"
                class="form-input"
                placeholder="0"
                min="0"
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Tình trạng xe <span class="required">*</span>
              </label>
              <select v-model="props.form.MotorbikeConditionStatus" class="form-select" required>
                <option value="">Chọn tình trạng</option>
                <option
                  v-for="condition in conditionOptions"
                  :key="condition.value"
                  :value="condition.value"
                >
                  {{ condition.label }}
                </option>
              </select>
            </div>
          </div>
        </div>

        <!-- Pricing Information -->
        <div class="form-section">
          <h2>Thông tin giá thuê</h2>
          <div class="form-grid pricing-grid">
            <div class="form-group">
              <label class="form-label">
                Giá thuê theo giờ (VNĐ) <span class="required">*</span>
              </label>
              <input
                v-model="props.form.HourlyRate"
                type="number"
                class="form-input"
                placeholder="50000"
                min="0"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Giá thuê theo ngày (VNĐ) <span class="required">*</span>
              </label>
              <input
                v-model="props.form.DailyRate"
                type="number"
                class="form-input"
                placeholder="300000"
                min="0"
                required
              />
            </div>
          </div>
        </div>

        <!-- Description & Image -->
        <div class="form-section">
          <h2>Mô tả & Hình ảnh</h2>
          
          <div class="form-group">
            <label class="form-label">Mô tả chi tiết</label>
            <textarea
              v-model="props.form.Description"
              class="form-textarea"
              placeholder="Mô tả thêm về xe: tình trạng, đặc điểm nổi bật, lưu ý..."
              rows="4"
            ></textarea>
          </div>

          <!-- Image Upload -->
          <div class="form-group">
            <label class="form-label">Hình ảnh xe</label>
            <div class="image-upload-section">
              <div v-if="!previewImage" class="upload-area">
                <div class="upload-content">
                  <p>Chọn hình ảnh xe máy</p>
                  <button type="button" @click="triggerFileInput" class="upload-btn">
                    Chọn ảnh
                  </button>
                </div>
                <input 
                  ref="fileInputRef"
                  type="file" 
                  accept="image/*" 
                  @change="onFileChange" 
                  class="file-input" 
                  hidden
                />
              </div>

              <div v-else class="image-preview">
                <img :src="previewImage" alt="Preview" class="preview-img" />
                <button type="button" @click="removeImage" class="remove-btn">
                  Xóa ảnh
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Form Actions -->
        <div class="form-actions">
          <button type="button" class="btn btn-secondary">
            Hủy bỏ
          </button>
          <button type="submit" class="btn btn-primary" :disabled="isLoading">
            {{ isLoading ? 'Đang tạo...' : 'Tạo xe máy' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.create-motorbike-container {
  padding: 20px;
  background: #f8f9fa;
  min-height: 100vh;
}

.page-header {
  text-align: center;
  margin-bottom: 24px;
  padding: 24px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.page-header h1 {
  margin: 0 0 8px 0;
  font-size: 24px;
  font-weight: 600;
  color: #333;
}

.page-header p {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.form-container {
  max-width: 800px;
  margin: 0 auto;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
  overflow: hidden;
}

.motorbike-form {
  padding: 0;
}

.form-section {
  padding: 24px;
  border-bottom: 1px solid #eee;
}

.form-section:last-child {
  border-bottom: none;
}

.form-section h2 {
  margin: 0 0 20px 0;
  font-size: 18px;
  font-weight: 600;
  color: #333;
  padding-bottom: 8px;
  border-bottom: 1px solid #eee;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
}

.pricing-grid {
  grid-template-columns: repeat(2, 1fr);
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-label {
  font-weight: 500;
  color: #333;
  font-size: 14px;
}

.required {
  color: #dc3545;
}

.form-input,
.form-select,
.form-textarea {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  background: white;
}

.form-input:focus,
.form-select:focus,
.form-textarea:focus {
  outline: none;
  border-color: #007bff;
}

.form-textarea {
  resize: vertical;
  font-family: inherit;
  grid-column: 1 / -1;
}

.image-upload-section {
  grid-column: 1 / -1;
}

.upload-area {
  border: 2px dashed #ddd;
  border-radius: 8px;
  padding: 40px 20px;
  text-align: center;
  background: #f8f9fa;
}

.upload-content p {
  margin: 0 0 16px 0;
  color: #666;
  font-size: 14px;
}

.upload-btn {
  background: #007bff;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.upload-btn:hover {
  background: #0056b3;
}

.image-preview {
  position: relative;
  display: inline-block;
  border: 1px solid #ddd;
  border-radius: 8px;
  overflow: hidden;
}

.preview-img {
  width: 300px;
  height: 200px;
  object-fit: cover;
  display: block;
}

.remove-btn {
  position: absolute;
  top: 8px;
  right: 8px;
  background: #dc3545;
  color: white;
  border: none;
  padding: 4px 8px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 12px;
}

.remove-btn:hover {
  background: #c82333;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  padding: 24px;
  background: #f8f9fa;
  border-top: 1px solid #eee;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  font-weight: 500;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #0056b3;
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #5a6268;
}

/* Responsive */
@media (max-width: 768px) {
  .create-motorbike-container {
    padding: 10px;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .pricing-grid {
    grid-template-columns: 1fr;
  }

  .form-actions {
    flex-direction: column-reverse;
  }

  .preview-img {
    width: 100%;
    max-width: 300px;
  }
}
</style>