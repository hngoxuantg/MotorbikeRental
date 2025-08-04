<script setup>
import { jwtDecode } from 'jwt-decode'
import { ref, defineProps, defineEmits, watch, nextTick } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'

const props = defineProps({
  motorbike: {
    type: Object,
    required: true,
  },
  customer: {
    type: Object,
    required: true,
  },
  discounts: {
    type: Array,
    required: true,
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  price: {
    type: Object,
  },
})

const emits = defineEmits(['calculatePrice', 'createContract'])
const formData = ref({
  rentalDate: '',
  expectedReturnDate: '',
  totalAmount: 0,
  discountId: null,
  rentalTypeStatus: '',
  idCardHeld: false,
  status: '',
  note: '',
})

function convertToUTC(localDateTimeString) {
  if (!localDateTimeString) return ''
  
  const [datePart, timePart] = localDateTimeString.split('T')
  const [year, month, day] = datePart.split('-')
  const [hour, minute] = timePart.split(':')
  
  const localDate = new Date(
    parseInt(year),
    parseInt(month) - 1, // month is 0-indexed
    parseInt(day),
    parseInt(hour),
    parseInt(minute),
    0, // seconds
    0  // milliseconds
  )
  
  if (isNaN(localDate.getTime())) {
    console.error('Invalid date:', localDateTimeString)
    return ''
  }
  
  return localDate.toISOString()
}

function getTimezoneInfo() {
  const now = new Date()
  const timezoneOffset = now.getTimezoneOffset()
  const timezoneHours = -timezoneOffset / 60 
  
  return {
    offset: timezoneOffset,
    hours: timezoneHours,
    name: Intl.DateTimeFormat().resolvedOptions().timeZone
  }
}

function calculatePrice() {
  emits('calculatePrice', {
    rentalDate: convertToUTC(formData.value.rentalDate), // ✅ Chuyển sang UTC
    expectedReturnDate: convertToUTC(formData.value.expectedReturnDate), // ✅ Chuyển sang UTC
    rentalTypeStatus: formData.value.rentalTypeStatus,
    discountId: formData.value.discountId,
  })
}

function createContract() {
  const contractData = {
    ...formData.value,
    rentalDate: convertToUTC(formData.value.rentalDate),
    expectedReturnDate: convertToUTC(formData.value.expectedReturnDate)
  }
  
  emits('createContract', contractData)
}

watch(
  [
    () => formData.value.rentalDate,
    () => formData.value.expectedReturnDate,
    () => formData.value.discountId,
    () => formData.value.rentalTypeStatus,
  ],
  () => {
    if (
      formData.value.rentalDate &&
      formData.value.expectedReturnDate &&
      formData.value.rentalTypeStatus !== ''
    ) {
      calculatePrice()
    }
  }
)

watch(() => props.price, (newPrice) => {
  if(newPrice && newPrice.totalPrice){
    formData.value.totalAmount = newPrice.totalPrice
  }
})

function formatPrice(price){
  if(!price) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN').format(price) + ' VNĐ'
}

function formatDateTime(dateTimeString) {
  if (!dateTimeString) return ''
  
  const date = new Date(dateTimeString)
  return date.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

function debugDateConversion() {
  console.log('🕐 Date Conversion Debug:', {
    rentalDate_local: formData.value.rentalDate,
    rentalDate_UTC: convertToUTC(formData.value.rentalDate),
    expectedReturnDate_local: formData.value.expectedReturnDate,
    expectedReturnDate_UTC: convertToUTC(formData.value.expectedReturnDate),
    timezone: Intl.DateTimeFormat().resolvedOptions().timeZone
  })
}
</script>

<template>
  <div class="contract-page">
    <!-- Header -->
    <div class="page-header">
      <h2 class="page-title">
        <i class="fas fa-file-contract"></i>
        Thông tin hợp đồng thuê xe
      </h2>
    </div>

    <!-- Main Content -->
    <div class="page-content">
      <!-- Left Column - Information Display -->
      <div class="info-section">
        <!-- Customer Information -->
        <div class="info-panel">
          <div class="panel-header">
            <i class="fas fa-user"></i>
            <span>Thông tin khách hàng</span>
          </div>
          <div class="panel-body">
            <table class="info-table">
              <tr>
                <td class="label">Họ tên:</td>
                <td class="value">{{ customer?.fullName }}</td>
              </tr>
              <tr>
                <td class="label">Số điện thoại:</td>
                <td class="value">{{ customer?.phoneNumber }}</td>
              </tr>
              <tr>
                <td class="label">Email:</td>
                <td class="value">{{ customer?.email }}</td>
              </tr>
            </table>
          </div>
        </div>

        <!-- Motorbike Information -->
        <div class="info-panel">
          <div class="panel-header">
            <i class="fas fa-motorcycle"></i>
            <span>Thông tin xe máy</span>
          </div>
          <div class="panel-body">
            <div class="motorbike-layout">
              <div class="motorbike-info">
                <table class="info-table">
                  <tr>
                    <td class="label">Tên xe:</td>
                    <td class="value">{{ motorbike?.motorbikeName }}</td>
                  </tr>
                  <tr>
                    <td class="label">Biển số:</td>
                    <td class="value license-plate">{{ motorbike?.licensePlate }}</td>
                  </tr>
                  <tr>
                    <td class="label">Loại xe:</td>
                    <td class="value">{{ motorbike?.categoryName }}</td>
                  </tr>
                  <tr>
                    <td class="label">Hãng xe:</td>
                    <td class="value">{{ motorbike?.brand }}</td>
                  </tr>
                  <tr>
                    <td class="label">Dung tích:</td>
                    <td class="value">{{ motorbike?.engineCapacity }}</td>
                  </tr>
                </table>
                <div class="price-list">
                  <div class="price-row">
                    <i class="fas fa-clock"></i>
                    <span>Theo giờ: {{ formatPrice(motorbike?.hourlyRate) }}</span>
                  </div>
                  <div class="price-row">
                    <i class="fas fa-calendar-day"></i>
                    <span>Theo ngày: {{ formatPrice(motorbike?.dailyRate) }}</span>
                  </div>
                </div>
              </div>
              <div class="motorbike-image">
                <img
                  :src="motorbike?.imageUrl ? getFullPath(motorbike?.imageUrl) : '/placeholder-bike.jpg'"
                  :alt="motorbike?.motorbikeName"
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Right Column - Contract Form -->
      <div class="form-section">
        <div class="form-panel">
          <div class="panel-header">
            <i class="fas fa-edit"></i>
            <span>Điền thông tin hợp đồng</span>
          </div>
          <div class="panel-body">
            <form @submit.prevent="createContract" class="contract-form">
              <!-- Basic Information -->
              <div class="form-row">
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-cogs"></i>
                    Hình thức thuê
                  </label>
                  <select v-model="formData.rentalTypeStatus" class="form-input">
                    <option value="" disabled>-- Chọn hình thức thuê --</option>
                    <option :value="0">Theo giờ</option>
                    <option :value="1">Theo ngày</option>
                  </select>
                </div>

                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-flag"></i>
                    Tình trạng hợp đồng
                  </label>
                  <select v-model="formData.status" class="form-input">
                    <option value="" disabled>-- Tình trạng hợp đồng --</option>
                    <option :value="0">Thuê ngay</option>
                    <option :value="3">Đặt trước</option>
                  </select>
                </div>
              </div>

              <!-- Date Information -->
              <div class="form-row">
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-plus"></i>
                    Ngày bắt đầu thuê
                  </label>
                  <input 
                    v-model="formData.rentalDate" 
                    type="datetime-local" 
                    class="form-input" 
                    required 
                    @change="debugDateConversion"
                  />
                  <!-- ✅ THÊM: Hiển thị preview cho user -->
                  <small v-if="formData.rentalDate" class="datetime-preview">
                    <i class="fas fa-clock"></i>
                    {{ formatDateTime(formData.rentalDate) }}
                  </small>
                </div>

                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-minus"></i>
                    Ngày dự kiến trả
                  </label>
                  <input 
                    v-model="formData.expectedReturnDate" 
                    type="datetime-local" 
                    class="form-input" 
                    required 
                    @change="debugDateConversion"
                  />
                  <!-- ✅ THÊM: Hiển thị preview cho user -->
                  <small v-if="formData.expectedReturnDate" class="datetime-preview">
                    <i class="fas fa-clock"></i>
                    {{ formatDateTime(formData.expectedReturnDate) }}
                  </small>
                </div>
              </div>

              <!-- Discount -->
              <div class="form-row">
                <div class="form-group full-width">
                  <label class="form-label">
                    <i class="fas fa-percent"></i>
                    Mã giảm giá
                  </label>
                  <select v-model="formData.discountId" class="form-input">
                    <option :value="null">Không sử dụng</option>
                    <option
                      v-for="discount in discounts"
                      :key="discount.discountId"
                      :value="discount.discountId"
                    >
                      {{ discount.name }}
                    </option>
                  </select>
                </div>
              </div>

              <!-- Price Calculation Result -->
              <div v-if="price" class="price-calculation">
                <div class="calc-header">
                  <i class="fas fa-calculator"></i>
                  <span>Kết quả tính giá</span>
                </div>
                <table class="calc-table">
                  <tr>
                    <td>Giá gốc:</td>
                    <td class="text-right">{{ formatPrice(price.rentalPrice) }}</td>
                  </tr>
                  <tr v-if="price.discountAmount">
                    <td>Giảm giá:</td>
                    <td class="text-right text-red">-{{ formatPrice(price.discountAmount) }}</td>
                  </tr>
                  <tr>
                    <td>Tiền đặt cọc:</td>
                    <td class="text-right text-orange">{{ formatPrice(price.depositAmount) }}</td>
                  </tr>
                  <tr class="total-row">
                    <td><strong>Tổng tiền:</strong></td>
                    <td class="text-right"><strong>{{ formatPrice(price.totalPrice) }}</strong></td>
                  </tr>
                </table>
              </div>

              <!-- Contract Amount -->
              <div class="form-row">
                <div class="form-group full-width">
                  <label class="form-label">
                    <i class="fas fa-money-bill-wave"></i>
                    Tổng tiền hợp đồng
                  </label>
                  <input 
                    v-model.number="formData.totalAmount" 
                    type="number" 
                    class="form-input amount-input" 
                    placeholder="Nhập tổng tiền hợp đồng..."
                    required
                  />
                  <small v-if="price" class="form-note">
                    <i class="fas fa-info-circle"></i>
                    Hệ thống tính: {{ formatPrice(price.totalPrice) }}
                  </small>
                </div>
              </div>

              <!-- Notes -->
              <div class="form-row">
                <div class="form-group full-width">
                  <label class="form-label">
                    <i class="fas fa-sticky-note"></i>
                    Ghi chú
                  </label>
                  <textarea 
                    v-model="formData.note" 
                    class="form-input form-textarea" 
                    rows="3"
                    placeholder="Nhập ghi chú thêm (nếu có)..."
                  ></textarea>
                </div>
              </div>

              <!-- ID Card Checkbox -->
              <div class="form-row">
                <div class="form-group full-width">
                  <label class="checkbox-container">
                    <input 
                      v-model="formData.idCardHeld" 
                      type="checkbox" 
                      class="checkbox-input"
                    />
                    <span class="checkmark"></span>
                    <span class="checkbox-text">
                      <i class="fas fa-id-card"></i>
                      Đã giữ CMND/CCCD của khách hàng
                    </span>
                  </label>
                </div>
              </div>

              <!-- Submit Button -->
              <div class="form-actions">
                <button 
                  type="submit" 
                  :disabled="isLoading" 
                  class="submit-button"
                >
                  <i class="fas fa-check-circle"></i>
                  {{ isLoading ? 'Đang tạo hợp đồng...' : 'Tạo hợp đồng' }}
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
/* ...existing styles... */

/* ✅ THÊM: Style cho datetime preview */
.datetime-preview {
  color: #666;
  font-size: 11px;
  display: flex;
  align-items: center;
  gap: 4px;
  margin-top: 2px;
  font-style: italic;
}

.datetime-preview i {
  color: #28a745;
}

/* Existing styles remain the same */
.contract-page {
  background: #f5f5f5;
  min-height: 100vh;
  font-family: Arial, sans-serif;
}

/* Header */
.page-header {
  background: #fff;
  border-bottom: 1px solid #ddd;
  padding: 15px 20px;
}

.page-title {
  margin: 0;
  color: #333;
  font-size: 18px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 10px;
}

.page-title i {
  color: #28a745;
}

/* Main Content */
.page-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  padding: 20px;
  max-width: none;
  width: 100%;
}

/* Panels */
.info-panel, .form-panel {
  background: #fff;
  border: 1px solid #ddd;
  margin-bottom: 20px;
}

.panel-header {
  background: #f8f9fa;
  border-bottom: 1px solid #ddd;
  padding: 12px 15px;
  font-weight: bold;
  color: #333;
  display: flex;
  align-items: center;
  gap: 8px;
}

.panel-header i {
  color: #28a745;
}

.panel-body {
  padding: 15px;
}

/* Info Tables */
.info-table {
  width: 100%;
  border-collapse: collapse;
}

.info-table td {
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.info-table td.label {
  width: 120px;
  color: #666;
  font-weight: 500;
}

.info-table td.value {
  color: #333;
  font-weight: 600;
}

.license-plate {
  color: #dc3545;
  background: #f8f9fa;
  padding: 2px 6px;
  border-radius: 3px;
  font-family: monospace;
}

/* Motorbike Layout */
.motorbike-layout {
  display: grid;
  grid-template-columns: 1fr auto;
  gap: 15px;
}

.motorbike-image {
  width: 100px;
}

.motorbike-image img {
  width: 100%;
  height: 80px;
  object-fit: cover;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.price-list {
  margin-top: 15px;
  padding-top: 15px;
  border-top: 1px solid #eee;
}

.price-row {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 6px;
  color: #28a745;
  font-weight: 600;
  font-size: 14px;
}

.price-row i {
  color: #6c757d;
  width: 14px;
}

/* Form */
.contract-form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.form-group.full-width {
  grid-column: 1 / -1;
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
  color: #28a745;
  width: 14px;
}

.form-input {
  padding: 8px 12px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
  background: #fff;
}

.form-input:focus {
  outline: none;
  border-color: #28a745;
}

.form-textarea {
  resize: vertical;
  min-height: 70px;
}

.amount-input {
  font-weight: 600;
  color: #28a745;
}

.form-note {
  color: #666;
  font-size: 12px;
  display: flex;
  align-items: center;
  gap: 4px;
}

/* Price Calculation */
.price-calculation {
  background: #f8f9fa;
  border: 1px solid #28a745;
  border-radius: 4px;
  margin: 15px 0;
}

.calc-header {
  background: #28a745;
  color: white;
  padding: 10px 15px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 8px;
}

.calc-table {
  width: 100%;
  border-collapse: collapse;
  margin: 15px;
  width: calc(100% - 30px);
}

.calc-table td {
  padding: 6px 0;
  border-bottom: 1px solid #eee;
}

.text-right {
  text-align: right;
  font-weight: 600;
  font-family: monospace;
}

.text-red {
  color: #dc3545;
}

.text-orange {
  color: #fd7e14;
}

.total-row {
  border-top: 2px solid #28a745;
}

.total-row td {
  padding-top: 10px;
  font-size: 16px;
  color: #28a745;
}

/* Checkbox */
.checkbox-container {
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  user-select: none;
  padding: 10px 0;
}

.checkbox-input {
  display: none;
}

.checkmark {
  width: 18px;
  height: 18px;
  border: 2px solid #ccc;
  border-radius: 3px;
  position: relative;
  transition: all 0.2s;
}

.checkbox-input:checked + .checkmark {
  background: #28a745;
  border-color: #28a745;
}

.checkbox-input:checked + .checkmark::after {
  content: '✓';
  color: white;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  font-size: 12px;
  font-weight: bold;
}

.checkbox-text {
  color: #333;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 6px;
}

/* Submit Button */
.form-actions {
  margin-top: 20px;
  padding-top: 15px;
  border-top: 1px solid #eee;
}

.submit-button {
  width: 100%;
  background: #28a745;
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 4px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.submit-button:hover:not(:disabled) {
  background: #218838;
}

.submit-button:disabled {
  background: #6c757d;
  cursor: not-allowed;
}

/* Responsive */
@media (max-width: 1200px) {
  .page-content {
    grid-template-columns: 1fr;
  }
  
  .form-row {
    grid-template-columns: 1fr;
  }
  
  .motorbike-layout {
    grid-template-columns: 1fr;
    text-align: center;
  }
  
  .motorbike-image {
    width: 150px;
    margin: 0 auto;
  }
}

@media (max-width: 768px) {
  .page-content {
    padding: 15px;
    gap: 15px;
  }
  
  .panel-body {
    padding: 12px;
  }
  
  .calc-table {
    margin: 10px;
    width: calc(100% - 20px);
  }
}
</style>