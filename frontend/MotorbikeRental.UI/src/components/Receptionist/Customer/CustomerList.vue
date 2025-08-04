<script setup>
import { ref, watch, computed } from 'vue'
import { useRouter } from 'vue-router'
import { contractStorage } from '@/utils/ContractStorageUtils.js'

const router = useRouter()
const props = defineProps({
    customers: {
        type: Array,
        required: true
    },
    isLoading: {
        type: Boolean,
        default: false
    },
    totalCount: {
        type: Number,
        default: 0
    },
    query: {
        type: Object,
        default: () => ({
            Search: '',
            PageNumber: 1,
            PageSize: 12
        })
    }
})

const emit = defineEmits(['update-query'])

const genderOptions = {
    0: 'Nam',
    1: 'Nữ', 
    2: 'Khác'
}

// Search
const searchValue = ref(props.query.Search || '')

// Debounced search
let searchTimeout = null
watch(searchValue, (newVal) => {
    clearTimeout(searchTimeout)
    searchTimeout = setTimeout(() => {
        emit('update-query', { 
            Search: newVal, 
            PageNumber: 1 
        })
    }, 300)
})

// Computed properties
const totalPages = computed(() => Math.ceil(props.totalCount / props.query.PageSize))

// Functions
function changePage(page) {
    if (page >= 1 && page <= totalPages.value) {
        emit('update-query', { PageNumber: page })
    }
}

function changePageSize(size) {
    emit('update-query', { 
        PageSize: parseInt(size), 
        PageNumber: 1 
    })
}

function clearSearch() {
    searchValue.value = ''
}

function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString('vi-VN')
}

function getGenderText(gender) {
    return genderOptions[gender] || 'Không xác định'
}

function getGenderBadgeClass(gender) {
    switch(gender) {
        case 0: return 'badge-male'
        case 1: return 'badge-female'
        default: return 'badge-other'
    }
}

function goToCreateCustomer() {
    router.push('/receptionist/customer/create')
}

function goToCreateContract(customerId) {
    contractStorage.clear()
    contractStorage.setCustomer(customerId)
    router.push(`/receptionist/contract/select-motorbike`)
}
</script>

<template>
    <div class="customer-table-container">
        <!-- Header -->
        <div class="page-header">
            <h1>Danh sách khách hàng</h1>
            <div class="header-actions">
                <button class="btn-create" @click="goToCreateCustomer">Tạo khách hàng</button>
                <span class="total-count">{{ totalCount }} khách hàng</span>
            </div>
        </div>

        <!-- Search Section -->
        <div class="search-section">
            <div class="search-wrapper">
                <div class="search-input-wrapper">
                    <input
                        v-model="searchValue"
                        class="search-input"
                        placeholder="Tìm kiếm khách hàng theo tên hoặc số điện thoại..."
                        type="text"
                    />
                    <button 
                        v-if="searchValue" 
                        @click="clearSearch" 
                        class="clear-btn"
                    >
                        ×
                    </button>
                </div>
                <div class="search-info">
                    <span v-if="searchValue">
                        Tìm kiếm: "{{ searchValue }}" - {{ totalCount }} kết quả
                    </span>
                </div>
            </div>

            <div class="page-size-control">
                <label>Hiển thị:</label>
                <select 
                    :value="props.query.PageSize" 
                    @change="changePageSize($event.target.value)"
                    class="page-size-select"
                >
                    <option :value="6">6</option>
                    <option :value="12">12</option>
                    <option :value="24">24</option>
                    <option :value="50">50</option>
                </select>
                <span>/ trang</span>
            </div>
        </div>

        <!-- Table Info -->
        <div class="table-info" v-if="!isLoading && totalCount > 0">
            Hiển thị {{ (props.query.PageNumber - 1) * props.query.PageSize + 1 }} - 
            {{ Math.min(props.query.PageNumber * props.query.PageSize, totalCount) }} 
            trong {{ totalCount }} khách hàng
        </div>

        <!-- Content -->
        <div class="content">
            <!-- Loading State -->
            <div v-if="isLoading" class="loading">
                <p>Đang tải dữ liệu...</p>
            </div>

            <!-- Table -->
            <div v-else-if="customers && customers.length > 0" class="table-container">
                <table class="table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Họ tên</th>
                            <th>Giới tính</th>
                            <th>Số điện thoại</th>
                            <th>Lượt thuê</th>
                            <th>Ngày tạo</th>
                            <th>Thao tác</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="customer in customers" :key="customer.customerId">
                            <td>
                                <span class="customer-id">#{{ customer.customerId }}</span>
                            </td>
                            <td>
                                <span class="customer-name">{{ customer.fullName }}</span>
                            </td>
                            <td>
                                <span class="gender-badge" :class="getGenderBadgeClass(customer.gender)">
                                    {{ getGenderText(customer.gender) }}
                                </span>
                            </td>
                            <td>
                                <a :href="`tel:${customer.phoneNumber}`" class="phone-link">
                                    {{ customer.phoneNumber }}
                                </a>
                            </td>
                            <td>
                                <span class="rental-count">{{ customer.rentalCount }}</span>
                            </td>
                            <td>
                                <span class="date-text">{{ formatDate(customer.createAt) }}</span>
                            </td>
                            <td>
                                <button @click="goToCreateContract(customer.customerId)" class="btn-createContract">Tạo hợp đồng</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            
            <!-- Empty State -->
            <div v-else class="empty-state">
                <h3>{{ searchValue ? 'Không tìm thấy kết quả' : 'Chưa có khách hàng nào' }}</h3>
                <p v-if="searchValue">
                    Không tìm thấy khách hàng nào với từ khóa "{{ searchValue }}"
                </p>
                <p v-else>
                    Chưa có khách hàng nào được đăng ký trong hệ thống
                </p>
                <button v-if="searchValue" @click="clearSearch" class="btn-primary">
                    Xóa tìm kiếm
                </button>
            </div>
        </div>

        <!-- Pagination -->
        <div v-if="totalPages > 1" class="pagination">
            <button
                :disabled="props.query.PageNumber === 1"
                @click="changePage(props.query.PageNumber - 1)"
                class="page-btn"
            >
                ‹
            </button>
            
            <span class="page-info">
                Trang {{ props.query.PageNumber }} / {{ totalPages }}
            </span>
            
            <button
                :disabled="props.query.PageNumber >= totalPages"
                @click="changePage(props.query.PageNumber + 1)"
                class="page-btn"
            >
                ›
            </button>
        </div>
    </div>
</template>

<style scoped>
.customer-table-container {
    background: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    margin: 20px;
    overflow: hidden;
}

.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 20px 24px;
    background: white;
    border-bottom: 1px solid #eee;
}

.page-header h1 {
    font-size: 24px;
    font-weight: 600;
    margin: 0;
    color: #333;
}

.header-actions {
    display: flex;
    align-items: center;
    gap: 16px;
}

.btn-create {
    background: #007bff;
    color: white;
    border: none;
    padding: 8px 16px;
    border-radius: 4px;
    font-size: 14px;
    cursor: pointer;
}

.btn-create:hover {
    background: #0056b3;
}
.btn-createContract {
    background: #28a745;
    color: white;
    border: none;
    padding: 6px 12px;
    border-radius: 4px;
    font-size: 13px;
    font-weight: 500;
    cursor: pointer;
    transition: background-color 0.2s ease;
}

.btn-createContract:hover {
    background: #218838;
}

.btn-createContract:active {
    background: #1e7e34;
}

.btn-createContract:focus {
    outline: none;
    box-shadow: 0 0 0 2px rgba(40, 167, 69, 0.25);
}

.total-count {
    font-size: 14px;
    color: #666;
}

.search-section {
    padding: 20px 24px;
    background: #f8f9fa;
    border-bottom: 1px solid #eee;
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 20px;
}

.search-wrapper {
    flex: 1;
}

.search-input-wrapper {
    position: relative;
    margin-bottom: 8px;
}

.search-input {
    width: 100%;
    padding: 8px 12px;
    padding-right: 32px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 14px;
}

.search-input:focus {
    outline: none;
    border-color: #007bff;
}

.clear-btn {
    position: absolute;
    right: 8px;
    top: 50%;
    transform: translateY(-50%);
    background: #6c757d;
    color: white;
    border: none;
    border-radius: 50%;
    width: 20px;
    height: 20px;
    cursor: pointer;
    font-size: 12px;
}

.clear-btn:hover {
    background: #5a6268;
}

.search-info {
    font-size: 14px;
    color: #666;
    font-style: italic;
}

.page-size-control {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 14px;
    color: #666;
}

.page-size-select {
    padding: 6px 8px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 14px;
}

.table-info {
    padding: 12px 24px;
    background: white;
    border-bottom: 1px solid #f0f0f0;
    font-size: 14px;
    color: #666;
}

.content {
    min-height: 400px;
}

.loading {
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 60px;
    color: #666;
}

.table-container {
    overflow-x: auto;
}

.table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
}

.table th {
    background: #f8f9fa;
    padding: 12px;
    text-align: left;
    font-weight: 600;
    color: #333;
    border-bottom: 1px solid #ddd;
}

.table td {
    padding: 12px;
    border-bottom: 1px solid #f0f0f0;
}

.table tr:hover {
    background: #f8f9fa;
}

.customer-id {
    font-weight: 600;
    color: #6366f1;
}

.customer-name {
    font-weight: 500;
    color: #333;
}

.gender-badge {
    padding: 4px 8px;
    border-radius: 4px;
    font-size: 12px;
    font-weight: 500;
}

.badge-male {
    background: #cce5ff;
    color: #0066cc;
}

.badge-female {
    background: #f8d7da;
    color: #721c24;
}

.badge-other {
    background: #f8f9fa;
    color: #6c757d;
}

.phone-link {
    color: #28a745;
    text-decoration: none;
}

.phone-link:hover {
    text-decoration: underline;
}

.rental-count {
    font-weight: 600;
    color: #28a745;
}

.date-text {
    color: #666;
    font-size: 13px;
}

.empty-state {
    text-align: center;
    padding: 60px 20px;
    color: #666;
}

.empty-state h3 {
    margin: 0 0 8px 0;
    color: #333;
}

.btn-primary {
    background: #007bff;
    color: white;
    border: none;
    padding: 8px 16px;
    border-radius: 4px;
    font-size: 14px;
    cursor: pointer;
}

.btn-primary:hover {
    background: #0056b3;
}

.pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 12px;
    padding: 20px 24px;
    background: #f8f9fa;
    border-top: 1px solid #eee;
}

.page-btn {
    background: white;
    border: 1px solid #ddd;
    color: #333;
    padding: 6px 10px;
    border-radius: 4px;
    cursor: pointer;
    font-size: 14px;
}

.page-btn:hover:not(:disabled) {
    background: #f8f9fa;
}

.page-btn:disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

.page-info {
    font-size: 14px;
    color: #333;
    font-weight: 500;
}

@media (max-width: 768px) {
    .customer-table-container {
        margin: 10px;
    }
    
    .page-header {
        flex-direction: column;
        gap: 12px;
    }
    
    .search-section {
        flex-direction: column;
        align-items: stretch;
        gap: 16px;
    }
    
    .page-size-control {
        justify-content: center;
    }
}
</style>