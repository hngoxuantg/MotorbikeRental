<script setup>
import { ref } from 'vue'

const props = defineProps({
  roles: {
    type: Array,
    required: true,
  },
  form: {
    type: Object,
    required: true,
  },
})

const emit = defineEmits(['submit'])
const showPassword = ref(false)
const showConfirmPassword = ref(false)

function onSubmit() {
  emit('submit', {
    ...props.form,
    roleId: Number(props.form.roleId),
  })
}

function togglePasswordVisibility() {
  showPassword.value = !showPassword.value
}

function toggleConfirmPasswordVisibility() {
  showConfirmPassword.value = !showConfirmPassword.value
}
</script>

<template>
  <div class="create-user-container">
    <!-- Header -->
    <div class="header">
      <h1>Tạo tài khoản nhân viên</h1>
      <p>Tạo tài khoản đăng nhập cho nhân viên</p>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form class="user-form" @submit.prevent="onSubmit">
        <!-- Account Info -->
        <div class="form-section">
          <h2>Thông tin tài khoản</h2>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">
                Tên đăng nhập <span class="required">*</span>
              </label>
              <input 
                v-model="form.userName" 
                type="text" 
                class="form-input"
                placeholder="Nhập tên đăng nhập"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Quyền hạn <span class="required">*</span>
              </label>
              <select v-model="form.roleId" class="form-select" required>
                <option value="">Chọn quyền hạn</option>
                <option v-for="role in roles" :key="role.id" :value="role.id">
                  {{ role.roleName }}
                </option>
              </select>
            </div>

            <div class="form-group">
              <label class="form-label">
                Mật khẩu <span class="required">*</span>
              </label>
              <div class="password-wrapper">
                <input 
                  v-model="form.password" 
                  :type="showPassword ? 'text' : 'password'" 
                  class="form-input password-input"
                  placeholder="Nhập mật khẩu"
                  required
                />
                <button 
                  type="button" 
                  class="password-toggle"
                  @click="togglePasswordVisibility"
                >
                  {{ showPassword ? 'Ẩn' : 'Hiện' }}
                </button>
              </div>
            </div>

            <div class="form-group">
              <label class="form-label">
                Xác nhận mật khẩu <span class="required">*</span>
              </label>
              <div class="password-wrapper">
                <input 
                  v-model="form.confirmPassword" 
                  :type="showConfirmPassword ? 'text' : 'password'" 
                  class="form-input password-input"
                  placeholder="Xác nhận mật khẩu"
                  required
                />
                <button 
                  type="button" 
                  class="password-toggle"
                  @click="toggleConfirmPasswordVisibility"
                >
                  {{ showConfirmPassword ? 'Ẩn' : 'Hiện' }}
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Contact Info -->
        <div class="form-section">
          <h2>Thông tin liên hệ</h2>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">
                Số điện thoại <span class="required">*</span>
              </label>
              <input 
                v-model="form.phoneNumber" 
                type="tel" 
                class="form-input"
                placeholder="Nhập số điện thoại"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                Email <span class="required">*</span>
              </label>
              <input 
                v-model="form.email" 
                type="email" 
                class="form-input"
                placeholder="Nhập địa chỉ email"
                required
              />
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="form-actions">
          <button type="button" class="btn btn-secondary" @click="$router.back()">
            Hủy bỏ
          </button>
          <button type="submit" class="btn btn-primary">
            Tạo tài khoản
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.create-user-container {
  padding: 20px;
}

.header {
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
  text-align: center;
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

.user-form {
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
  overflow: hidden;
}

.form-section {
  padding: 20px;
  border-bottom: 1px solid #eee;
}

.form-section:last-child {
  border-bottom: none;
}

.form-section h2 {
  margin: 0 0 16px 0;
  font-size: 18px;
  color: #333;
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

.password-wrapper {
  position: relative;
}

.password-input {
  padding-right: 60px;
}

.password-toggle {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  cursor: pointer;
  font-size: 12px;
  color: #007bff;
}

.password-toggle:hover {
  text-decoration: underline;
}

.form-actions {
  background: #f8f9fa;
  padding: 20px;
  display: flex;
  justify-content: center;
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
  min-width: 120px;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover {
  background: #0056b3;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #5a6268;
}

@media (max-width: 768px) {
  .create-user-container {
    padding: 10px;
  }
  
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .form-actions {
    flex-direction: column;
  }
}
</style>