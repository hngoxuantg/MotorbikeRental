<script setup>
import { ref, watch } from 'vue'
import { useRouter } from 'vue-router'

const props = defineProps({
  form: { type: Object, required: true },
  roles: { type: Array, default: () => [] },
  isLoading: { type: Boolean, default: false }
})
const emit = defineEmits(['submit', 'cancel'])
const router = useRouter()

const previewImage = ref(null)
const fileInputRef = ref(null)

watch(() => props.form.FormFile, (file) => {
  if (file) {
    const reader = new FileReader()
    reader.onload = (e) => {
      previewImage.value = e.target.result
    }
    reader.readAsDataURL(file)
  } else {
    previewImage.value = null
  }
})

function onFileChange(event) {
  const file = event.target.files[0]
  if (file) {
    props.form.FormFile = file
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

function onSubmit() {
  emit('submit', props.form)
}

function onCancel() {
  router.push('/admin/employees')
}

const statusOptions = {
  0: 'Đang làm',
  1: 'Xin nghỉ',
  2: 'Đã nghỉ',
}
</script>

<template>
  <div class="employee-create-container">
    <!-- Header -->
    <div class="header">
      <div class="breadcrumb">
        <span class="breadcrumb-item" @click="router.push('/admin/employees')">
          Quản lý nhân viên
        </span>
        <span class="breadcrumb-separator">></span>
        <span class="breadcrumb-current">Thêm nhân viên mới</span>
      </div>
      <h1>Thêm nhân viên mới</h1>
      <p>Điền thông tin chi tiết để tạo nhân viên mới</p>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form class="employee-form" @submit.prevent="onSubmit">
        <!-- Avatar Section -->
        <div class="avatar-section">
          <h2>Ảnh đại diện</h2>
          <div class="avatar-content">
            <div class="avatar-preview">
              <img
                v-if="previewImage"
                :src="previewImage"
                alt="Preview"
                class="avatar-img"
              />
              <img
                v-else-if="form.Avatar"
                :src="form.Avatar"
                alt="Current Avatar"
                class="avatar-img"
              />
              <div v-else class="avatar-placeholder">
                <span>Chưa có ảnh</span>
              </div>
            </div>
            <div class="avatar-actions">
              <button type="button" class="btn btn-upload" @click="triggerFileInput">
                Chọn ảnh
              </button>
              <button 
                v-if="previewImage || form.FormFile" 
                type="button" 
                class="btn btn-remove" 
                @click="removeImage"
              >
                Xóa ảnh
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
        </div>

        <!-- Personal Info -->
        <div class="form-section">
          <h2>Thông tin cá nhân</h2>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">
                Họ và tên <span class="required">*</span>
              </label>
              <input 
                v-model="form.FullName" 
                type="text" 
                class="form-input"
                placeholder="Nhập họ và tên đầy đủ"
                required 
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Ngày sinh <span class="required">*</span>
              </label>
              <input 
                v-model="form.DateOfBirth" 
                type="date" 
                class="form-input"
                required 
              />
            </div>

            <div class="form-group full-width">
              <label class="form-label">
                Địa chỉ <span class="required">*</span>
              </label>
              <input 
                v-model="form.Address" 
                type="text" 
                class="form-input"
                placeholder="Nhập địa chỉ đầy đủ"
                required 
              />
            </div>
          </div>
        </div>

        <!-- Work Info -->
        <div class="form-section">
          <h2>Thông tin công việc</h2>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">
                Ngày vào làm <span class="required">*</span>
              </label>
              <input 
                v-model="form.StartDate" 
                type="date" 
                class="form-input"
                required 
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Lương (VNĐ) <span class="required">*</span>
              </label>
              <input 
                v-model="form.Salary" 
                type="number" 
                min="0" 
                step="100000"
                class="form-input"
                placeholder="0"
                required 
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Trạng thái <span class="required">*</span>
              </label>
              <select v-model="form.Status" class="form-select" required>
                <option v-for="(label, code) in statusOptions" :key="code" :value="code">
                  {{ label }}
                </option>
              </select>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="form-actions">
          <button type="button" class="btn btn-secondary" @click="onCancel">
            Hủy bỏ
          </button>
          <button type="submit" class="btn btn-primary" :disabled="isLoading">
            {{ isLoading ? 'Đang lưu...' : 'Lưu nhân viên' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.employee-create-container {
  padding: 20px;
}

.header {
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.breadcrumb {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 12px;
  font-size: 14px;
}

.breadcrumb-item {
  color: #007bff;
  cursor: pointer;
}

.breadcrumb-item:hover {
  text-decoration: underline;
}

.breadcrumb-separator {
  color: #666;
}

.breadcrumb-current {
  color: #666;
}

.header h1 {
  margin: 0 0 8px 0;
  font-size: 24px;
  color: #333;
}

.header p {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.form-container {
  max-width: 800px;
  margin: 0 auto;
}

.employee-form {
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
  overflow: hidden;
}

.avatar-section,
.form-section {
  padding: 20px;
  border-bottom: 1px solid #eee;
}

.avatar-section:last-child,
.form-section:last-child {
  border-bottom: none;
}

.avatar-section h2,
.form-section h2 {
  margin: 0 0 16px 0;
  font-size: 18px;
  color: #333;
}

.avatar-content {
  display: flex;
  align-items: center;
  gap: 20px;
}

.avatar-preview {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  overflow: hidden;
  border: 1px solid #ddd;
  background: #f8f9fa;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.avatar-placeholder {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #666;
  font-size: 12px;
}

.avatar-actions {
  display: flex;
  gap: 10px;
}

.btn-upload {
  background: #007bff;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-upload:hover {
  background: #0056b3;
}

.btn-remove {
  background: #dc3545;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-remove:hover {
  background: #c82333;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group.full-width {
  grid-column: 1 / -1;
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
.form-select {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.form-input:focus,
.form-select:focus {
  outline: none;
  border-color: #007bff;
}

.form-actions {
  background: #f8f9fa;
  padding: 20px;
  display: flex;
  justify-content: flex-end;
  gap: 16px;
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

@media (max-width: 768px) {
  .employee-create-container {
    padding: 10px;
  }
  
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .form-group.full-width {
    grid-column: 1;
  }
  
  .form-actions {
    flex-direction: column-reverse;
  }
  
  .avatar-content {
    flex-direction: column;
    align-items: center;
  }
  
  .avatar-actions {
    flex-direction: column;
    width: 100%;
  }
}
</style>