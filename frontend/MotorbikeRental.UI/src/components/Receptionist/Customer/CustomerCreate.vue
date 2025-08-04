<script setup>
import { ref, reactive } from 'vue'

const emit = defineEmits(['submit'])

// Form data
const form = reactive({
    fullName: '',
    gender: 0,
    idNumber: '',
    phoneNumber: '',
    email: ''
})

// Form validation
const errors = ref({})
const isSubmitting = ref(false)

function validateForm() {
    errors.value = {}
    
    if (!form.fullName.trim()) {
        errors.value.fullName = 'Họ tên là bắt buộc'
    }
    
    if (!form.idNumber.trim()) {
        errors.value.idNumber = 'Số CCCD/CMND là bắt buộc'
    } else if (!/^\d{9,12}$/.test(form.idNumber)) {
        errors.value.idNumber = 'Số CCCD/CMND phải từ 9-12 chữ số'
    }
    
    if (!form.phoneNumber.trim()) {
        errors.value.phoneNumber = 'Số điện thoại là bắt buộc'
    } else if (!/^[0-9]{10,11}$/.test(form.phoneNumber)) {
        errors.value.phoneNumber = 'Số điện thoại không hợp lệ'
    }
    
    if (!form.email.trim()) {
        errors.value.email = 'Email là bắt buộc'
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
        errors.value.email = 'Email không hợp lệ'
    }
    
    return Object.keys(errors.value).length === 0
}

function handleSubmit() {
    if (!validateForm()) return
    
    isSubmitting.value = true
    emit('submit', { ...form })
    
    setTimeout(() => {
        isSubmitting.value = false
    }, 1000)
}

function resetForm() {
    Object.assign(form, {
        fullName: '',
        gender: 0,
        idNumber: '',
        phoneNumber: '',
        email: ''
    })
    errors.value = {}
}
</script>

<template>
    <div class="create-customer-container">
        <!-- Header -->
        <div class="form-header">
            <h1>Tạo khách hàng mới</h1>
            <p>Nhập thông tin khách hàng để tạo tài khoản mới</p>
        </div>

        <!-- Form -->
        <div class="form-content">
            <form @submit.prevent="handleSubmit" class="customer-form">
                <div class="form-grid">
                    <!-- Full Name -->
                    <div class="form-group">
                        <label class="form-label" for="fullName">
                            Họ và tên <span class="required">*</span>
                        </label>
                        <input
                            id="fullName"
                            v-model="form.fullName"
                            type="text"
                            class="form-input"
                            :class="{ 'error': errors.fullName }"
                            placeholder="Nhập họ và tên"
                        />
                        <span v-if="errors.fullName" class="error-message">{{ errors.fullName }}</span>
                    </div>

                    <!-- Gender -->
                    <div class="form-group">
                        <label class="form-label" for="gender">
                            Giới tính <span class="required">*</span>
                        </label>
                        <select
                            id="gender"
                            v-model="form.gender"
                            class="form-select"
                        >
                            <option :value="0">Nam</option>
                            <option :value="1">Nữ</option>
                            <option :value="2">Khác</option>
                        </select>
                    </div>

                    <!-- ID Number -->
                    <div class="form-group">
                        <label class="form-label" for="idNumber">
                            Số CCCD/CMND <span class="required">*</span>
                        </label>
                        <input
                            id="idNumber"
                            v-model="form.idNumber"
                            type="text"
                            class="form-input"
                            :class="{ 'error': errors.idNumber }"
                            placeholder="Nhập số CCCD/CMND"
                        />
                        <span v-if="errors.idNumber" class="error-message">{{ errors.idNumber }}</span>
                    </div>

                    <!-- Phone Number -->
                    <div class="form-group">
                        <label class="form-label" for="phoneNumber">
                            Số điện thoại <span class="required">*</span>
                        </label>
                        <input
                            id="phoneNumber"
                            v-model="form.phoneNumber"
                            type="tel"
                            class="form-input"
                            :class="{ 'error': errors.phoneNumber }"
                            placeholder="Nhập số điện thoại"
                        />
                        <span v-if="errors.phoneNumber" class="error-message">{{ errors.phoneNumber }}</span>
                    </div>

                    <!-- Email -->
                    <div class="form-group full-width">
                        <label class="form-label" for="email">
                            Email <span class="required">*</span>
                        </label>
                        <input
                            id="email"
                            v-model="form.email"
                            type="email"
                            class="form-input"
                            :class="{ 'error': errors.email }"
                            placeholder="Nhập địa chỉ email"
                        />
                        <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
                    </div>
                </div>

                <!-- Form Actions -->
                <div class="form-actions">
                    <button
                        type="button"
                        @click="resetForm"
                        class="btn btn-secondary"
                        :disabled="isSubmitting"
                    >
                        Làm mới
                    </button>
                    <button
                        type="submit"
                        class="btn btn-primary"
                        :disabled="isSubmitting"
                    >
                        {{ isSubmitting ? 'Đang tạo...' : 'Tạo khách hàng' }}
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<style scoped>
.create-customer-container {
    background: #fff;
    border-radius: 8px;
    border: 1px solid #ddd;
    margin: 20px;
    overflow: hidden;
}

.form-header {
    padding: 20px 24px;
    background: white;
    border-bottom: 1px solid #eee;
    text-align: center;
}

.form-header h1 {
    font-size: 24px;
    font-weight: 600;
    margin: 0 0 8px 0;
    color: #333;
}

.form-header p {
    font-size: 14px;
    margin: 0;
    color: #666;
}

.form-content {
    padding: 24px;
}

.customer-form {
    max-width: 600px;
    margin: 0 auto;
}

.form-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 20px;
    margin-bottom: 24px;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.form-group.full-width {
    grid-column: 1 / -1;
}

.form-label {
    font-size: 14px;
    font-weight: 500;
    color: #333;
}

.required {
    color: #dc3545;
}

.form-input,
.form-select {
    padding: 8px 12px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 14px;
}

.form-input:focus,
.form-select:focus {
    outline: none;
    border-color: #007bff;
}

.form-input.error {
    border-color: #dc3545;
}

.error-message {
    font-size: 12px;
    color: #dc3545;
}

.form-actions {
    display: flex;
    justify-content: center;
    gap: 12px;
    padding-top: 20px;
    border-top: 1px solid #eee;
}

.btn {
    padding: 8px 16px;
    border: none;
    border-radius: 4px;
    font-size: 14px;
    cursor: pointer;
    min-width: 120px;
}

.btn:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.btn-primary {
    background: #007bff;
    color: white;
}

.btn-primary:hover:not(:disabled) {
    background: #0056b3;
}

.btn-secondary {
    background: #6c757d;
    color: white;
}

.btn-secondary:hover:not(:disabled) {
    background: #5a6268;
}

@media (max-width: 768px) {
    .create-customer-container {
        margin: 10px;
    }

    .form-grid {
        grid-template-columns: 1fr;
    }

    .form-actions {
        flex-direction: column;
    }
}
</style>