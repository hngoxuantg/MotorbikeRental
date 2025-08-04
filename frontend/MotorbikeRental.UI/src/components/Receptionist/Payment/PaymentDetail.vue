<script setup>
import { computed } from 'vue'

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

const emit = defineEmits(['goBack', 'goIncident'])

const paymentStatusInfo = computed(() => {
  if (!props.payment) return {}
  
  const statusMap = {
    0: { text: 'Tiền mặt', color: '#28a745', bgColor: '#d4edda', icon: 'fa-money-bill-alt' },
    1: { text: 'Chuyển khoản', color: '#007bff', bgColor: '#d1ecf1', icon: 'fa-credit-card' },
    2: { text: 'Ví điện tử', color: '#fd7e14', bgColor: '#ffeaa7', icon: 'fa-mobile-alt' }
  }
  
  return statusMap[props.payment.paymentStatus] || { 
    text: 'Không xác định', 
    color: '#6c757d', 
    bgColor: '#f8f9fa', 
    icon: 'fa-question-circle' 
  }
})

const formatPrice = (price) => {
  if (!price) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN').format(price) + ' VNĐ'
}

const formatDate = (dateString) => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}
</script>

<template>
  <div class="payment-detail-page">
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner">
        <i class="fas fa-spinner fa-spin"></i>
        <span>Đang tải thông tin thanh toán...</span>
      </div>
    </div>

    <div v-else-if="payment" class="payment-content">
      <div class="page-header">
        <div class="header-left">
          <button @click="$emit('goBack')" class="back-button">
            <i class="fas fa-arrow-left"></i>
            Quay lại
          </button>
          <h2 class="page-title">
            <i class="fas fa-receipt"></i>
            Chi tiết thanh toán #{{ payment.paymentId }}
          </h2>
        </div>
        
        <div class="header-actions">
          <div class="payment-status" :style="{ 
            color: paymentStatusInfo.color, 
            backgroundColor: paymentStatusInfo.bgColor 
          }">
            <i :class="['fas', paymentStatusInfo.icon]"></i>
            {{ paymentStatusInfo.text }}
          </div>
          
          <button 
            v-if="payment.isIncident"
            @click="$emit('goIncident')"
            class="incident-button"
          >
            <i class="fas fa-exclamation-triangle"></i>
            Xem sự cố
          </button>
        </div>
      </div>

      <div v-if="payment.isIncident" class="incident-notice">
        <div class="notice-content">
          <i class="fas fa-exclamation-triangle"></i>
          <div class="notice-text">
            <h4>Thanh toán có sự cố</h4>
            <p>Thanh toán này bao gồm các khoản phí liên quan đến sự cố xảy ra trong quá trình thuê xe.</p>
          </div>
        </div>
      </div>

      <div class="page-content">
        <div class="info-section">
          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-file-invoice"></i>
              <span>Thông tin thanh toán</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr>
                  <td class="label">Mã thanh toán:</td>
                  <td class="value payment-id">#{{ payment.paymentId }}</td>
                </tr>
                <tr>
                  <td class="label">Hợp đồng liên quan:</td>
                  <td class="value contract-id">#{{ payment.contractId }}</td>
                </tr>
                <tr>
                  <td class="label">Ngày thanh toán:</td>
                  <td class="value">{{ formatDate(payment.paymentDate) }}</td>
                </tr>
                <tr>
                  <td class="label">Phương thức:</td>
                  <td class="value">
                    <span class="payment-method" :style="{ color: paymentStatusInfo.color }">
                      <i :class="['fas', paymentStatusInfo.icon]"></i>
                      {{ paymentStatusInfo.text }}
                    </span>
                  </td>
                </tr>
                <tr>
                  <td class="label">Có sự cố:</td>
                  <td class="value">
                    <span :class="['incident-status', payment.isIncident ? 'has-incident' : 'no-incident']">
                      <i :class="['fas', payment.isIncident ? 'fa-exclamation-triangle' : 'fa-check-circle']"></i>
                      {{ payment.isIncident ? 'Có' : 'Không' }}
                    </span>
                  </td>
                </tr>
              </table>
            </div>
          </div>

          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-user"></i>
              <span>Thông tin khách hàng</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr>
                  <td class="label">Họ tên:</td>
                  <td class="value">{{ payment.customerName }}</td>
                </tr>
              </table>
            </div>
          </div>

          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-user-tie"></i>
              <span>Nhân viên xử lý</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr>
                  <td class="label">Mã nhân viên:</td>
                  <td class="value">#{{ payment.employeeId }}</td>
                </tr>
                <tr>
                  <td class="label">Họ tên:</td>
                  <td class="value">{{ payment.employeeName }}</td>
                </tr>
              </table>
            </div>
          </div>
        </div>

        <div class="financial-section">
          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-money-bill-wave"></i>
              <span>Chi tiết tài chính</span>
            </div>
            <div class="panel-body">
              <div class="financial-breakdown">
                <div v-if="payment.contractIndemnity" class="financial-row">
                  <span class="label">Tiền bồi thường hợp đồng:</span>
                  <span class="value indemnity">{{ formatPrice(payment.contractIndemnity) }}</span>
                </div>
                
                <div v-if="payment.incidentFineAmount" class="financial-row">
                  <span class="label">Phí phạt sự cố:</span>
                  <span class="value fine">{{ formatPrice(payment.incidentFineAmount) }}</span>
                </div>
                
                <div class="financial-row total">
                  <span class="label">Tổng số tiền đã thanh toán:</span>
                  <span class="value">{{ formatPrice(payment.amount) }}</span>
                </div>
              </div>

              <div class="payment-summary">
                <div class="summary-card">
                  <div class="summary-icon">
                    <i :class="['fas', paymentStatusInfo.icon]"></i>
                  </div>
                  <div class="summary-content">
                    <h3>{{ formatPrice(payment.amount) }}</h3>
                    <p>{{ paymentStatusInfo.text }}</p>
                    <span class="payment-time">{{ formatDate(payment.paymentDate) }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="no-data">
      <i class="fas fa-receipt"></i>
      <p>Không tìm thấy thông tin thanh toán</p>
    </div>
  </div>
</template>

<style scoped>
.payment-detail-page {
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
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 20px;
}

.back-button {
  padding: 10px 16px;
  background: #6c757d;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
  transition: background 0.2s;
}

.back-button:hover {
  background: #5a6268;
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

.header-actions {
  display: flex;
  gap: 15px;
  align-items: center;
  flex-wrap: wrap;
}

.payment-status {
  padding: 8px 16px;
  border-radius: 20px;
  font-weight: 600;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 8px;
}

.incident-button {
  padding: 10px 16px;
  background: #dc3545;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
  transition: background 0.2s;
}

.incident-button:hover {
  background: #c82333;
}

.incident-notice {
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  border-left: 4px solid #dc3545;
  margin: 0 20px 20px 20px;
  border-radius: 4px;
}

.notice-content {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  padding: 15px 20px;
}

.notice-content i {
  color: #dc3545;
  font-size: 24px;
  margin-top: 2px;
}

.notice-text h4 {
  color: #721c24;
  margin: 0 0 5px 0;
  font-size: 16px;
}

.notice-text p {
  color: #721c24;
  margin: 0;
  line-height: 1.4;
  font-size: 14px;
}

.page-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  padding: 20px;
}

.info-panel {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  margin-bottom: 20px;
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

.info-table {
  width: 100%;
  border-collapse: collapse;
}

.info-table td {
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
  vertical-align: top;
}

.info-table td.label {
  width: 150px;
  color: #666;
  font-weight: 500;
}

.info-table td.value {
  color: #333;
  font-weight: 600;
}

.payment-id, .contract-id {
  color: #007bff;
  font-family: monospace;
  font-size: 16px;
}

.payment-method {
  display: flex;
  align-items: center;
  gap: 6px;
  font-weight: 600;
}

.incident-status {
  display: flex;
  align-items: center;
  gap: 6px;
  font-weight: 600;
}

.incident-status.has-incident {
  color: #dc3545;
}

.incident-status.no-incident {
  color: #28a745;
}

.financial-breakdown {
  display: flex;
  flex-direction: column;
  gap: 15px;
  margin-bottom: 30px;
}

.financial-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
}

.financial-row.total {
  border-top: 2px solid #28a745;
  border-bottom: none;
  margin-top: 15px;
  padding-top: 20px;
  font-size: 18px;
  font-weight: bold;
  color: #28a745;
}

.financial-row .label {
  color: #666;
  font-weight: 500;
}

.financial-row .value {
  font-weight: 600;
  font-family: monospace;
}

.financial-row .value.indemnity,
.financial-row .value.fine {
  color: #dc3545;
}

.payment-summary {
  border-top: 1px solid #e9ecef;
  padding-top: 20px;
}

.summary-card {
  background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
  color: white;
  border-radius: 12px;
  padding: 25px;
  display: flex;
  align-items: center;
  gap: 20px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.summary-icon {
  font-size: 36px;
  opacity: 0.9;
}

.summary-content h3 {
  margin: 0 0 5px 0;
  font-size: 24px;
  font-weight: bold;
}

.summary-content p {
  margin: 0 0 5px 0;
  font-size: 16px;
  opacity: 0.9;
}

.payment-time {
  font-size: 14px;
  opacity: 0.8;
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
  .page-content {
    grid-template-columns: 1fr;
  }
  
  .header-actions {
    flex-wrap: wrap;
  }
}

@media (max-width: 768px) {
  .page-header {
    flex-direction: column;
    gap: 15px;
    align-items: flex-start;
  }
  
  .header-left {
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }
  
  .page-content {
    padding: 15px;
    gap: 15px;
  }
  
  .panel-body {
    padding: 15px;
  }
  
  .summary-card {
    flex-direction: column;
    text-align: center;
    gap: 15px;
  }
  
  .financial-row {
    flex-direction: column;
    align-items: flex-start;
    gap: 5px;
  }
  
  .incident-button {
    width: 100%;
    justify-content: center;
  }
}
</style>