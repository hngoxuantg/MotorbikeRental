<script setup>
import { ref, onMounted } from 'vue'
import { motorbikeService } from '@/api/services/motorbikeService'
import { categoryService } from '@/api/services/categoryService'
import { useRouter } from 'vue-router'
import { getFullPath } from '@/utils/UrlUtils'

const props = defineProps({
  motorbike: {
    type: Object,
    required: true,
  },
})


const conditionOptions = [
  { value: 0, label: 'Mới' },
  { value: 1, label: 'Như mới' },
  { value: 2, label: 'Tốt' },
  { value: 3, label: 'Khá' },
  { value: 4, label: 'Trung bình' },
]

const router = useRouter()
const form = ref({
  ...props.motorbike,
  formFile: null,
})
const isSaving = ref(false)
const categories = ref([])

onMounted(async () => {
  try {
    const res = await categoryService.getAll()
    categories.value = res.data || res
  } catch (e) {
    categories.value = []
  }
})

function handleFileChange(e) {
  form.value.formFile = e.target.files[0]
}

async function onSubmit() {
  isSaving.value = true
  try {
    const res = await motorbikeService.updateMotorbike(form.value)
    if (res?.success) {
      alert('Cập nhật thành công!')
      router.push('/admin/motorbike/detail/' + form.value.motorbikeId)
    } else {
      alert(res?.message || 'Cập nhật thất bại!')
    }
  } catch (e) {
    alert('Có lỗi xảy ra!')
  } finally {
    isSaving.value = false
  }
}

function goBack() {
  router.push('/admin/motorbike/detail/' + form.value.motorbikeId)
}
</script>

<template>
  <div class="edit-motorbike-container">
    <div class="header">
      <div class="header-content">
        <div class="header-info">
          <h1>Chỉnh sửa xe máy</h1>
          <p>Cập nhật thông tin xe máy {{ form.motorbikeName }}</p>
        </div>
        <div class="header-actions">
          <button type="button" @click="goBack" class="btn-back">
            ← Quay lại
          </button>
        </div>
      </div>
    </div>

    <form @submit.prevent="onSubmit" class="edit-form">
      <div class="form-card">
        <h3>Thông tin cơ bản</h3>
        <div class="form-grid">
          <div class="form-group">
            <label>Tên xe *</label>
            <input v-model="form.motorbikeName" required placeholder="Nhập tên xe" />
          </div>

          <div class="form-group">
            <label>Loại xe *</label>
            <select v-model="form.categoryId" required>
              <option value="" disabled>Chọn loại xe</option>
              <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
                {{ cat.categoryName }}
              </option>
            </select>
          </div>

          <div class="form-group">
            <label>Biển số *</label>
            <input v-model="form.licensePlate" required placeholder="Nhập biển số" />
          </div>

          <div class="form-group">
            <label>Thương hiệu *</label>
            <input v-model="form.brand" required placeholder="Nhập thương hiệu" />
          </div>

          <div class="form-group">
            <label>Năm sản xuất *</label>
            <input v-model="form.year" type="number" required placeholder="Nhập năm sản xuất" />
          </div>

          <div class="form-group">
            <label>Màu sắc *</label>
            <input v-model="form.color" required placeholder="Nhập màu sắc" />
          </div>
        </div>
      </div>

      <div class="form-card">
        <h3>Thông tin giá cả</h3>
        <div class="form-grid">
          <div class="form-group">
            <label>Giá theo giờ (VNĐ) *</label>
            <input v-model="form.hourlyRate" type="number" required placeholder="Nhập giá theo giờ" />
          </div>

          <div class="form-group">
            <label>Giá theo ngày (VNĐ) *</label>
            <input v-model="form.dailyRate" type="number" required placeholder="Nhập giá theo ngày" />
          </div>
        </div>
      </div>

      <div class="form-card">
        <h3>Thông tin kỹ thuật</h3>
        <div class="form-grid">
          <div class="form-group">
            <label>Dung tích động cơ (cc) *</label>
            <input v-model="form.engineCapacity" type="number" required placeholder="Nhập dung tích" />
          </div>

          <div class="form-group">
            <label>Số khung *</label>
            <input v-model="form.chassisNumber" required placeholder="Nhập số khung" />
          </div>

          <div class="form-group">
            <label>Số máy *</label>
            <input v-model="form.engineNumber" required placeholder="Nhập số máy" />
          </div>

          <div class="form-group">
            <label>Số km đã đi</label>
            <input v-model="form.mileage" type="number" placeholder="Nhập số km" />
          </div>
        </div>

        <div class="form-group full-width">
          <label>Mô tả</label>
          <textarea v-model="form.description" rows="4" placeholder="Nhập mô tả chi tiết về xe"></textarea>
        </div>
      </div>

      <div class="form-card">
        <h3>Tình trạng và trạng thái</h3>
        <div class="form-grid">
          <div class="form-group">
            <label>Tình trạng xe *</label>
            <select v-model="form.motorbikeConditionStatus" required>
              <option value="" disabled>Chọn tình trạng</option>
              <option v-for="option in conditionOptions" :key="option.value" :value="option.value">
                {{ option.label }}
              </option>
            </select>
          </div>
        </div>
      </div>

      <div class="form-card">
        <h3>Hình ảnh xe</h3>
        <div class="image-section">
          <div class="image-upload">
            <label>Chọn ảnh mới (nếu muốn thay đổi)</label>
            <input type="file" @change="handleFileChange" accept="image/*" />
          </div>
          
          <div v-if="form.imageUrl" class="current-image">
            <p>Ảnh hiện tại:</p>
            <img :src="getFullPath(form.imageUrl)" alt="Ảnh xe" />
          </div>
        </div>
      </div>

      <div class="form-actions">
        <button type="button" @click="goBack" class="btn-cancel">
          Hủy bỏ
        </button>
        <button type="submit" :disabled="isSaving" class="btn-save">
          {{ isSaving ? 'Đang lưu...' : 'Lưu thay đổi' }}
        </button>
      </div>
    </form>
  </div>
</template>

<style scoped>
.edit-motorbike-container {
  padding: 20px;
}

.header {
  background: white;
  padding: 20px;
  border-radius: 8px;
  margin-bottom: 20px;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-info h1 {
  margin: 0 0 8px 0;
  font-size: 24px;
  color: #333;
}

.header-info p {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.btn-back {
  background: #f8f9fa;
  border: 1px solid #ddd;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  color: #666;
  font-size: 14px;
}

.btn-back:hover {
  background: #e9ecef;
}

.edit-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.form-card {
  background: white;
  border-radius: 8px;
  padding: 20px;
}

.form-card h3 {
  margin: 0 0 20px 0;
  font-size: 18px;
  color: #333;
  padding-bottom: 8px;
  border-bottom: 1px solid #eee;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group.full-width {
  grid-column: 1 / -1;
}

.form-group label {
  font-weight: 500;
  color: #333;
  font-size: 14px;
}

.form-group input,
.form-group select,
.form-group textarea {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #007bff;
}

.form-group textarea {
  resize: vertical;
  min-height: 80px;
}

.image-section {
  display: grid;
  grid-template-columns: 1fr 300px;
  gap: 20px;
  align-items: start;
}

.image-upload label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #333;
  font-size: 14px;
}

.image-upload input {
  padding: 8px;
  border: 1px dashed #ddd;
  border-radius: 4px;
  width: 100%;
  cursor: pointer;
}

.current-image p {
  margin: 0 0 8px 0;
  font-weight: 500;
  color: #333;
  font-size: 14px;
}

.current-image img {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
}

.btn-cancel,
.btn-save {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
}

.btn-cancel {
  background: #6c757d;
  color: white;
}

.btn-cancel:hover {
  background: #5a6268;
}

.btn-save {
  background: #007bff;
  color: white;
}

.btn-save:hover:not(:disabled) {
  background: #0056b3;
}

.btn-save:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

@media (max-width: 768px) {
  .edit-motorbike-container {
    padding: 10px;
  }

  .header-content {
    flex-direction: column;
    gap: 16px;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .image-section {
    grid-template-columns: 1fr;
  }

  .form-actions {
    flex-direction: column;
  }
}
</style>