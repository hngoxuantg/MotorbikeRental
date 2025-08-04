<script setup>
import { ref, onMounted } from 'vue'
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import Statistics from '@/components/Admin/Statistics/Statistics.vue'
import { useRouter } from 'vue-router'
import { statisticService } from '@/api/services/statisticService'

const router = useRouter()

const overview = ref(null)
const motorbikes = ref(null)
const contracts = ref(null)
const incidents = ref(null)
const revenue = ref(null)
const highlights = ref(null)
const isLoading = ref(false)

onMounted(async () => {
  isLoading.value = true
  try {
    const queryHighlights = {
      Month: new Date().getMonth() + 1,
      Year: new Date().getFullYear(),
    }
    const now = new Date()
    const queryRevenue = {
      FromDate: new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), 1)),
      ToDate: new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate())),
    }
    const [
      overviewResponse,
      motorbikesResponse,
      contractsResponse,
      incidentsResponse,
      revenueResponse,
      highlightsResponse,
    ] = await Promise.all([
      statisticService.getOverview(),
      statisticService.getMotorbikesStatus(),
      statisticService.getContractsStatus(),
      statisticService.getIncidentsStatistics(),
      statisticService.getRevenueStatistics(queryRevenue),
      statisticService.getHighlightMonthly(queryHighlights),
    ])
    overview.value = overviewResponse.data
    motorbikes.value = motorbikesResponse.data
    contracts.value = contractsResponse.data
    incidents.value = incidentsResponse.data
    revenue.value = revenueResponse.data
    highlights.value = highlightsResponse.data
  } catch (error) {
    console.error('Error fetching statistics:', error)
    router.push({ name: 'ErrorPage' })
  } finally {
    isLoading.value = false
  }
})

async function refreshRevenue(params) {
  try {
    isLoading.value = true
    const response = await statisticService.getRevenueStatistics(params)
    revenue.value = response.data
  } catch (error) {
    console.error('Error fetching revenue statistics:', error)
  } finally {
    isLoading.value = false
  }
}

async function refreshHighlights(params) {
  try {
    isLoading.value = true
    const response = await statisticService.getHighlightMonthly(params)
    highlights.value = response.data
  } catch (error) {
    console.error('Error fetching highlights statistics:', error)
  } finally {
    isLoading.value = false
  }
}
</script>
<template>
  <AdminLayout>
    <div v-if="isLoading" class="loading-overlay">
      <div class="loading-container">
        <div class="loading-spinner"></div>
        <p class="loading-text">Đang tải dữ liệu thống kê...</p>
      </div>
    </div>
    <div v-else>
      <Statistics
        :overview="overview"
        :motorbikes="motorbikes"
        :contracts="contracts"
        :incidents="incidents"
        :revenue="revenue"
        :highlights="highlights"
        @refresh-revenue="refreshRevenue"
        @refresh-highlights="refreshHighlights"
      />
    </div>
  </AdminLayout>
</template>
<style scoped>
.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(2px);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.loading-container {
  text-align: center;
  padding: 40px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.loading-spinner {
  width: 50px;
  height: 50px;
  border: 4px solid #e3f2fd;
  border-top: 4px solid #007bff;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 20px;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.loading-text {
  margin: 0;
  color: #007bff;
  font-size: 16px;
  font-weight: 500;
}

.loading-simple {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 400px;
  flex-direction: column;
}

.loading-simple .spinner {
  width: 40px;
  height: 40px;
  border: 3px solid #f3f3f3;
  border-top: 3px solid #007bff;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 15px;
}

.skeleton-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 20px;
}

.skeleton-line {
  height: 20px;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite;
  border-radius: 4px;
  margin-bottom: 10px;
}

.skeleton-line:last-child {
  margin-bottom: 0;
}

.skeleton-line.short {
  width: 60%;
}

.skeleton-line.medium {
  width: 80%;
}

@keyframes shimmer {
  0% {
    background-position: -200% 0;
  }
  100% {
    background-position: 200% 0;
  }
}
</style>