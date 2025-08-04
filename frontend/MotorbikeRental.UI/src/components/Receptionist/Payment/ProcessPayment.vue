<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  payment: {
    type: Object,
    default: null
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['payment-success'])

const isProcessing = ref(false)
const paymentForm = ref({
  paymentDate: '',
  paymentStatus: 0
})

const fillCurrentDateTime = () => {
  const now = new Date()
  const year = now.getFullYear()
  const month = String(now.getMonth() + 1).padStart(2, '0')
  const day = String(now.getDate()).padStart(2, '0')
  const hours = String(now.getHours()).padStart(2, '0')
  const minutes = String(now.getMinutes()).padStart(2, '0')

  paymentForm.value.paymentDate = `${year}-${month}-${day}T${hours}:${minutes}`
}

const convertToUTC = (localDateTimeString) => {
  if (!localDateTimeString) return ''
  const localDate = new Date(localDateTimeString)
  return localDate.toISOString()
}

const formatPrice = (price) => {
  if (!price) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN').format(price) + ' VNĐ'
}

const paymentStatusOptions = [
  { value: 0, label: 'Tiền mặt', icon: 'fa-money-bill-alt' },
  { value: 1, label: 'Chuyển khoản', icon: 'fa-credit-card' },
  { value: 2, label: 'Ví điện tử', icon: 'fa-mobile-alt' }
]

const selectedPaymentMethod = computed(() => {
  return paymentStatusOptions.find(option => option.value === paymentForm.value.paymentStatus)
})

const handleSubmit = () => {
  if (!paymentForm.value.paymentDate) {
    alert('Vui lòng chọn ngày thanh toán')
    return
  }

  const submitData = {
    paymentDate: convertToUTC(paymentForm.value.paymentDate),
    paymentStatus: paymentForm.value.paymentStatus
  }

  emit('payment-success', submitData)
}

fillCurrentDateTime()
</script>

<template>
  <div class="payment-process-page">
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner">
        <i class="fas fa-spinner fa-spin"></i>
        <span>Đang tải thông tin thanh toán...</span>
      </div>
    </div>

    <div v-else-if="payment" class="payment-content">
      <div class="page-header">
        <h2 class="page-title">
          <i class="fas fa-credit-card"></i>
          Xử lý thanh toán - Hợp đồng #{{ payment.contractId }}
        </h2>
      </div>

      <div class="payment-container">
        <div class="payment-info-section">
          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-file-invoice"></i>
              <span>Thông tin thanh toán</span>
            </div>
            <div class="panel-body">
              <div class="payment-summary">
                <div class="summary-row">
                  <span class="label">Hợp đồng:</span>
                  <span class="value contract-id">#{{ payment.contractId }}</span>
                </div>
                
                <div class="summary-row">
                  <span class="label">Khách hàng:</span>
                  <span class="value">{{ payment.customerName }}</span>
                </div>
                
                <div v-if="payment.contractIndemnity" class="summary-row">
                  <span class="label">Tiền bồi thường:</span>
                  <span class="value indemnity">{{ formatPrice(payment.contractIndemnity) }}</span>
                </div>
                
                <div v-if="payment.incidentFineAmount > 0" class="summary-row">
                  <span class="label">Phí phạt sự cố:</span>
                  <span class="value fine">{{ formatPrice(payment.incidentFineAmount) }}</span>
                </div>
                
                <div class="summary-row total">
                  <span class="label">Tổng số tiền phải thanh toán:</span>
                  <span class="value">{{ formatPrice(payment.amount) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="payment-form-section">
          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-cash-register"></i>
              <span>Xác nhận thanh toán</span>
            </div>
            <div class="panel-body">
              <form @submit.prevent="handleSubmit" class="payment-form">
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-alt"></i>
                    Thời gian thanh toán
                  </label>
                  <div class="datetime-input-group">
                    <input 
                      v-model="paymentForm.paymentDate" 
                      type="datetime-local" 
                      class="form-input" 
                      required 
                    />
                    <button 
                      type="button" 
                      @click="fillCurrentDateTime"
                      class="btn-now"
                    >
                      <i class="fas fa-clock"></i>
                      Bây giờ
                    </button>
                  </div>
                </div>

                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-credit-card"></i>
                    Phương thức thanh toán
                  </label>
                  <div class="payment-methods">
                    <div 
                      v-for="option in paymentStatusOptions" 
                      :key="option.value"
                      class="payment-method-option"
                      :class="{ active: paymentForm.paymentStatus === option.value }"
                      @click="paymentForm.paymentStatus = option.value"
                    >
                      <div class="method-icon">
                        <i :class="['fas', option.icon]"></i>
                      </div>
                      <div class="method-label">{{ option.label }}</div>
                      <div class="method-radio">
                        <input 
                          type="radio" 
                          :value="option.value" 
                          v-model="paymentForm.paymentStatus"
                        />
                      </div>
                    </div>
                  </div>
                </div>

                <div class="payment-confirm">
                  <div class="confirm-summary">
                    <div class="confirm-row">
                      <span>Khách hàng thanh toán:</span>
                      <strong>{{ formatPrice(payment.amount) }}</strong>
                    </div>
                    <div class="confirm-row">
                      <span>Phương thức:</span>
                      <strong>{{ selectedPaymentMethod?.label }}</strong>
                    </div>
                  </div>
                  
                  <button 
                    type="submit" 
                    :disabled="isProcessing" 
                    class="submit-button"
                  >
                    <i class="fas fa-check-circle"></i>
                    {{ isProcessing ? 'Đang xử lý...' : 'Xác nhận thanh toán' }}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="no-data">
      <i class="fas fa-exclamation-triangle"></i>
      <p>Không tìm thấy thông tin thanh toán</p>
    </div>
  </div>
</template>

<style scoped>
.payment-process-page {
  background: #f5f5f5;
  min-height: 100vh;
  font-family: Arial, sans-serif;
}

.loading-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 300px;
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
  color: #28a745;
}

.page-header {
  background: #fff;
  border-bottom: 1px solid #ddd;
  padding: 20px;
  margin-bottom: 20px;
}

.page-title {
  margin: 0;
  color: #333;
  font-size: 20px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 10px;
}

.page-title i {
  color: #28a745;
}

.payment-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  padding: 0 20px 20px;
}

.info-panel {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  overflow: hidden;
}

.panel-header {
  background: #f8f9fa;
  border-bottom: 1px solid #ddd;
  padding: 15px 20px;
  font-weight: bold;
  color: #333;
  display: flex;
  align-items: center;
  gap: 10px;
}

.panel-header i {
  color: #28a745;
}

.panel-body {
  padding: 20px;
}

.payment-summary {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
}

.summary-row.total {
  border-top: 2px solid #28a745;
  border-bottom: none;
  margin-top: 15px;
  padding-top: 20px;
  font-size: 18px;
  font-weight: bold;
  color: #28a745;
}

.summary-row .label {
  color: #666;
  font-weight: 500;
}

.summary-row .value {
  font-weight: 600;
  font-family: monospace;
}

.contract-id {
  color: #007bff;
  font-size: 16px;
}

.value.indemnity, .value.fine {
  color: #dc3545;
}

.payment-form {
  display: flex;
  flex-direction: column;
  gap: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.form-label {
  font-weight: 600;
  color: #333;
  display: flex;
  align-items: center;
  gap: 8px;
}

.form-label i {
  color: #28a745;
}

.datetime-input-group {
  display: flex;
  gap: 10px;
}

.form-input {
  flex: 1;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.form-input:focus {
  outline: none;
  border-color: #28a745;
  box-shadow: 0 0 0 2px rgba(40, 167, 69, 0.2);
}

.btn-now {
  padding: 12px 16px;
  background: #6c757d;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 6px;
  white-space: nowrap;
}

.btn-now:hover {
  background: #5a6268;
}

.payment-methods {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 15px;
}

.payment-method-option {
  border: 2px solid #e9ecef;
  border-radius: 8px;
  padding: 20px 15px;
  text-align: center;
  cursor: pointer;
  transition: all 0.2s;
  position: relative;
}

.payment-method-option:hover {
  border-color: #28a745;
  background: #f8f9fa;
}

.payment-method-option.active {
  border-color: #28a745;
  background: #d4edda;
}

.method-icon {
  font-size: 24px;
  color: #6c757d;
  margin-bottom: 10px;
}

.payment-method-option.active .method-icon {
  color: #28a745;
}

.method-label {
  font-weight: 600;
  color: #333;
  font-size: 14px;
}

.method-radio {
  position: absolute;
  top: 10px;
  right: 10px;
}

.method-radio input {
  transform: scale(1.2);
}

.payment-confirm {
  background: #f8f9fa;
  border: 1px solid #dee2e6;
  border-radius: 8px;
  padding: 20px;
}

.confirm-summary {
  margin-bottom: 20px;
}

.confirm-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
  font-size: 16px;
}

.submit-button {
  width: 100%;
  padding: 15px;
  background: #28a745;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  font-weight: bold;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  transition: background 0.2s;
}

.submit-button:hover:not(:disabled) {
  background: #1e7e34;
}

.submit-button:disabled {
  background: #6c757d;
  cursor: not-allowed;
}

.no-data {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 300px;
  color: #666;
  gap: 15px;
}

.no-data i {
  font-size: 60px;
  color: #ccc;
}

@media (max-width: 1200px) {
  .payment-container {
    grid-template-columns: 1fr;
    gap: 20px;
  }
  
  .payment-methods {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .payment-container {
    padding: 0 15px 15px;
  }
  
  .panel-body {
    padding: 15px;
  }
  
  .datetime-input-group {
    flex-direction: column;
  }
  
  .btn-now {
    align-self: flex-start;
  }
  
  .confirm-row {
    flex-direction: column;
    align-items: flex-start;
    gap: 5px;
  }
}
</style>