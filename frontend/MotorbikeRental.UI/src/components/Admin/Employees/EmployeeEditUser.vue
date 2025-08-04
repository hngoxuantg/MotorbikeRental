<script setup>
import { ref } from 'vue'

const props = defineProps({
  userCredentials: {
    type: Object,
    required: true,
  },
  roles: {
    type: Array,
    required: true,
  },
})

const editing = ref({
  userName: false,
  email: false,
  phoneNumber: false,
  password: false,
  role: false,
})

const temp = ref({
  userName: props.userCredentials.userName,
  email: props.userCredentials.email,
  phoneNumber: props.userCredentials.phoneNumber,
  password: '',
  confirmPassword: '',
  roleId: props.userCredentials.roleId,
})

const emit = defineEmits(['reset-username', 'reset-email', 'reset-phone-number', 'reset-password', 'reset-role', 'delete-user'])

function startEdit(field) {
  editing.value[field] = true
  temp.value[field] = props.userCredentials[field] || ''
  if (field === 'role') {
    temp.value.roleId = props.userCredentials.roleId
  }
}

function cancelEdit(field) {
  editing.value[field] = false
  temp.value[field] = props.userCredentials[field] || ''
  if (field === 'password') {
    temp.value.password = ''
    temp.value.confirmPassword = ''
  }
  if (field === 'role') {
    temp.value.roleId = props.userCredentials.roleId
  }
}

function deleteUser() {
  if (confirm('Bạn có chắc chắn muốn xóa tài khoản này không? Hành động này không thể hoàn tác!')) {
    emit('delete-user')
  }
}

function saveUserName() {
  emit('reset-username', { userName: temp.value.userName })
  editing.value.userName = false
}

function saveEmail() {
  emit('reset-email', { email: temp.value.email })
  editing.value.email = false
}

function savePhoneNumber() {
  emit('reset-phone-number', { phoneNumber: temp.value.phoneNumber })
  editing.value.phoneNumber = false
}

function savePassword() {
  if (!temp.value.password || !temp.value.confirmPassword) {
    alert('Vui lòng nhập đầy đủ thông tin!')
    return
  }
  if (temp.value.password !== temp.value.confirmPassword) {
    alert('Mật khẩu mới không khớp!')
    return
  }
  emit('reset-password', {
    password: temp.value.password,
    confirmPassword: temp.value.confirmPassword,
  })
  editing.value.password = false
  temp.value.password = ''
  temp.value.confirmPassword = ''
}

function saveRole() {
  emit('reset-role', { roleId: temp.value.roleId })
  editing.value.role = false
}
</script>

<template>
  <div class="edit-user-container">
    <!-- Header -->
    <div class="header">
      <h1>Chỉnh sửa tài khoản nhân viên</h1>
    </div>
    
    <!-- Form -->
    <div class="form-container">
      <!-- UserName -->
      <div class="form-group">
        <label class="form-label">Tên đăng nhập</label>
        <div class="form-field">
          <template v-if="editing.userName">
            <div class="edit-mode">
              <input 
                v-model="temp.userName" 
                class="form-input"
                placeholder="Nhập tên đăng nhập"
              />
              <div class="button-group">
                <button @click="saveUserName" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('userName')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.userName }}</span>
              <button @click="startEdit('userName')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Email -->
      <div class="form-group">
        <label class="form-label">Email</label>
        <div class="form-field">
          <template v-if="editing.email">
            <div class="edit-mode">
              <input 
                v-model="temp.email" 
                type="email" 
                class="form-input"
                placeholder="Nhập email"
              />
              <div class="button-group">
                <button @click="saveEmail" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('email')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.email }}</span>
              <button @click="startEdit('email')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Phone Number -->
      <div class="form-group">
        <label class="form-label">Số điện thoại</label>
        <div class="form-field">
          <template v-if="editing.phoneNumber">
            <div class="edit-mode">
              <input 
                v-model="temp.phoneNumber" 
                type="tel" 
                class="form-input"
                placeholder="Nhập số điện thoại"
              />
              <div class="button-group">
                <button @click="savePhoneNumber" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('phoneNumber')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.phoneNumber }}</span>
              <button @click="startEdit('phoneNumber')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Role -->
      <div class="form-group">
        <label class="form-label">Vai trò</label>
        <div class="form-field">
          <template v-if="editing.role">
            <div class="edit-mode">
              <select 
                v-model="temp.roleId" 
                class="form-input"
              >
                <option v-for="role in props.roles" :key="role.id" :value="role.id">
                  {{ role.roleName }} - {{ role.description }}
                </option>
              </select>
              <div class="button-group">
                <button @click="saveRole" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('role')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.roleName }}</span>
              <button @click="startEdit('role')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Password -->
      <div class="form-group">
        <label class="form-label">Mật khẩu</label>
        <div class="form-field">
          <template v-if="editing.password">
            <div class="edit-mode">
              <div class="password-inputs">
                <input 
                  v-model="temp.password" 
                  type="password" 
                  class="form-input"
                  placeholder="Mật khẩu mới" 
                />
                <input 
                  v-model="temp.confirmPassword" 
                  type="password" 
                  class="form-input"
                  placeholder="Xác nhận mật khẩu mới" 
                />
              </div>
              <div class="button-group">
                <button @click="savePassword" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('password')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">********</span>
              <button @click="startEdit('password')" class="btn btn-edit">Đổi mật khẩu</button>
            </div>
          </template>
        </div>
      </div>
    </div>

    <!-- Danger Zone -->
    <div class="danger-zone">
      <h2>Vùng nguy hiểm</h2>
      <p>Xóa tài khoản này sẽ không thể hoàn tác</p>
      <button @click="deleteUser" class="btn btn-danger">Xóa tài khoản</button>
    </div>
  </div>
</template>

<style scoped>
.edit-user-container {
  padding: 20px;
}

.header {
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.header h1 {
  margin: 0;
  font-size: 24px;
  color: #333;
}

.form-container {
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.form-label {
  display: block;
  font-weight: 500;
  color: #333;
  margin-bottom: 12px;
  font-size: 14px;
}

.form-field {
  background: #f8f9fa;
  border: 1px solid #e9ecef;
  border-radius: 4px;
  padding: 12px;
}

.view-mode {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.field-value {
  font-size: 14px;
  color: #333;
  font-weight: 500;
}

.edit-mode {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.password-inputs {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-input {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  width: 100%;
}

.form-input:focus {
  outline: none;
  border-color: #007bff;
}

.button-group {
  display: flex;
  gap: 8px;
}

.btn {
  padding: 6px 12px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
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

.btn-save:hover {
  background: #218838;
}

.btn-cancel {
  background: #6c757d;
  color: white;
}

.btn-cancel:hover {
  background: #5a6268;
}

.danger-zone {
  padding: 20px;
  background: #fff5f5;
  border: 1px solid #fed7d7;
  border-radius: 8px;
  text-align: center;
}

.danger-zone h2 {
  margin: 0 0 8px 0;
  color: #dc3545;
  font-size: 18px;
}

.danger-zone p {
  margin: 0 0 12px 0;
  color: #666;
  font-size: 14px;
}

.btn-danger {
  background: #dc3545;
  color: white;
  padding: 8px 16px;
  font-size: 14px;
}

.btn-danger:hover {
  background: #c82333;
}

@media (max-width: 768px) {
  .edit-user-container {
    padding: 10px;
  }
  
  .view-mode {
    flex-direction: column;
    align-items: flex-start;
    gap: 8px;
  }
  
  .button-group {
    flex-direction: column;
    width: 100%;
  }
  
  .btn {
    width: 100%;
  }
}
</style>