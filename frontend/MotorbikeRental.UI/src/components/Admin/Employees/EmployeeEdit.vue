<script setup>
import { ref, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import defaultAvatar from '@/assets/image.png'
import { employeeService } from '@/api/services/employeeService'
import { getFullPath } from '@/utils/UrlUtils'

const previewImage = ref(null)
const fileInputRef = ref(null)
const props = defineProps({
  employee: { type: Object, required: true },
})
const emit = defineEmits(['update', 'create-account','delete-employee'])

const router = useRouter()
const route = useRoute();
const employeeId = route.params.id;
const form = ref({ ...props.employee })

watch(
  () => props.employee,
  (val) => {
    if (val) {
      form.value = { ...val }
      form.value.dateOfBirth = toDateInputString(val.dateOfBirth)
      form.value.startDate = toDateInputString(val.startDate)
      previewImage.value = val.avatar || null
    }
  },
  { immediate: true },
)

function onFileChange(event) {
  const file = event.target.files[0]
  if (file) {
    form.value.formFile = file
    const reader = new FileReader()
    reader.onload = (e) => {
      previewImage.value = e.target.result
    }
    reader.readAsDataURL(file)
  }
}

async function removeImage() {
  if (!confirm('Bạn có chắc chắn muốn xóa ảnh đại diện này không?')) {
    return
  }
  if (form.value.avatar) {
    try {
      await employeeService.deleteAvatar(form.value.employeeId)
      form.value.avatar = null
      previewImage.value = null
      form.value.FormFile = null
      if (fileInputRef.value) fileInputRef.value.value = ''
    } catch (error) {
      alert('Xóa ảnh thất bại!')
    }
  } else {
    form.value.FormFile = null
    previewImage.value = null
    if (fileInputRef.value) fileInputRef.value.value = ''
  }
}

function triggerFileInput() {
  fileInputRef.value?.click()
}

function onSubmit() {
  emit('update', form.value)
}

function onCreateAccount() {
  router.push('/admin/employees/create-user/' + employeeId)
}

function onEditAccount(){
  router.push('/admin/employees/edit-user/' + employeeId)
}

function onCancel() {
  router.back()
}

const statusOptions = {
  0: 'Đang làm',
  1: 'Xin nghỉ',
  2: 'Đã nghỉ',
}

function toDateInputString(dateStr) {
  if (!dateStr) return ''
  return dateStr.split('T')[0]
}

function onDeleteEmployee() {
  if (confirm('Bạn có chắc chắn muốn xóa nhân viên này không? Hành động này không thể hoàn tác!')) {
    emit('delete-employee')
  }
}
</script>

<template>
  <div class="employee-edit-container">
    <!-- Header -->
    <div class="header">
      <div class="header-left">
        <h1>Chỉnh sửa nhân viên</h1>
        <nav class="breadcrumb">
          <span class="breadcrumb-item" @click="router.push('/admin/employees')">
            Quản lý nhân viên
          </span>
          <span class="breadcrumb-separator">></span>
          <span class="breadcrumb-current">Chỉnh sửa</span>
        </nav>
      </div>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form @submit.prevent="onSubmit" class="employee-form">
        <!-- Avatar Section -->
        <div class="avatar-section">
          <h2>Ảnh đại diện</h2>
          <div class="avatar-content">
            <div class="avatar-preview">
              <img
                v-if="form.avatar"
                :src="getFullPath(form.avatar)"
                alt="Current Avatar"
                class="avatar-img"
              />
              <div v-else class="avatar-placeholder">
                <span>Chưa có ảnh</span>
              </div>
            </div>
            <div class="avatar-actions">
              <button type="button" @click="triggerFileInput" class="btn-upload">
                Chọn ảnh
              </button>
              <button
                v-if="previewImage || form.avatar"
                type="button"
                @click="removeImage"
                class="btn-remove"
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
        <div class="info-section">
          <h2>Thông tin cá nhân</h2>
          <div class="form-grid">
            <div class="form-group">
              <label>Họ và tên *</label>
              <input
                v-model="form.fullName"
                type="text"
                class="form-input"
                placeholder="Nhập họ và tên"
                required
              />
            </div>

            <div class="form-group">
              <label>Ngày sinh *</label>
              <input v-model="form.dateOfBirth" type="date" class="form-input" required />
            </div>

            <div class="form-group full-width">
              <label>Địa chỉ *</label>
              <input
                v-model="form.address"
                type="text"
                class="form-input"
                placeholder="Nhập địa chỉ"
                required
              />
            </div>
          </div>
        </div>

        <!-- Work Info -->
        <div class="info-section">
          <h2>Thông tin công việc</h2>
          <div class="form-grid">
            <div class="form-group">
              <label>Ngày vào làm *</label>
              <input v-model="form.startDate" type="date" class="form-input" required />
            </div>

            <div class="form-group">
              <label>Lương (VNĐ) *</label>
              <input
                v-model="form.salary"
                type="number"
                min="0"
                step="100000"
                class="form-input"
                placeholder="0"
                required
              />
            </div>

            <div class="form-group">
              <label>Trạng thái *</label>
              <select v-model="form.status" class="form-select" required>
                <option v-for="(label, code) in statusOptions" :key="code" :value="Number(code)">
                  {{ label }}
                </option>
              </select>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="form-actions">
          <div class="action-left">
            <button
              v-if="!form.roleName"
              type="button"
              @click="onCreateAccount"
              class="btn-account"
            >
              Tạo tài khoản
            </button>
            <button
              v-else
              type="button"
              @click="onEditAccount"
              class="btn-edit-account"
            >
              Chỉnh sửa tài khoản
            </button>
            <button
              type="button"
              @click="onDeleteEmployee"
              class="btn-danger"
            >
              Xóa nhân viên
            </button>
          </div>
          <div class="action-right">
            <button type="button" @click="onCancel" class="btn-cancel">
              Hủy bỏ
            </button>
            <button type="submit" class="btn-save">
              Lưu thay đổi
            </button>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.employee-edit-container {
  padding: 20px;
}

.header {
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.header-left h1 {
  margin: 0 0 8px 0;
  font-size: 24px;
  color: #333;
}

.breadcrumb {
  display: flex;
  align-items: center;
  gap: 8px;
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

.form-container {
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
  overflow: hidden;
}

.avatar-section,
.info-section {
  padding: 20px;
  border-bottom: 1px solid #eee;
}

.avatar-section:last-child,
.info-section:last-child {
  border-bottom: none;
}

.avatar-section h2,
.info-section h2 {
  margin: 0 0 15px 0;
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

.btn-upload,
.btn-remove {
  padding: 6px 12px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-upload {
  background: #007bff;
  color: white;
}

.btn-upload:hover {
  background: #0056b3;
}

.btn-remove {
  background: #dc3545;
  color: white;
}

.btn-remove:hover {
  background: #c82333;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group.full-width {
  grid-column: 1 / -1;
}

.form-group label {
  font-weight: 500;
  color: #333;
  font-size: 14px;
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
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  background: #f8f9fa;
  border-top: 1px solid #eee;
}

.action-left {
  display: flex;
  gap: 10px;
}

.action-right {
  display: flex;
  gap: 10px;
}

.btn-account,
.btn-edit-account,
.btn-danger,
.btn-cancel,
.btn-save {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-account {
  background: #28a745;
  color: white;
}

.btn-account:hover {
  background: #218838;
}

.btn-edit-account {
  background: #ffc107;
  color: #212529;
}

.btn-edit-account:hover {
  background: #e0a800;
}

.btn-danger {
  background: #dc3545;
  color: white;
}

.btn-danger:hover {
  background: #c82333;
}

.btn-cancel {
  background: #6c757d;
  color: white;
}

.btn-cancel:hover {
  background: #5a6268;
}

.btn-save {
  background: #007bff;
  color: white;
}

.btn-save:hover {
  background: #0056b3;
}

@media (max-width: 768px) {
  .employee-edit-container {
    padding: 10px;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .form-group.full-width {
    grid-column: 1;
  }

  .form-actions {
    flex-direction: column;
    gap: 16px;
  }

  .action-left,
  .action-right {
    width: 100%;
  }

  .action-left {
    flex-direction: column;
    gap: 8px;
  }

  .action-right {
    flex-direction: column-reverse;
    gap: 8px;
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