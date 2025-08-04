<script setup>
import { ref, computed, watch } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()

const props = defineProps({
  contract: {
    type: Object,
    required: true,
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
})

const emits = defineEmits(['submit', 'goBack', 'goToPayment'])

const settlementData = ref({
  actualReturnDate: '',
  note: '',
})

const contractInfo = computed(() => {
  if (!props.contract) return {}

  const status = props.contract.rentalContractStatus
  const statusMap = {
    0: { text: 'Đang thuê', color: '#28a745', bgColor: '#d4edda' },
    1: { text: 'Đã hoàn thành', color: '#007bff', bgColor: '#d1ecf1' },
    2: { text: 'Đã hủy', color: '#dc3545', bgColor: '#f8d7da' },
    3: { text: 'Đang đặt trước', color: '#ffc107', bgColor: '#fff3cd' },
    4: { text: 'Đang xử lý sự cố', color: '#dc3545', bgColor: '#f8d7da' }, // ✅ Màu đỏ
  }

  return {
    statusInfo: statusMap[status] || {
      text: 'Không xác định',
      color: '#6c757d',
      bgColor: '#f8f9fa',
    },
    rentalTypeText: props.contract.rentalTypeStatus === 0 ? 'Theo giờ' : 'Theo ngày',
    isCompleted: status === 1,
    canSettle: status === 0 || status === 4, // ✅ Cả 2 status đều thanh lý được
    canSettleIncident: status === 4,
  }
})

const fillCurrentDateTime = () => {
  const now = new Date()
  const year = now.getFullYear()
  const month = String(now.getMonth() + 1).padStart(2, '0')
  const day = String(now.getDate()).padStart(2, '0')
  const hours = String(now.getHours()).padStart(2, '0')
  const minutes = String(now.getMinutes()).padStart(2, '0')

  settlementData.value.actualReturnDate = `${year}-${month}-${day}T${hours}:${minutes}`
}

const convertToUTC = (localDateTimeString) => {
  if (!localDateTimeString) return ''
  const localDate = new Date(localDateTimeString)
  return localDate.toISOString()
}

const handleSubmit = () => {
  if (!settlementData.value.actualReturnDate) {
    alert('Vui lòng chọn ngày trả thực tế')
    return
  }

  const submitData = {
    actualReturnDate: convertToUTC(settlementData.value.actualReturnDate),
    note: settlementData.value.note || '',
  }

  emits('submit', submitData)
}

const handleGoBack = () => {
  emits('goBack')
}

function handleGoToPayment() {
  router.push( `/Receptionist/payment/process/${props.contract.contractId}` )
}

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
    minute: '2-digit',
  })
}

const lateReturnInfo = computed(() => {
  if (!props.contract || !props.contract.actualReturnDate || !props.contract.expectedReturnDate) {
    return { isLate: false, lateDuration: 0, lateFee: 0 }
  }

  const expectedDate = new Date(props.contract.expectedReturnDate)
  const actualDate = new Date(props.contract.actualReturnDate)

  const diffMs = actualDate.getTime() - expectedDate.getTime()
  const isLate = diffMs > 0

  if (!isLate) {
    return { isLate: false, lateDuration: 0, lateFee: 0 }
  }

  const diffHours = Math.ceil(diffMs / (1000 * 60 * 60))
  const diffDays = Math.ceil(diffMs / (1000 * 60 * 60 * 24))

  return {
    isLate: true,
    lateDuration: props.contract.rentalTypeStatus === 0 ? diffHours : diffDays,
    lateDurationText:
      props.contract.rentalTypeStatus === 0 ? `${diffHours} giờ` : `${diffDays} ngày`,
    lateFee: props.contract.lateReturnFee || 0,
    multiplier: props.contract.lateReturnFeeMultiplier || 2,
  }
})
</script>

<template>
  <div class="settlement-page">
    <!-- Loading State -->
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner">
        <i class="fas fa-spinner fa-spin"></i>
        <span>Đang xử lý...</span>
      </div>
    </div>

    <!-- Main Content -->
    <div v-else-if="contract" class="settlement-content">
      <!-- Page Header -->
      <div class="page-header">
        <div class="header-left">
          <h2 class="page-title">
            <i class="fas fa-handshake"></i>
            Thanh lý hợp đồng #{{ contract.contractId }}
          </h2>
          <div
            class="contract-status"
            :style="{
              color: contractInfo.statusInfo.color,
              backgroundColor: contractInfo.statusInfo.bgColor,
            }"
          >
            <i class="fas fa-info-circle"></i>
            {{ contractInfo.statusInfo.text }}
          </div>
        </div>
        <button @click="handleGoBack" class="back-button">
          <i class="fas fa-arrow-left"></i>
          Quay về
        </button>
      </div>

      <!-- Settlement Form View (Trước khi thanh lý - Status 0 hoặc 4) -->
      <div v-if="contractInfo.canSettle" class="settlement-form-view">
        <div class="form-container">
          <!-- Incident Notice (chỉ hiện khi status = 4) -->
          <div v-if="contractInfo.canSettleIncident" class="incident-notice">
            <div class="incident-alert">
              <i class="fas fa-exclamation-triangle"></i>
              <div class="alert-content">
                <h4>Thanh lý hợp đồng có sự cố</h4>
                <p>
                  Hợp đồng này đang xử lý sự cố. Vui lòng kiểm tra kỹ tình trạng xe và ghi chú đầy
                  đủ thông tin trước khi thanh lý.
                </p>
              </div>
            </div>
          </div>

          <!-- Basic Contract Info -->
          <div class="basic-info">
            <h3>
              <i class="fas fa-info-circle"></i>
              Hợp đồng: {{ contract.customerFullName }} - {{ contract.motorbikeName }} ({{
                contract.motorbikeLicensePlate
              }})
            </h3>
            <div class="rental-period">
              <span><strong>Ngày thuê:</strong> {{ formatDate(contract.rentalDate) }}</span>
              <span
                ><strong>Dự kiến trả:</strong> {{ formatDate(contract.expectedReturnDate) }}</span
              >
              <span
                ><strong>Trạng thái:</strong>
                <span :style="{ color: contractInfo.statusInfo.color }">
                  {{ contractInfo.statusInfo.text }}
                </span>
              </span>
            </div>
          </div>

          <!-- Settlement Form Panel -->
          <div class="settlement-form-panel">
            <div class="panel-header">
              <i class="fas fa-clipboard-check"></i>
              <span>
                {{
                  contractInfo.canSettleIncident
                    ? 'Thanh lý hợp đồng có sự cố'
                    : 'Thông tin thanh lý'
                }}
              </span>
            </div>
            <div class="panel-body">
              <form @submit.prevent="handleSubmit" class="settlement-form">
                <!-- Actual Return Date -->
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-calendar-check"></i>
                    Ngày trả thực tế
                  </label>
                  <div class="datetime-input-group">
                    <input
                      v-model="settlementData.actualReturnDate"
                      type="datetime-local"
                      class="form-input"
                      required
                    />
                    <button type="button" @click="fillCurrentDateTime" class="btn-now">
                      <i class="fas fa-clock"></i>
                      Bây giờ
                    </button>
                  </div>
                </div>

                <!-- Notes -->
                <div class="form-group">
                  <label class="form-label">
                    <i class="fas fa-sticky-note"></i>
                    {{
                      contractInfo.canSettleIncident
                        ? 'Ghi chú về sự cố và thanh lý'
                        : 'Ghi chú thanh lý'
                    }}
                  </label>
                  <textarea
                    v-model="settlementData.note"
                    class="form-input form-textarea"
                    rows="4"
                    :placeholder="
                      contractInfo.canSettleIncident
                        ? 'Ghi chú về sự cố đã xử lý, tình trạng xe sau sự cố, các vấn đề phát sinh...'
                        : 'Ghi chú về tình trạng xe, các vấn đề phát sinh (nếu có)...'
                    "
                  ></textarea>
                </div>

                <!-- Action Buttons -->
                <div class="form-actions">
                  <button
                    type="submit"
                    :disabled="isLoading"
                    class="submit-button"
                    :class="{ 'incident-settle': contractInfo.canSettleIncident }"
                  >
                    <i class="fas fa-check-circle"></i>
                    {{
                      isLoading
                        ? 'Đang xử lý...'
                        : contractInfo.canSettleIncident
                        ? 'Hoàn tất thanh lý (có sự cố)'
                        : 'Hoàn tất thanh lý'
                    }}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>

      <!-- Settlement Result View (Sau khi thanh lý - Status 1) -->
      <div v-else-if="contractInfo.isCompleted" class="settlement-result-view">
        <!-- Result Header -->
        <div class="result-header">
          <div class="success-badge">
            <i class="fas fa-check-circle"></i>
            <span>Thanh lý thành công!</span>
          </div>
          <div class="header-actions">
            <button @click="handleGoToPayment" class="payment-button">
              <i class="fas fa-credit-card"></i>
              Tới trang thanh toán
            </button>
          </div>
        </div>

        <!-- Result Content -->
        <div class="result-content">
          <!-- Left Column - Contract Information -->
          <div class="info-section">
            <!-- Contract Summary -->
            <div class="info-panel">
              <div class="panel-header">
                <i class="fas fa-file-contract"></i>
                <span>Thông tin hợp đồng</span>
              </div>
              <div class="panel-body">
                <table class="info-table">
                  <tr>
                    <td class="label">Mã hợp đồng:</td>
                    <td class="value contract-id">#{{ contract.contractId }}</td>
                  </tr>
                  <tr>
                    <td class="label">Khách hàng:</td>
                    <td class="value">{{ contract.customerFullName }}</td>
                  </tr>
                  <tr>
                    <td class="label">Xe máy:</td>
                    <td class="value">
                      {{ contract.motorbikeName }} - {{ contract.motorbikeLicensePlate }}
                    </td>
                  </tr>
                  <tr>
                    <td class="label">Hình thức thuê:</td>
                    <td class="value">{{ contractInfo.rentalTypeText }}</td>
                  </tr>
                  <tr>
                    <td class="label">Ngày bắt đầu:</td>
                    <td class="value">{{ formatDate(contract.rentalDate) }}</td>
                  </tr>
                  <tr>
                    <td class="label">Ngày dự kiến trả:</td>
                    <td class="value">{{ formatDate(contract.expectedReturnDate) }}</td>
                  </tr>
                  <tr>
                    <td class="label">Ngày trả thực tế:</td>
                    <td class="value actual-return">{{ formatDate(contract.actualReturnDate) }}</td>
                  </tr>
                  <tr v-if="lateReturnInfo.isLate">
                    <td class="label">Trả muộn:</td>
                    <td class="value late-duration">{{ lateReturnInfo.lateDurationText }}</td>
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
                        <td class="value">{{ contract.motorbikeName }}</td>
                      </tr>
                      <tr>
                        <td class="label">Biển số:</td>
                        <td class="value license-plate">{{ contract.motorbikeLicensePlate }}</td>
                      </tr>
                      <tr>
                        <td class="label">Loại xe:</td>
                        <td class="value">{{ contract.categoryName }}</td>
                      </tr>
                    </table>
                  </div>
                  <div class="motorbike-image">
                    <img
                      :src="
                        contract.motorbikeImageUrl
                          ? getFullPath(contract.motorbikeImageUrl)
                          : '/placeholder-bike.jpg'
                      "
                      :alt="contract.motorbikeName"
                    />
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Right Column - Financial Information -->
          <div class="financial-section">
            <!-- Financial Summary -->
            <div class="info-panel">
              <div class="panel-header">
                <i class="fas fa-money-bill-wave"></i>
                <span>Thông tin tài chính</span>
              </div>
              <div class="panel-body">
                <div class="financial-summary">
                  <div class="financial-row">
                    <span class="label">Tiền đặt cọc:</span>
                    <span class="value deposit">{{ formatPrice(contract.depositAmount) }}</span>
                  </div>

                  <div v-if="contract.discountAmount" class="financial-row">
                    <span class="label">Giảm giá ({{ contract.discountName }}):</span>
                    <span class="value discount">-{{ formatPrice(contract.discountAmount) }}</span>
                  </div>

                  <div
                    v-if="lateReturnInfo.isLate && contract.lateReturnFee"
                    class="financial-row late-info"
                  >
                    <span class="label">
                      <i class="fas fa-exclamation-triangle"></i>
                      Phí trả muộn ({{ lateReturnInfo.lateDurationText }}):
                    </span>
                    <span class="value late-fee">{{ formatPrice(contract.lateReturnFee) }}</span>
                  </div>

                  <div class="financial-row contract-total">
                    <span class="label">Tổng tiền hợp đồng:</span>
                    <span class="value">{{ formatPrice(contract.totalAmount) }}</span>
                  </div>
                </div>

                <!-- Payment Notice -->
                <div class="payment-notice">
                  <div class="notice-box">
                    <i class="fas fa-info-circle"></i>
                    <div class="notice-content">
                      <h4>Cần thanh toán</h4>
                      <p>
                        Vui lòng chuyển sang trang thanh toán để tính toán số tiền cuối cùng và hoàn
                        tất giao dịch.
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Settlement Notes -->
            <div class="info-panel" v-if="contract.note">
              <div class="panel-header">
                <i class="fas fa-sticky-note"></i>
                <span>Ghi chú thanh lý</span>
              </div>
              <div class="panel-body">
                <div class="settlement-note">
                  {{ contract.note }}
                </div>
              </div>
            </div>

            <!-- Late Return Warning -->
            <div v-if="lateReturnInfo.isLate" class="late-warning">
              <div class="warning-box">
                <i class="fas fa-clock"></i>
                <div class="warning-content">
                  <h4>Thông báo trả muộn</h4>
                  <p>
                    Khách hàng đã trả xe muộn {{ lateReturnInfo.lateDurationText }} so với dự kiến.
                    <span v-if="contract.lateReturnFee">
                      Phí trả muộn: <strong>{{ formatPrice(contract.lateReturnFee) }}</strong>
                    </span>
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Cannot Settle View (Status không phù hợp) -->
      <div v-else class="cannot-settle-view">
        <div class="warning-container">
          <div class="warning-message">
            <i class="fas fa-exclamation-triangle"></i>
            <h3>Không thể thanh lý hợp đồng</h3>
            <p>
              Hợp đồng này đang ở trạng thái: <strong>{{ contractInfo.statusInfo.text }}</strong>
            </p>
            <p>
              Chỉ có thể thanh lý hợp đồng đang ở trạng thái "Đang thuê" hoặc "Đang xử lý sự cố".
            </p>
            <button @click="handleGoBack" class="back-button">
              <i class="fas fa-arrow-left"></i>
              Quay về danh sách
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- No Data State -->
    <div v-else class="no-data">
      <i class="fas fa-file-alt"></i>
      <p>Không tìm thấy thông tin hợp đồng</p>
      <button @click="handleGoBack" class="back-button">
        <i class="fas fa-arrow-left"></i>
        Quay về danh sách
      </button>
    </div>
  </div>
</template>

<style scoped>
.settlement-page {
  background: #f5f5f5;
  min-height: 100vh;
  font-family: Arial, sans-serif;
}

/* Loading State */
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

/* Header */
.page-header {
  background: #fff;
  border-bottom: 1px solid #ddd;
  padding: 20px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 20px;
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
  color: #fd7e14;
}

.contract-status {
  padding: 6px 12px;
  border-radius: 20px;
  font-weight: 600;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 6px;
}

/* ✅ SETTLEMENT FORM VIEW (Trước khi thanh lý) */
.settlement-form-view {
  padding: 20px;
}

.form-container {
  max-width: 600px;
  margin: 0 auto;
}

.basic-info {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 20px;
}

.basic-info h3 {
  margin: 0 0 15px 0;
  color: #333;
  display: flex;
  align-items: center;
  gap: 10px;
}

.basic-info h3 i {
  color: #007bff;
}

.rental-period {
  display: flex;
  gap: 20px;
  flex-wrap: wrap;
}

.rental-period span {
  color: #666;
  font-size: 14px;
}

.settlement-form-panel {
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
  color: #fd7e14;
}

.panel-body {
  padding: 20px;
}

.settlement-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
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
  color: #fd7e14;
  width: 14px;
}

.datetime-input-group {
  display: flex;
  gap: 10px;
}

.form-input {
  padding: 12px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
  background: #fff;
  flex: 1;
}

.form-input:focus {
  outline: none;
  border-color: #fd7e14;
}

.form-textarea {
  resize: vertical;
  min-height: 100px;
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
  font-size: 14px;
  white-space: nowrap;
}

.btn-now:hover {
  background: #5a6268;
}

.form-actions {
  display: flex;
  gap: 15px;
  margin-top: 10px;
}

.incident-button {
  flex: 1;
  background: #ffc107;
  color: #212529;
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

.incident-button:hover {
  background: #e0a800;
}

.submit-button {
  flex: 2;
  background: #fd7e14;
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
  background: #e55a00;
}

.submit-button:disabled {
  background: #6c757d;
  cursor: not-allowed;
}

/* ✅ SETTLEMENT RESULT VIEW (Sau khi thanh lý) */
.settlement-result-view {
  padding: 20px;
}

.result-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 20px;
}

.success-badge {
  display: flex;
  align-items: center;
  gap: 12px;
  color: #28a745;
  font-size: 18px;
  font-weight: bold;
}

.success-badge i {
  font-size: 24px;
}

/* ✅ THÊM: Header Actions */
.header-actions {
  display: flex;
  gap: 15px;
}

.payment-button {
  background: #28a745;
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
  font-size: 16px;
}

.payment-button:hover {
  background: #218838;
}

.back-button {
  background: #007bff;
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
}

.back-button:hover {
  background: #0056b3;
}

.result-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

/* Info Panels */
.info-panel {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  margin-bottom: 20px;
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
  width: 140px;
  color: #666;
  font-weight: 500;
}

.info-table td.value {
  color: #333;
  font-weight: 600;
}

.contract-id {
  color: #007bff;
  font-family: monospace;
  font-size: 16px;
}

.license-plate {
  color: #dc3545;
  background: #f8f9fa;
  padding: 4px 8px;
  border-radius: 4px;
  font-family: monospace;
}

.actual-return {
  color: #28a745;
  font-weight: bold;
}

.late-duration {
  color: #dc3545;
  font-weight: bold;
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

/* Financial Summary */
.financial-summary {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.financial-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 0;
  border-bottom: 1px solid #f0f0f0;
}

/* ✅ SỬA: Contract Total thay vì Final Total */
.financial-row.contract-total {
  border-top: 2px solid #007bff;
  border-bottom: none;
  margin-top: 15px;
  padding-top: 15px;
  font-size: 16px;
  font-weight: bold;
  color: #007bff;
}

.financial-row.late-info {
  background: #fff3cd;
  border-left: 4px solid #ffc107;
  padding-left: 15px;
  margin: 5px -15px;
  border-radius: 4px;
}

.financial-row .label {
  color: #666;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 6px;
}

.financial-row .value {
  font-weight: 600;
  font-family: monospace;
}

.financial-row .value.deposit {
  color: #fd7e14;
}

.financial-row .value.discount {
  color: #28a745;
}

.financial-row .value.late-fee {
  color: #dc3545;
}

/* ✅ THÊM: Payment Notice */
.payment-notice {
  margin-top: 20px;
  padding-top: 15px;
  border-top: 1px solid #eee;
}

.notice-box {
  background: #e3f2fd;
  border: 1px solid #90caf9;
  border-radius: 8px;
  padding: 15px;
  display: flex;
  gap: 12px;
  align-items: flex-start;
}

.notice-box i {
  color: #1976d2;
  font-size: 20px;
  margin-top: 2px;
}

.notice-content h4 {
  color: #1565c0;
  margin: 0 0 8px 0;
  font-size: 16px;
}

.notice-content p {
  color: #1565c0;
  margin: 0;
  line-height: 1.4;
  font-size: 14px;
}

/* Settlement Note */
.settlement-note {
  background: #f8f9fa;
  border-left: 4px solid #007bff;
  padding: 15px;
  border-radius: 4px;
  color: #333;
  font-style: italic;
}

/* Late Warning */
.late-warning {
  margin: 15px 0;
}

.warning-box {
  background: #fff3cd;
  border: 1px solid #ffeaa7;
  border-radius: 8px;
  padding: 15px;
  display: flex;
  gap: 12px;
  align-items: flex-start;
}

.warning-box i {
  color: #ffc107;
  font-size: 24px;
  margin-top: 2px;
}

.warning-content h4 {
  color: #856404;
  margin: 0 0 8px 0;
  font-size: 16px;
}

.warning-content p {
  color: #856404;
  margin: 0;
  line-height: 1.4;
}

/* Cannot Settle View */
.cannot-settle-view {
  padding: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 400px;
}

.warning-container {
  max-width: 500px;
  text-align: center;
}

.warning-message {
  background: #fff3cd;
  border: 1px solid #ffeaa7;
  border-radius: 8px;
  padding: 40px 30px;
}

.warning-message i {
  font-size: 60px;
  color: #ffc107;
  margin-bottom: 20px;
}

.warning-message h3 {
  color: #856404;
  margin: 0 0 15px 0;
}

.warning-message p {
  color: #856404;
  margin: 10px 0;
}

/* No Data State */
.no-data {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 400px;
  color: #666;
  gap: 20px;
  padding: 20px;
}

.no-data i {
  font-size: 60px;
  color: #ccc;
}

/* Responsive */
@media (max-width: 1200px) {
  .result-content {
    grid-template-columns: 1fr;
  }

  .datetime-input-group {
    flex-direction: column;
  }

  .form-actions {
    flex-direction: column;
  }

  .result-header {
    flex-direction: column;
    gap: 15px;
    text-align: center;
  }

  .header-actions {
    flex-direction: column;
    width: 100%;
  }

  .payment-button,
  .back-button {
    justify-content: center;
  }
}

@media (max-width: 768px) {
  .settlement-form-view,
  .settlement-result-view {
    padding: 15px;
  }

  .basic-info {
    padding: 15px;
  }

  .panel-body {
    padding: 15px;
  }

  .rental-period {
    flex-direction: column;
    gap: 10px;
  }

  .motorbike-layout {
    grid-template-columns: 1fr;
    text-align: center;
  }

  .motorbike-image {
    width: 120px;
    margin: 0 auto;
  }

  .header-actions {
    gap: 10px;
  }
  /* ✅ THÊM: Incident Notice CSS */
  .incident-notice {
    margin-bottom: 20px;
  }

  .incident-alert {
    background: #f8d7da;
    border: 1px solid #f5c6cb;
    border-left: 4px solid #dc3545;
    border-radius: 8px;
    padding: 15px;
    display: flex;
    gap: 12px;
    align-items: flex-start;
  }

  .incident-alert i {
    color: #dc3545;
    font-size: 24px;
    margin-top: 2px;
  }

  .alert-content h4 {
    color: #721c24;
    margin: 0 0 8px 0;
    font-size: 16px;
  }

  .alert-content p {
    color: #721c24;
    margin: 0;
    line-height: 1.4;
    font-size: 14px;
  }

  .submit-button.incident-settle {
    background: #dc3545;
  }

  .submit-button.incident-settle:hover:not(:disabled) {
    background: #c82333;
  }
}
</style>