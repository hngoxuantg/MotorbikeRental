<script setup>
import { computed } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'

const props = defineProps({
  incident: {
    type: Object,
    default: null
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const typeInfo = computed(() => {
  if (!props.incident) return {}
  
  const typeMap = {
    0: { text: 'Hư hỏng xe', color: '#dc3545', bgColor: '#f8d7da', icon: 'fa-wrench' },
    1: { text: 'Tai nạn', color: '#fd7e14', bgColor: '#fff3cd', icon: 'fa-car-crash' },
    2: { text: 'Mất trộm', color: '#6f42c1', bgColor: '#e2e3f0', icon: 'fa-user-secret' },
    3: { text: 'Khác', color: '#6c757d', bgColor: '#f8f9fa', icon: 'fa-question-circle' }
  }
  
  return typeMap[props.incident.type] || { 
    text: 'Không xác định', 
    color: '#6c757d', 
    bgColor: '#f8f9fa', 
    icon: 'fa-question-circle' 
  }
})

const severityInfo = computed(() => {
  if (!props.incident) return {}
  
  const severityMap = {
    0: { text: 'Thấp', color: '#28a745', bgColor: '#d4edda', icon: 'fa-info-circle' },
    1: { text: 'Trung bình', color: '#ffc107', bgColor: '#fff3cd', icon: 'fa-exclamation-circle' },
    2: { text: 'Cao', color: '#fd7e14', bgColor: '#ffeaa7', icon: 'fa-exclamation-triangle' },
    3: { text: 'Nghiêm trọng', color: '#dc3545', bgColor: '#f8d7da', icon: 'fa-times-circle' }
  }
  
  return severityMap[props.incident.severity] || { 
    text: 'Không xác định', 
    color: '#6c757d', 
    bgColor: '#f8f9fa', 
    icon: 'fa-question-circle' 
  }
})

const statusInfo = computed(() => {
  if (!props.incident) return {}
  
  return props.incident.isResolved ? {
    text: 'Đã giải quyết',
    color: '#28a745',
    bgColor: '#d4edda',
    icon: 'fa-check-circle'
  } : {
    text: 'Chưa giải quyết',
    color: '#dc3545',
    bgColor: '#f8d7da',
    icon: 'fa-clock'
  }
})

const formatPrice = (price) => {
  if (!price) return 'Chưa xác định'
  return new Intl.NumberFormat('vi-VN').format(price) + ' VNĐ'
}

const formatDate = (dateString) => {
  if (!dateString) return 'Chưa có'
  const date = new Date(dateString)
  return date.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const goBack = () => {
  window.history.back()
}
</script>

<template>
  <div class="incident-detail-page">
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner">
        <i class="fas fa-spinner fa-spin"></i>
        <span>Đang tải thông tin sự cố...</span>
      </div>
    </div>

    <div v-else-if="incident" class="incident-content">
      <div class="page-header">
        <div class="header-left">
          <button @click="goBack" class="back-button">
            <i class="fas fa-arrow-left"></i>
            Quay lại
          </button>
          <h2 class="page-title">
            <i class="fas fa-exclamation-triangle"></i>
            Chi tiết sự cố #{{ incident.incidentId }}
          </h2>
        </div>
        
        <div class="header-status">
          <div class="incident-type" :style="{ 
            color: typeInfo.color, 
            backgroundColor: typeInfo.bgColor 
          }">
            <i :class="['fas', typeInfo.icon]"></i>
            {{ typeInfo.text }}
          </div>
          
          <div class="incident-severity" :style="{ 
            color: severityInfo.color, 
            backgroundColor: severityInfo.bgColor 
          }">
            <i :class="['fas', severityInfo.icon]"></i>
            {{ severityInfo.text }}
          </div>
          
          <div class="incident-status" :style="{ 
            color: statusInfo.color, 
            backgroundColor: statusInfo.bgColor 
          }">
            <i :class="['fas', statusInfo.icon]"></i>
            {{ statusInfo.text }}
          </div>
        </div>
      </div>

      <div v-if="!incident.isResolved" class="pending-notice">
        <div class="notice-content">
          <i class="fas fa-clock"></i>
          <div class="notice-text">
            <h4>Sự cố đang được xử lý</h4>
            <p>Sự cố này chưa được giải quyết. Vui lòng liên hệ bộ phận kỹ thuật để được hỗ trợ.</p>
          </div>
        </div>
      </div>

      <div v-else class="resolved-notice">
        <div class="notice-content">
          <i class="fas fa-check-circle"></i>
          <div class="notice-text">
            <h4>Sự cố đã được giải quyết</h4>
            <p>Sự cố đã được xử lý thành công vào {{ formatDate(incident.resolvedDate) }}</p>
          </div>
        </div>
      </div>

      <div class="page-content">
        <div class="info-section">
          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-info-circle"></i>
              <span>Thông tin sự cố</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr>
                  <td class="label">Mã sự cố:</td>
                  <td class="value incident-id">#{{ incident.incidentId }}</td>
                </tr>
                <tr>
                  <td class="label">Hợp đồng liên quan:</td>
                  <td class="value contract-id">#{{ incident.contractId }}</td>
                </tr>
                <tr>
                  <td class="label">Thời gian xảy ra:</td>
                  <td class="value">{{ formatDate(incident.incidentDate) }}</td>
                </tr>
                <tr>
                  <td class="label">Thời gian báo cáo:</td>
                  <td class="value">{{ formatDate(incident.createdAt) }}</td>
                </tr>
                <tr>
                  <td class="label">Loại sự cố:</td>
                  <td class="value">
                    <span class="type-badge" :style="{ color: typeInfo.color }">
                      <i :class="['fas', typeInfo.icon]"></i>
                      {{ typeInfo.text }}
                    </span>
                  </td>
                </tr>
                <tr>
                  <td class="label">Mức độ nghiêm trọng:</td>
                  <td class="value">
                    <span class="severity-badge" :style="{ color: severityInfo.color }">
                      <i :class="['fas', severityInfo.icon]"></i>
                      {{ severityInfo.text }}
                    </span>
                  </td>
                </tr>
                <tr>
                  <td class="label">Trạng thái:</td>
                  <td class="value">
                    <span class="status-badge" :style="{ color: statusInfo.color }">
                      <i :class="['fas', statusInfo.icon]"></i>
                      {{ statusInfo.text }}
                    </span>
                  </td>
                </tr>
                <tr v-if="incident.resolvedDate">
                  <td class="label">Ngày giải quyết:</td>
                  <td class="value">{{ formatDate(incident.resolvedDate) }}</td>
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
                  <td class="value">{{ incident.customerName }}</td>
                </tr>
              </table>
            </div>
          </div>

          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-user-tie"></i>
              <span>Nhân viên báo cáo</span>
            </div>
            <div class="panel-body">
              <table class="info-table">
                <tr>
                  <td class="label">Họ tên:</td>
                  <td class="value">{{ incident.reportedByEmployeeName }}</td>
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
                      <td class="value">#{{ incident.motorbikeId }}</td>
                    </tr>
                    <tr>
                      <td class="label">Tên xe:</td>
                      <td class="value">{{ incident.motorbikeName }}</td>
                    </tr>
                  </table>
                </div>
                <div class="motorbike-image">
                  <img
                    :src="incident.motorbikeImage ? getFullPath(incident.motorbikeImage) : '/placeholder-bike.jpg'"
                    :alt="incident.motorbikeName"
                  />
                </div>
              </div>
            </div>
          </div>

          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-money-bill-wave"></i>
              <span>Thông tin thiệt hại</span>
            </div>
            <div class="panel-body">
              <div class="damage-info">
                <div class="damage-row">
                  <span class="label">Chi phí sửa chữa:</span>
                  <span class="value damage-cost">{{ formatPrice(incident.damageCost) }}</span>
                </div>
              </div>
            </div>
          </div>

          <div class="info-panel">
            <div class="panel-header">
              <i class="fas fa-file-alt"></i>
              <span>Mô tả chi tiết</span>
            </div>
            <div class="panel-body">
              <div class="description-content">
                <p v-if="incident.description">{{ incident.description }}</p>
                <p v-else class="no-description">Không có mô tả chi tiết</p>
              </div>
              
              <div v-if="incident.notes" class="notes-section">
                <h4>Ghi chú:</h4>
                <p>{{ incident.notes }}</p>
              </div>
            </div>
          </div>

          <div v-if="incident.images && incident.images.length > 0" class="info-panel">
            <div class="panel-header">
              <i class="fas fa-images"></i>
              <span>Hình ảnh sự cố ({{ incident.images.length }})</span>
            </div>
            <div class="panel-body">
              <div class="images-grid">
                <div 
                  v-for="(image, index) in incident.images" 
                  :key="index"
                  class="image-item"
                >
                  <img 
                    :src="getFullPath(image)" 
                    :alt="`Hình ảnh sự cố ${index + 1}`"
                    @click="openImageModal(image)"
                  />
                  <div class="image-overlay">
                    <i class="fas fa-search-plus"></i>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="no-data">
      <i class="fas fa-exclamation-triangle"></i>
      <p>Không tìm thấy thông tin sự cố</p>
    </div>
  </div>
</template>

<style scoped>
.incident-detail-page {
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
  color: #dc3545;
}

.header-status {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.incident-type, .incident-severity, .incident-status {
  padding: 8px 16px;
  border-radius: 20px;
  font-weight: 600;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 6px;
}

.pending-notice {
  background: #fff3cd;
  border: 1px solid #ffeaa7;
  border-left: 4px solid #ffc107;
  margin: 0 20px 20px 20px;
  border-radius: 4px;
}

.resolved-notice {
  background: #d4edda;
  border: 1px solid #c3e6cb;
  border-left: 4px solid #28a745;
  margin: 0 20px 20px 20px;
  border-radius: 4px;
}

.notice-content {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  padding: 15px 20px;
}

.pending-notice .notice-content i {
  color: #856404;
  font-size: 24px;
  margin-top: 2px;
}

.resolved-notice .notice-content i {
  color: #155724;
  font-size: 24px;
  margin-top: 2px;
}

.notice-text h4 {
  margin: 0 0 5px 0;
  font-size: 16px;
}

.pending-notice .notice-text h4 {
  color: #856404;
}

.resolved-notice .notice-text h4 {
  color: #155724;
}

.notice-text p {
  margin: 0;
  line-height: 1.4;
  font-size: 14px;
}

.pending-notice .notice-text p {
  color: #856404;
}

.resolved-notice .notice-text p {
  color: #155724;
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
  color: #dc3545;
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

.incident-id, .contract-id {
  color: #007bff;
  font-family: monospace;
  font-size: 16px;
}

.type-badge, .severity-badge, .status-badge {
  display: flex;
  align-items: center;
  gap: 6px;
  font-weight: 600;
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

.damage-info {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.damage-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
}

.damage-row .label {
  color: #666;
  font-weight: 500;
}

.damage-row .value {
  font-weight: 600;
  font-family: monospace;
}

.damage-cost {
  color: #dc3545;
  font-size: 16px;
}

.description-content {
  line-height: 1.6;
  color: #333;
}

.no-description {
  color: #666;
  font-style: italic;
}

.notes-section {
  margin-top: 20px;
  padding-top: 20px;
  border-top: 1px solid #e9ecef;
}

.notes-section h4 {
  color: #495057;
  margin: 0 0 10px 0;
  font-size: 14px;
}

.images-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
  gap: 15px;
}

.image-item {
  position: relative;
  border-radius: 8px;
  overflow: hidden;
  cursor: pointer;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s;
}

.image-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.image-item img {
  width: 100%;
  height: 120px;
  object-fit: cover;
}

.image-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0;
  transition: opacity 0.2s;
}

.image-item:hover .image-overlay {
  opacity: 1;
}

.image-overlay i {
  color: white;
  font-size: 24px;
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
  
  .header-status {
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
  
  .motorbike-layout {
    grid-template-columns: 1fr;
    text-align: center;
  }
  
  .motorbike-image {
    width: 150px;
    margin: 0 auto;
  }
  
  .images-grid {
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
  }
  
  .damage-row {
    flex-direction: column;
    align-items: flex-start;
    gap: 5px;
  }
}
</style>