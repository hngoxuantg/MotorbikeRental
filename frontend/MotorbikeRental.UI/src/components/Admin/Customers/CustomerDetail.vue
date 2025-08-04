<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  customer: {
    type: Object,
    required: true,
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['update-customer', 'delete-customer', 'back'])

// Form data
const form = ref({
  customerId: props.customer.customerId,
  fullName: props.customer.fullName,
  gender: props.customer.gender,
  idNumber: props.customer.idNumber,
  phoneNumber: props.customer.phoneNumber,
  email: props.customer.email,
})

// Edit mode
const isEditing = ref(false)

// Watch for customer prop changes
watch(
  () => props.customer,
  (newCustomer) => {
    form.value = {
      customerId: newCustomer.customerId,
      fullName: newCustomer.fullName,
      gender: newCustomer.gender,
      idNumber: newCustomer.idNumber,
      phoneNumber: newCustomer.phoneNumber,
      email: newCustomer.email,
    }
  },
  { deep: true },
)

function toggleEdit() {
  isEditing.value = !isEditing.value
  if (!isEditing.value) {
    // Reset form if canceling
    form.value = {
      customerId: props.customer.customerId,
      fullName: props.customer.fullName,
      gender: props.customer.gender,
      idNumber: props.customer.idNumber,
      phoneNumber: props.customer.phoneNumber,
      email: props.customer.email,
    }
  }
}

function saveCustomer() {
  emit('update-customer', form.value)
  isEditing.value = false
}

function deleteCustomer() {
  if (
    confirm('Bạn có chắc chắn muốn xóa khách hàng này không? Hành động này không thể hoàn tác!')
  ) {
    emit('delete-customer', props.customer.customerId)
  }
}

function handleBack() {
  emit('back')
}

function formatDateVN(isoDate) {
  if (!isoDate) return ''
  return new Date(isoDate).toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    timeZone: 'Asia/Ho_Chi_Minh'
  })
}

function getGenderText(gender) {
  const genderOptions = {
    0: 'Nam',
    1: 'Nữ',
    2: 'Khác',
  }
  return genderOptions[gender] || 'Không xác định'
}

function getGenderBadgeClass(gender) {
  switch (gender) {
    case 0:
      return 'badge-male'
    case 1:
      return 'badge-female'
    default:
      return 'badge-other'
  }
}
</script>

<template>
  <div class="customer-detail-container">
    <!-- Header -->
    <div class="detail-header">
      <div class="header-left">
        <h1>Chi tiết khách hàng</h1>
        <p>Thông tin khách hàng {{ customer.fullName }}</p>
      </div>
      <div class="header-actions">
        <button @click="handleBack" class="btn-back">← Quay lại</button>
        <button v-if="!isEditing" @click="toggleEdit" class="btn-edit">Chỉnh sửa</button>
        <button v-if="isEditing" @click="saveCustomer" class="btn-save">Lưu thay đổi</button>
        <button v-if="isEditing" @click="toggleEdit" class="btn-cancel">Hủy bỏ</button>
        <button @click="deleteCustomer" class="btn-delete">Xóa khách hàng</button>
      </div>
    </div>

    <!-- Customer Info -->
    <div class="info-section">
      <h2>Thông tin cá nhân</h2>
      <div class="info-grid">
        <div class="info-item">
          <label>Họ và tên</label>
          <input
            v-if="isEditing"
            v-model="form.fullName"
            type="text"
            class="info-input"
            placeholder="Nhập họ và tên"
          />
          <span v-else class="info-value">{{ customer.fullName }}</span>
        </div>

        <div class="info-item">
          <label>Giới tính</label>
          <select v-if="isEditing" v-model="form.gender" class="info-select">
            <option :value="0">Nam</option>
            <option :value="1">Nữ</option>
            <option :value="2">Khác</option>
          </select>
          <span v-else class="info-value">
            <span class="gender-badge" :class="getGenderBadgeClass(customer.gender)">
              {{ getGenderText(customer.gender) }}
            </span>
          </span>
        </div>

        <div class="info-item">
          <label>Số CCCD/CMND</label>
          <input
            v-if="isEditing"
            v-model="form.idNumber"
            type="text"
            class="info-input"
            placeholder="Nhập số CCCD/CMND"
          />
          <span v-else class="info-value id-number">{{ customer.idNumber }}</span>
        </div>

        <div class="info-item">
          <label>Số điện thoại</label>
          <input
            v-if="isEditing"
            v-model="form.phoneNumber"
            type="tel"
            class="info-input"
            placeholder="Nhập số điện thoại"
          />
          <a v-else :href="`tel:${customer.phoneNumber}`" class="info-value phone-link">
            {{ customer.phoneNumber }}
          </a>
        </div>

        <div class="info-item">
          <label>Email</label>
          <input
            v-if="isEditing"
            v-model="form.email"
            type="email"
            class="info-input"
            placeholder="Nhập email"
          />
          <a v-else :href="`mailto:${customer.email}`" class="info-value email-link">
            {{ customer.email }}
          </a>
        </div>

        <div class="info-item">
          <label>ID khách hàng</label>
          <span class="info-value customer-id">#{{ customer.customerId }}</span>
        </div>
      </div>
    </div>

    <!-- Statistics -->
    <div class="info-section">
      <h2>Thống kê</h2>
      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-label">Số lần thuê xe</div>
          <div class="stat-value">{{ customer.rentalCount }}</div>
        </div>
        <div v-if="customer.createAt" class="stat-card">
          <div class="stat-label">Ngày tạo tài khoản</div>
          <div class="stat-value">{{ formatDateVN(customer.createAt) }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.customer-detail-container {
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

.btn-delete {
  background: #dc3545;
  color: white;
}

.btn-delete:hover {
  background: #c82333;
}

.info-section {
  margin-bottom: 30px;
  padding: 20px;
  background: white;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.info-section h2 {
  margin: 0 0 20px 0;
  font-size: 18px;
  color: #333;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
}

.info-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
}

.info-item {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.info-item label {
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

.info-input,
.info-select {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.info-input:focus,
.info-select:focus {
  outline: none;
  border-color: #007bff;
}

.gender-badge {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.badge-male {
  background: #e3f2fd;
  color: #1565c0;
}

.badge-female {
  background: #fce4ec;
  color: #ad1457;
}

.badge-other {
  background: #f5f5f5;
  color: #666;
}

.id-number {
  font-family: monospace;
  background: #f8f9fa;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 13px;
  color: #495057;
}

.phone-link,
.email-link {
  color: #007bff;
  text-decoration: none;
}

.phone-link:hover,
.email-link:hover {
  text-decoration: underline;
}

.customer-id {
  font-weight: 600;
  color: #6366f1;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
}

.stat-card {
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e9ecef;
  text-align: center;
}

.stat-label {
  font-size: 14px;
  color: #666;
  margin-bottom: 8px;
}

.stat-value {
  font-size: 24px;
  font-weight: 600;
  color: #333;
}

@media (max-width: 768px) {
  .customer-detail-container {
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

  .info-grid {
    grid-template-columns: 1fr;
  }

  .stats-grid {
    grid-template-columns: 1fr;
  }
}
</style>
