<script setup>
import { ref, onMounted, watch, nextTick } from 'vue'
import { Chart, registerables } from 'chart.js'
import { getFullPath } from '@/utils/UrlUtils'
import defaultAvatar from '@/assets/image.png'

Chart.register(...registerables)

const revenueChartRef = ref(null)
const motorbikeChartRef = ref(null)

let revenueChart = null
let motorbikeChart = null

const emit = defineEmits(['refresh-revenue', 'refresh-highlights'])

const props = defineProps({
  overview: Object,
  motorbikes: Object,
  contracts: Object,
  incidents: Object,
  revenue: Object,
  highlights: Object,
})

const revenueFromDate = ref('')
const revenueToDate = ref('')
const highlightMonth = ref(new Date().getMonth() + 1)
const highlightYear = ref(new Date().getFullYear())

const formatCurrency = (value) => {
  if (!value) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN').format(value) + ' VNĐ'
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('vi-VN')
}

const createRevenueChart = () => {
  if (
    !props.revenue?.dailyRevenues &&
    !props.revenue?.monthlyRevenues &&
    !props.revenue?.yearlyRevenues
  )
    return

  nextTick(() => {
    if (!revenueChartRef.value) return

    const ctx = revenueChartRef.value.getContext('2d')

    let labels = []
    let revenueData = []
    let maintenanceData = []

    if (props.revenue.dailyRevenues) {
      labels = props.revenue.dailyRevenues.map((item) => {
        const date = new Date(item.date)
        return date.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' })
      })
      revenueData = props.revenue.dailyRevenues.map((item) => item.totalRevenue)
      maintenanceData = props.revenue.dailyRevenues.map((item) => item.totalMaintenanceCost)
    } else if (props.revenue.monthlyRevenues) {
      labels = props.revenue.monthlyRevenues.map((item) => `Tháng ${item.month}`)
      revenueData = props.revenue.monthlyRevenues.map((item) => item.totalRevenue)
      maintenanceData = props.revenue.monthlyRevenues.map((item) => item.totalMaintenanceCost)
    } else if (props.revenue.yearlyRevenues) {
      labels = props.revenue.yearlyRevenues.map((item) => `Năm ${item.year}`)
      revenueData = props.revenue.yearlyRevenues.map((item) => item.totalRevenue)
      maintenanceData = props.revenue.yearlyRevenues.map((item) => item.totalMaintenanceCost)
    }

    if (revenueChart) {
      revenueChart.destroy()
    }

    revenueChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: labels,
        datasets: [
          {
            label: 'Doanh thu',
            data: revenueData,
            borderColor: '#2c5aa0',
            backgroundColor: 'rgba(44, 90, 160, 0.1)',
            borderWidth: 2,
            fill: true,
            tension: 0.1,
          },
          {
            label: 'Chi phí bảo trì',
            data: maintenanceData,
            borderColor: '#dc3545',
            backgroundColor: 'rgba(220, 53, 69, 0.1)',
            borderWidth: 2,
            fill: true,
            tension: 0.1,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          title: {
            display: true,
            text: 'Biểu đồ Doanh thu',
            font: { size: 16, weight: 'bold' },
          },
          legend: {
            position: 'top',
            labels: { font: { size: 12 } },
          },
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              callback: function (value) {
                return new Intl.NumberFormat('vi-VN').format(value)
              },
            },
          },
        },
      },
    })
  })
}

const createMotorbikeChart = () => {
  if (!props.motorbikes) return

  nextTick(() => {
    if (!motorbikeChartRef.value) return

    const ctx = motorbikeChartRef.value.getContext('2d')
    const data = props.motorbikes

    if (motorbikeChart) {
      motorbikeChart.destroy()
    }

    motorbikeChart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['Có sẵn', 'Đang thuê', 'Bảo trì', 'Đặt trước', 'Hỏng'],
        datasets: [
          {
            data: [
              data.available || 0,
              data.rented || 0,
              data.underMaintenance || 0,
              data.reserved || 0,
              data.damaged || 0,
            ],
            backgroundColor: ['#28a745', '#007bff', '#ffc107', '#17a2b8', '#dc3545'],
            borderWidth: 1,
            borderColor: '#fff',
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          title: {
            display: true,
            text: 'Trạng thái Xe máy',
            font: { size: 16, weight: 'bold' },
          },
          legend: {
            position: 'bottom',
            labels: { font: { size: 11 } },
          },
        },
      },
    })
  })
}

const refreshRevenue = () => {
  if (revenueFromDate.value && revenueToDate.value) {
    emit('refresh-revenue', {
      FromDate: new Date(revenueFromDate.value),
      ToDate: new Date(revenueToDate.value),
    })
  }
}

const refreshHighlights = () => {
  emit('refresh-highlights', {
    Month: highlightMonth.value,
    Year: highlightYear.value,
  })
}

watch(
  () => props.revenue,
  () => {
    if (props.revenue) {
      createRevenueChart()
    }
  },
  { deep: true }
)

watch(
  () => props.motorbikes,
  () => {
    if (props.motorbikes) {
      createMotorbikeChart()
    }
  },
  { deep: true }
)

onMounted(() => {
  const now = new Date()
  const firstDayOfMonth = new Date(now.getFullYear(), now.getMonth(), 1)
  revenueFromDate.value = firstDayOfMonth.toISOString().split('T')[0]
  revenueToDate.value = now.toISOString().split('T')[0]

  if (props.revenue) createRevenueChart()
  if (props.motorbikes) createMotorbikeChart()
})
</script>

<template>
  <div class="statistics-page">
    <div class="section-card" v-if="overview">
      <h3 class="section-title">Báo cáo tổng quan</h3>
      <div class="overview-grid">
        <div class="stat-item">
          <span class="stat-label">Tổng xe máy:</span>
          <span class="stat-value">{{ overview.totalMotorbikes }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Tổng hợp đồng:</span>
          <span class="stat-value">{{ overview.totalRentals }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Tổng khách hàng:</span>
          <span class="stat-value">{{ overview.totalCustomers }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Tổng doanh thu:</span>
          <span class="stat-value highlight">{{ formatCurrency(overview.totalRevenue) }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Tổng lương:</span>
          <span class="stat-value">{{ formatCurrency(overview.totalSalary) }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Chi phí bảo trì:</span>
          <span class="stat-value">{{ formatCurrency(overview.totalMaintenanceCost) }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Lợi nhuận:</span>
          <span
            class="stat-value"
            :class="{ negative: overview.profit < 0, positive: overview.profit >= 0 }"
          >
            {{ formatCurrency(overview.profit) }}
          </span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Tổng nhân viên:</span>
          <span class="stat-value">{{ overview.totalEmployees }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Ngày khởi động:</span>
          <span class="stat-value">{{ formatDate(overview.systemStartDate) }}</span>
        </div>
        <div class="stat-item">
          <span class="stat-label">Số ngày hoạt động:</span>
          <span class="stat-value">{{ overview.totalActiveDays }} ngày</span>
        </div>
      </div>
    </div>

    <div class="section-card" v-if="revenue">
      <div class="chart-header">
        <h3 class="section-title">Biểu đồ Doanh thu</h3>
        <div class="date-filters">
          <label>Từ ngày:</label>
          <input type="date" v-model="revenueFromDate" />
          <label>Đến ngày:</label>
          <input type="date" v-model="revenueToDate" />
          <button @click="refreshRevenue" class="btn-filter">Lọc</button>
        </div>
      </div>
      <div class="chart-wrapper">
        <canvas ref="revenueChartRef"></canvas>
      </div>
    </div>

    <div class="section-card" v-if="motorbikes">
      <div class="chart-wrapper-small">
        <canvas ref="motorbikeChartRef"></canvas>
      </div>
    </div>

    <div class="tables-row">
      <div class="section-card" v-if="motorbikes">
        <h3 class="section-title">Trạng thái xe máy</h3>
        <table class="status-table">
          <tr>
            <td>Xe có sẵn:</td>
            <td class="text-success">{{ motorbikes.available }}</td>
          </tr>
          <tr>
            <td>Đang cho thuê:</td>
            <td class="text-primary">{{ motorbikes.rented }}</td>
          </tr>
          <tr>
            <td>Đang bảo trì:</td>
            <td class="text-warning">{{ motorbikes.underMaintenance }}</td>
          </tr>
          <tr>
            <td>Xe đặt trước:</td>
            <td class="text-info">{{ motorbikes.reserved }}</td>
          </tr>
          <tr>
            <td>Xe hư hỏng:</td>
            <td class="text-danger">{{ motorbikes.damaged }}</td>
          </tr>
          <tr>
            <td>Ngừng hoạt động:</td>
            <td class="text-muted">{{ motorbikes.outOfService }}</td>
          </tr>
        </table>
      </div>

      <div class="section-card" v-if="contracts">
        <h3 class="section-title">Trạng thái hợp đồng</h3>
        <table class="status-table">
          <tr>
            <td>Tổng hợp đồng:</td>
            <td class="text-primary">{{ contracts.totalContracts }}</td>
          </tr>
          <tr>
            <td>Đang hoạt động:</td>
            <td class="text-success">{{ contracts.totalActiveContracts }}</td>
          </tr>
          <tr>
            <td>Đã hoàn thành:</td>
            <td class="text-info">{{ contracts.totalCompletedContracts }}</td>
          </tr>
          <tr>
            <td>Bị hủy:</td>
            <td class="text-danger">{{ contracts.totalCancelledContracts }}</td>
          </tr>
          <tr>
            <td>Chờ xử lý:</td>
            <td class="text-warning">{{ contracts.totalPendingContracts }}</td>
          </tr>
          <tr>
            <td>Đang xử lý sự cố:</td>
            <td class="text-muted">{{ contracts.totalProcessingContracts }}</td>
          </tr>
        </table>
      </div>

      <div class="section-card" v-if="incidents">
        <h3 class="section-title">Trạng thái sự cố</h3>
        <table class="status-table">
          <tr>
            <td>Tổng sự cố:</td>
            <td class="text-primary">{{ incidents.totalIncidents }}</td>
          </tr>
          <tr>
            <td>Đã xử lý:</td>
            <td class="text-success">{{ incidents.resolvedIncidents }}</td>
          </tr>
          <tr>
            <td>Chưa xử lý:</td>
            <td class="text-danger">{{ incidents.unresolvedIncidents }}</td>
          </tr>
        </table>
      </div>
    </div>

    <div class="highlights-row" v-if="highlights">
      <div class="section-card">
        <div class="chart-header">
          <h3 class="section-title">Nhân viên xuất sắc</h3>
          <div class="month-filters">
            <label>Tháng:</label>
            <select v-model="highlightMonth">
              <option v-for="month in 12" :key="month" :value="month">{{ month }}</option>
            </select>
            <label>Năm:</label>
            <select v-model="highlightYear">
              <option v-for="year in [2023, 2024, 2025]" :key="year" :value="year">
                {{ year }}
              </option>
            </select>
            <button @click="refreshHighlights" class="btn-filter">Lọc</button>
          </div>
        </div>
        <div class="highlights-list">
          <div v-if="highlights.topEmployees && highlights.topEmployees.length > 0">
            <div
              v-for="employee in highlights.topEmployees"
              :key="employee.employeeId"
              class="highlight-item"
            >
              <img
                :src="employee.avatar ? getFullPath(employee.avatar) : defaultAvatar"
                :alt="employee.employeeName"
                class="highlight-avatar"
              />
              <div class="highlight-info">
                <strong>{{ employee.employeeName }}</strong>
                <span>({{ employee.contractCount }} hợp đồng)</span>
              </div>
            </div>
          </div>
          <div v-else class="no-data">
            <p>
              Không có dữ liệu nhân viên trong tháng {{ highlights.month }}/{{ highlights.year }}
            </p>
          </div>
        </div>
      </div>

      <div class="section-card">
        <h3 class="section-title">Xe được thuê nhiều nhất</h3>
        <div class="highlights-list">
          <div v-if="highlights.topMotorbikes && highlights.topMotorbikes.length > 0">
            <div
              v-for="motorbike in highlights.topMotorbikes"
              :key="motorbike.motorbikeId"
              class="highlight-item"
            >
              <img
                :src="getFullPath(motorbike.motorbikeImage)"
                :alt="motorbike.motorbikeName"
                class="highlight-avatar"
              />
              <div class="highlight-info">
                <strong>{{ motorbike.motorbikeName }}</strong>
                <span>({{ motorbike.rentalCount }} lượt thuê)</span>
              </div>
            </div>
          </div>
          <div v-else class="no-data">
            <p>Không có dữ liệu xe máy trong tháng {{ highlights.month }}/{{ highlights.year }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.statistics-page {
  padding: 20px;
  background: #f5f5f5;
  min-height: 100vh;
}

.section-card {
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 20px;
  margin-bottom: 20px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.section-title {
  margin: 0 0 15px 0;
  padding-bottom: 10px;
  border-bottom: 2px solid #eee;
  color: #333;
  font-size: 16px;
  font-weight: bold;
}

.chart-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
}

.date-filters,
.month-filters {
  display: flex;
  align-items: center;
  gap: 10px;
}

.date-filters label,
.month-filters label {
  font-weight: 500;
  color: #495057;
}

.date-filters input,
.month-filters select {
  padding: 5px 8px;
  border: 1px solid #ccc;
  border-radius: 3px;
  font-size: 12px;
}

.btn-filter {
  padding: 6px 12px;
  background: #007bff;
  color: white;
  border: none;
  border-radius: 3px;
  cursor: pointer;
  font-size: 12px;
}

.btn-filter:hover {
  background: #0056b3;
}

.overview-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 15px;
}

.stat-item {
  display: flex;
  justify-content: space-between;
  padding: 10px;
  background: #f8f9fa;
  border: 1px solid #e9ecef;
  border-radius: 3px;
}

.stat-label {
  font-weight: 500;
  color: #495057;
}

.stat-value {
  font-weight: bold;
  color: #212529;
}

.stat-value.highlight {
  color: #007bff;
}

.stat-value.positive {
  color: #28a745;
}
.no-data {
  text-align: center;
  padding: 20px;
  color: #666;
  font-style: italic;
}

.stat-value.negative {
  color: #dc3545;
}

.chart-wrapper {
  height: 500px;
  width: 100%;
}

.chart-wrapper-small {
  height: 300px;
  width: 100%;
}

.tables-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
  margin-bottom: 20px;
}

.status-table {
  width: 100%;
  border-collapse: collapse;
}

.status-table td {
  padding: 8px 12px;
  border-bottom: 1px solid #eee;
}

.status-table td:first-child {
  font-weight: 500;
  color: #495057;
}

.status-table td:last-child {
  text-align: right;
  font-weight: bold;
}

.text-success {
  color: #28a745;
}
.text-primary {
  color: #007bff;
}
.text-warning {
  color: #ffc107;
}
.text-info {
  color: #17a2b8;
}
.text-danger {
  color: #dc3545;
}
.text-muted {
  color: #6c757d;
}

.highlights-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.highlights-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.highlight-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px;
  background: #f8f9fa;
  border: 1px solid #e9ecef;
  border-radius: 3px;
}

.highlight-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid #ddd;
}

.highlight-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.highlight-info strong {
  color: #333;
  font-size: 14px;
}

.highlight-info span {
  color: #666;
  font-size: 12px;
}

@media (max-width: 768px) {
  .statistics-page {
    padding: 10px;
  }

  .chart-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }

  .date-filters,
  .month-filters {
    flex-wrap: wrap;
  }

  .highlights-row {
    grid-template-columns: 1fr;
  }

  .tables-row {
    grid-template-columns: 1fr;
  }

  .overview-grid {
    grid-template-columns: 1fr;
  }
}
</style>