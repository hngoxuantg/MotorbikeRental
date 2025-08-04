<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

const { motorbike } = defineProps({
  motorbike: {
    type: Object,
    required: true,
  },
})

const emit = defineEmits(['delete'])

function goToEdit() {
  router.push(`/admin/motorbike/edit/${motorbike.motorbikeId}`)
}

function onDelete() {
  if (confirm('Bạn có chắc chắn muốn xóa xe này?')) {
    emit('delete', motorbike)
  }
}

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN').format(price)
}

function getStatusText(status) {
  const statusMap = {
    0: 'Có sẵn',
    1: 'Đang thuê',
    2: 'Bảo trì',
    3: 'Đã đặt trước',
    4: 'Không hoạt động',
    5: 'Đã hư hỏng'
  }
  return statusMap[status] || 'Không xác định'
}

function getConditionText(condition) {
  const conditionMap = {
    0: 'Mới',
    1: 'Như mới',
    2: 'Đã qua sử dụng (tốt)',
    3: 'Đã qua sử dụng (khá)'
  }
  return conditionMap[condition] || 'Không xác định'
}

function getStatusClass(status) {
  const classMap = {
    0: 'status-available',
    1: 'status-rented',
    2: 'status-maintenance',
    3: 'status-booked',
    4: 'status-inactive',
    5: 'status-broken'
  }
  return classMap[status] || 'status-unknown'
}
</script>

<template>
  <div class="motorbike-detail">
    <div class="content">
      <div class="image-section">
        <img
          v-if="motorbike.imageUrl"
          :src="motorbike.imageUrl.startsWith('/') ? 'https://localhost:7060' + motorbike.imageUrl : motorbike.imageUrl"
          :alt="motorbike.motorbikeName"
          class="motorbike-image"
        />
        <div v-else class="no-image">
          <span>Chưa có ảnh</span>
        </div>
      </div>

      <div class="info-section">
        <div class="info-card">
          <h3>Thông tin cơ bản</h3>
          <div class="info-grid">
            <div class="info-item">
              <span class="label">Loại xe:</span>
              <span class="value">{{ motorbike.categoryName }}</span>
            </div>
            <div class="info-item">
              <span class="label">Biển số:</span>
              <span class="value license-plate">{{ motorbike.licensePlate }}</span>
            </div>
            <div class="info-item">
              <span class="label">Thương hiệu:</span>
              <span class="value">{{ motorbike.brand }}</span>
            </div>
            <div class="info-item">
              <span class="label">Năm sản xuất:</span>
              <span class="value">{{ motorbike.year }}</span>
            </div>
            <div class="info-item">
              <span class="label">Màu sắc:</span>
              <span class="value">{{ motorbike.color }}</span>
            </div>
            <div class="info-item">
              <span class="label">Dung tích:</span>
              <span class="value">{{ motorbike.engineCapacity }} cc</span>
            </div>
          </div>
        </div>

        <div class="info-card">
          <h3>Thông tin kỹ thuật</h3>
          <div class="info-grid">
            <div class="info-item">
              <span class="label">Số khung:</span>
              <span class="value code">{{ motorbike.chassisNumber }}</span>
            </div>
            <div class="info-item">
              <span class="label">Số máy:</span>
              <span class="value code">{{ motorbike.engineNumber }}</span>
            </div>
            <div class="info-item">
              <span class="label">Tình trạng:</span>
              <span class="value">{{ getConditionText(motorbike.motorbikeConditionStatus) }}</span>
            </div>
            <div class="info-item">
              <span class="label">Số km đã đi:</span>
              <span class="value">{{ motorbike.mileage ? formatPrice(motorbike.mileage) + ' km' : 'Chưa cập nhật' }}</span>
            </div>
          </div>
        </div>

        <div class="info-card pricing-card">
          <h3>Bảng giá thuê</h3>
          <div class="pricing-grid">
            <div class="price-item">
              <span class="price-label">Giá theo giờ</span>
              <span class="price-value">{{ formatPrice(motorbike.hourlyRate) }} VNĐ</span>
            </div>
            <div class="price-item">
              <span class="price-label">Giá theo ngày</span>
              <span class="price-value">{{ formatPrice(motorbike.dailyRate) }} VNĐ</span>
            </div>
          </div>
        </div>

        <div class="info-card" v-if="motorbike.description">
          <h3>Mô tả</h3>
          <p class="description">{{ motorbike.description }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.motorbike-detail {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
  padding: 20px;
  background: white;
  border-radius: 8px;
}

.title-section {
  display: flex;
  align-items: center;
  gap: 16px;
}

.title-section h1 {
  margin: 0;
  font-size: 24px;
  color: #333;
}

.status-badge {
  padding: 4px 12px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 500;
}

.status-available {
  background: #d4edda;
  color: #155724;
}

.status-rented {
  background: #fff3cd;
  color: #856404;
}

.status-maintenance {
  background: #cce5ff;
  color: #004085;
}

.status-booked {
  background: #e7e3ff;
  color: #5a46d1;
}

.status-inactive {
  background: #e2e3e5;
  color: #383d41;
}

.status-broken {
  background: #f8d7da;
  color: #721c24;
}

.actions {
  display: flex;
  gap: 12px;
}

.btn-edit,
.btn-delete {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
}

.btn-edit {
  background: #007bff;
  color: white;
}

.btn-edit:hover {
  background: #0056b3;
}

.btn-delete {
  background: #dc3545;
  color: white;
}

.btn-delete:hover {
  background: #c82333;
}

.content {
  display: grid;
  grid-template-columns: 400px 1fr;
  gap: 24px;
}

.image-section {
  position: sticky;
  top: 20px;
  height: fit-content;
}

.motorbike-image {
  width: 100%;
  height: 300px;
  object-fit: cover;
  border-radius: 8px;
  background: white;
}

.no-image {
  height: 300px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f8f9fa;
  border-radius: 8px;
  color: #6c757d;
}

.info-section {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.info-card {
  background: white;
  border-radius: 8px;
  padding: 20px;
}

.info-card h3 {
  margin: 0 0 16px 0;
  font-size: 18px;
  color: #333;
  border-bottom: 1px solid #eee;
  padding-bottom: 8px;
}

.info-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 16px;
}

.info-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 0;
  border-bottom: 1px solid #f8f9fa;
}

.label {
  color: #666;
  font-size: 14px;
}

.value {
  color: #333;
  font-weight: 500;
  font-size: 14px;
}

.value.license-plate {
  background: #007bff;
  color: white;
  padding: 4px 8px;
  border-radius: 4px;
  font-family: monospace;
  font-size: 12px;
  font-weight: 600;
}

.value.code {
  background: #f8f9fa;
  padding: 4px 8px;
  border-radius: 4px;
  font-family: monospace;
  font-size: 12px;
  color: #495057;
}

.pricing-card {
  background: #f8f9fa;
}

.pricing-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;
}

.price-item {
  background: white;
  padding: 16px;
  border-radius: 8px;
  text-align: center;
  border: 1px solid #eee;
}

.price-label {
  display: block;
  color: #666;
  font-size: 14px;
  margin-bottom: 8px;
}

.price-value {
  display: block;
  color: #28a745;
  font-size: 20px;
  font-weight: 600;
}

.description {
  color: #666;
  line-height: 1.5;
  margin: 0;
}

@media (max-width: 768px) {
  .motorbike-detail {
    padding: 10px;
  }

  .header {
    flex-direction: column;
    gap: 16px;
  }

  .content {
    grid-template-columns: 1fr;
  }

  .image-section {
    position: static;
  }

  .info-grid {
    grid-template-columns: 1fr;
  }

  .pricing-grid {
    grid-template-columns: 1fr;
  }

  .info-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 4px;
  }
}
</style>