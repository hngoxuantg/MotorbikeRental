<script setup>
import { computed } from 'vue'
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

const emits = defineEmits([
  'edit',
  'delete',
  'cancel',
  'activate',
  'settle',
  'goToPayment',
  'addIncident',
  'viewInvoice',
])

const statusInfo = computed(() => {
  if (!props.contract) return {}

  const status = props.contract.rentalContractStatus

  const statusMap = {
    0: { text: 'Đang thuê', color: '#28a745', bgColor: '#d4edda', icon: 'fa-check-circle' },
    1: { text: 'Đã trả xe', color: '#007bff', bgColor: '#d1ecf1', icon: 'fa-flag-checkered' },
    2: { text: 'Đã hủy', color: '#dc3545', bgColor: '#f8d7da', icon: 'fa-times-circle' },
    3: { text: 'Đang đặt trước', color: '#ffc107', bgColor: '#fff3cd', icon: 'fa-clock' },
    4: {
      text: 'Đang xử lý sự cố',
      color: '#dc3545',
      bgColor: '#f8d7da',
      icon: 'fa-exclamation-triangle',
    },
  }

  const statusData = statusMap[status] || {
    text: 'Không xác định',
    color: '#6c757d',
    bgColor: '#f8f9fa',
    icon: 'fa-question-circle',
  }

  return {
    ...statusData,
    isProcessingIncident: status === 4,
  }
})

const rentalTypeText = computed(() => {
  if (!props.contract) return ''
  return props.contract.rentalTypeStatus === 0 ? 'Theo giờ' : 'Theo ngày'
})

const showActions = computed(() => {
  if (!props.contract) return {}

  const status = props.contract.rentalContractStatus
  const isPaid = props.contract.isPaid

  if (status === 4) {
    return {
      showCancel: false,
      showActivate: false,
      showSettle: true,
      showPayment: false,
      showAddIncident: false,
      showAnyAction: true,
    }
  }

  return {
    showCancel: status === 3,
    showActivate: status === 3,
    showSettle: status === 0,
    showPayment: status === 1 && !isPaid,
    showAddIncident: status === 0 && !isPaid,
    showAnyAction: status !== 2,
  }
})

const paymentInfo = computed(() => {
  if (!props.contract || props.contract.rentalContractStatus !== 1) return null

  const isPaid = props.contract.isPaid

  return {
    isPaid: isPaid,
    statusText: isPaid ? 'Đã thanh toán' : 'Chưa thanh toán',
    statusColor: isPaid ? '#28a745' : '#dc3545',
    statusBgColor: isPaid ? '#d4edda' : '#f8d7da',
    icon: isPaid ? 'fa-check-circle' : 'fa-exclamation-circle',
  }
})

const incidentInfo = computed(() => {
  if (!props.contract || props.contract.rentalContractStatus !== 4) return null

  return {
    isProcessing: true,
    statusText: 'Đang xử lý sự cố',
    statusColor: '#dc3545',
    statusBgColor: '#f8d7da',
    icon: 'fa-exclamation-triangle',
  }
})

function formatPrice(price) {
  if (!price) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN').format(price) + ' VNĐ'
}

function formatDate(dateString) {
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
function goToPayment() {
  emits('go-to-payment')
}
</script>

<template>
  <div class="contract-detail-page">
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner">
        <i class="fas fa-spinner fa-spin"></i>
        <span>Đang tải thông tin hợp đồng...</span>
      </div>
    </div>

    <div v-else-if="contract" class="contract-detail-content">
      <div class="page-header">
        <div class="header-left">
          <h2 class="page-title">
            <i class="fas fa-file-contract"></i>
            Chi tiết hợp đồng #{{ contract.contractId }}
          </h2>
          <div class="status-group">
            <div
              class="contract-status"
              :style="{
                color: statusInfo.color,
                backgroundColor: statusInfo.bgColor,
              }"
            >
              <i :class="['fas', statusInfo.icon]"></i>
              {{ statusInfo.text }}
            </div>

            <div
              v-if="paymentInfo"
              class="payment-status"
              :style="{
                color: paymentInfo.statusColor,
                backgroundColor: paymentInfo.statusBgColor,
              }"
            >
              <i :class="['fas', paymentInfo.icon]"></i>
              {{ paymentInfo.statusText }}
            </div>
          </div>
        </div>

        <div v-if="showActions.showAnyAction" class="header-actions">
          <button
            v-if="contract.isPaid"
            @click="$emit('viewInvoice')"
            class="action-btn invoice-btn"
          >
            <i class="fas fa-file-invoice"></i>
            Xem hóa đơn
          </button>
        </div>
      </div>

      <div v-if="incidentInfo" class="incident-notice">
        <div class="notice-container">
          <div class="notice-content">
            <i class="fas fa-exclamation-triangle"></i>
            <div class="notice-text">
              <h4>Đang xử lý sự cố</h4>
              <p>
                Hợp đồng này đang trong quá trình xử lý sự cố. Vui lòng thanh lý hợp đồng để hoàn
                tất quá trình xử lý và chuyển sang thanh toán.
              </p>
            </div>
          </div>
        </div>
      </div>

      <div v-if="paymentInfo && !paymentInfo.isPaid" class="payment-notice">
        <div class="notice-container">
          <div class="notice-content">
            <i class="fas fa-exclamation-triangle"></i>
            <div class="notice-text">
              <h4>Cần thanh toán</h4>
              <p>
                Hợp đồng đã được thanh lý thành công nhưng chưa thanh toán. Vui lòng chuyển sang
                trang thanh toán để hoàn tất giao dịch.
              </p>
            </div>
          </div>
        </div>
      </div>

      <div v-if="paymentInfo && paymentInfo.isPaid" class="payment-success">
        <div class="success-container">
          <i class="fas fa-check-circle"></i>
          <div class="success-text">
            <h4>Đã thanh toán thành công</h4>
            <p>Hợp đồng đã được thanh lý và thanh toán đầy đủ.</p>
          </div>
        </div>
      </div>

      <div class="page-content">
        <div class="info-section">
          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-file-alt"></i>
              <span>Thông tin hợp đồng</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr>
                  <td class="label">Mã hợp đồng:</td>
                  <td class="value contract-id">#{{ contract.contractId }}</td>
                </tr>
                <tr>
                  <td class="label">Hình thức thuê:</td>
                  <td class="value">{{ rentalTypeText }}</td>
                </tr>
                <tr>
                  <td class="label">Ngày bắt đầu:</td>
                  <td class="value">{{ formatDate(contract.rentalDate) }}</td>
                </tr>
                <tr>
                  <td class="label">Ngày dự kiến trả:</td>
                  <td class="value">{{ formatDate(contract.expectedReturnDate) }}</td>
                </tr>
                <tr v-if="contract.actualReturnDate">
                  <td class="label">Ngày trả thực tế:</td>
                  <td class="value">{{ formatDate(contract.actualReturnDate) }}</td>
                </tr>
                <tr>
                  <td class="label">Tình trạng:</td>
                  <td class="value">
                    <span
                      class="status-badge"
                      :style="{
                        color: statusInfo.color,
                      }"
                    >
                      <i :class="['fas', statusInfo.icon]"></i>
                      {{ statusInfo.text }}
                    </span>
                  </td>
                </tr>
                <tr v-if="paymentInfo">
                  <td class="label">Thanh toán:</td>
                  <td class="value">
                    <span
                      class="payment-badge"
                      :style="{
                        color: paymentInfo.statusColor,
                      }"
                    >
                      <i :class="['fas', paymentInfo.icon]"></i>
                      {{ paymentInfo.statusText }}
                    </span>
                  </td>
                </tr>
                <tr>
                  <td class="label">Giữ CMND:</td>
                  <td class="value">
                    <span :class="['id-status', contract.idCardHeld ? 'held' : 'not-held']">
                      <i
                        :class="[
                          'fas',
                          contract.idCardHeld ? 'fa-check-circle' : 'fa-times-circle',
                        ]"
                      ></i>
                      {{ contract.idCardHeld ? 'Đã giữ' : 'Chưa giữ' }}
                    </span>
                  </td>
                </tr>
                <tr v-if="contract.note">
                  <td class="label">Ghi chú:</td>
                  <td class="value">{{ contract.note }}</td>
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
                  <td class="label">Mã khách hàng:</td>
                  <td class="value">#{{ contract.customerId }}</td>
                </tr>
                <tr>
                  <td class="label">Họ tên:</td>
                  <td class="value">{{ contract.customerFullName }}</td>
                </tr>
              </table>
            </div>
          </div>

          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-user-tie"></i>
              <span>Nhân viên phụ trách</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr>
                  <td class="label">Mã nhân viên:</td>
                  <td class="value">#{{ contract.employeeId }}</td>
                </tr>
                <tr>
                  <td class="label">Họ tên:</td>
                  <td class="value">{{ contract.employeeFullName }}</td>
                </tr>
              </table>
            </div>
          </div>
        </div>

        <div class="details-section">
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
                      <td class="label">Mã xe:</td>
                      <td class="value">#{{ contract.motorbikeId }}</td>
                    </tr>
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

          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-money-bill-wave"></i>
              <span>Thông tin tài chính</span>
            </div>
            <div class="panel-body">
              <div class="financial-info">
                <div class="financial-row">
                  <span class="label">Tiền đặt cọc:</span>
                  <span class="value deposit">{{ formatPrice(contract.depositAmount) }}</span>
                </div>

                <div v-if="contract.discountName" class="financial-row">
                  <span class="label">Mã giảm giá:</span>
                  <span class="value">{{ contract.discountName }}</span>
                </div>

                <div v-if="contract.discountAmount" class="financial-row">
                  <span class="label">Số tiền giảm:</span>
                  <span class="value discount">-{{ formatPrice(contract.discountAmount) }}</span>
                </div>

                <div v-if="contract.lateReturnFee" class="financial-row">
                  <span class="label">Phí trả muộn:</span>
                  <span class="value late-fee">{{ formatPrice(contract.lateReturnFee) }}</span>
                </div>

                <div class="financial-row total">
                  <span class="label">Tổng tiền hợp đồng:</span>
                  <span class="value">{{ formatPrice(contract.totalAmount) }}</span>
                </div>
              </div>
            </div>
          </div>

          <div
            v-if="contract.lateReturnFee || contract.lateReturnFeeMultiplier > 1"
            class="info-panel"
          >
            <div class="panel-header">
              <i class="fas fa-exclamation-triangle"></i>
              <span>Thông tin phụ phí</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr v-if="contract.lateReturnFeeMultiplier > 1">
                  <td class="label">Hệ số phí trả muộn:</td>
                  <td class="value">{{ contract.lateReturnFeeMultiplier }}x</td>
                </tr>
                <tr v-if="contract.lateReturnFee">
                  <td class="label">Phí trả muộn:</td>
                  <td class="value late-fee">{{ formatPrice(contract.lateReturnFee) }}</td>
                </tr>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="no-data">
      <i class="fas fa-file-alt"></i>
      <p>Không tìm thấy thông tin hợp đồng</p>
    </div>
  </div>
</template>

<style scoped>
.contract-detail-page {
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

.status-group {
  display: flex;
  gap: 10px;
  align-items: center;
  flex-wrap: wrap;
}
.invoice-btn {
  background: #17a2b8;
  color: white;
}

.invoice-btn:hover {
  background: #138496;
}
.contract-status,
.payment-status {
  padding: 6px 12px;
  border-radius: 20px;
  font-weight: 600;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 6px;
}

.header-actions {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.action-btn {
  padding: 10px 16px;
  border: none;
  border-radius: 4px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 6px;
  transition: all 0.2s;
  white-space: nowrap;
}

.activate-btn {
  background: #28a745;
  color: white;
}

.activate-btn:hover {
  background: #1e7e34;
}

.settle-btn {
  background: #fd7e14;
  color: white;
}

.settle-btn:hover {
  background: #e55a00;
}

.settle-btn.incident-settle {
  background: #dc3545;
  color: white;
}

.settle-btn.incident-settle:hover {
  background: #c82333;
}

.payment-btn {
  background: #28a745;
  color: white;
}

.payment-btn:hover {
  background: #1e7e34;
}

.incident-btn {
  background: #ffc107;
  color: #212529;
}

.incident-btn:hover {
  background: #e0a800;
}

.cancel-btn {
  background: #dc3545;
  color: white;
}

.cancel-btn:hover {
  background: #c82333;
}

.incident-notice {
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  border-left: 4px solid #dc3545;
  margin: 0 20px 20px 20px;
  border-radius: 4px;
}

.notice-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 20px;
  gap: 20px;
}

.notice-content {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  flex: 1;
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

.payment-notice {
  background: #fff3cd;
  border: 1px solid #ffeaa7;
  border-left: 4px solid #ffc107;
  margin: 0 20px 20px 20px;
  border-radius: 4px;
}

.notice-payment-btn {
  background: #28a745;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  white-space: nowrap;
}

.notice-payment-btn:hover {
  background: #1e7e34;
}

.payment-success {
  background: #d4edda;
  border: 1px solid #c3e6cb;
  border-left: 4px solid #28a745;
  margin: 0 20px 20px 20px;
  border-radius: 4px;
}

.success-container {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 15px 20px;
}

.success-container i {
  color: #28a745;
  font-size: 32px;
}

.success-text h4 {
  color: #155724;
  margin: 0 0 5px 0;
  font-size: 18px;
}

.success-text p {
  color: #155724;
  margin: 0;
  line-height: 1.4;
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

.info-table {
  width: 100%;
  border-collapse: collapse;
}

.info-table td {
  padding: 10px 0;
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

.status-badge,
.payment-badge {
  padding: 4px 10px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 4px;
}

.id-status {
  display: flex;
  align-items: center;
  gap: 6px;
}

.id-status.held {
  color: #28a745;
}

.id-status.not-held {
  color: #dc3545;
}

.motorbike-layout {
  display: grid;
  grid-template-columns: 1fr auto;
  gap: 15px;
}

.motorbike-image {
  width: 120px;
}

.motorbike-image img {
  width: 100%;
  height: 100px;
  object-fit: cover;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.financial-info {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.financial-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.financial-row.total {
  border-top: 2px solid #28a745;
  border-bottom: none;
  margin-top: 10px;
  padding-top: 15px;
  font-size: 16px;
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

.financial-row .value.deposit {
  color: #fd7e14;
}

.financial-row .value.discount {
  color: #28a745;
}

.financial-row .value.late-fee {
  color: #dc3545;
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

  .status-group {
    flex-direction: column;
    align-items: flex-start;
    gap: 8px;
  }

  .notice-container {
    flex-direction: column;
    gap: 15px;
    align-items: flex-start;
  }

  .success-container {
    flex-direction: column;
    text-align: center;
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
    padding: 12px;
  }

  .motorbike-layout {
    grid-template-columns: 1fr;
    text-align: center;
  }

  .motorbike-image {
    width: 150px;
    margin: 0 auto;
  }

  .action-btn {
    padding: 8px 12px;
    font-size: 14px;
  }

  .notice-payment-btn {
    width: 100%;
    justify-content: center;
  }
}
</style>