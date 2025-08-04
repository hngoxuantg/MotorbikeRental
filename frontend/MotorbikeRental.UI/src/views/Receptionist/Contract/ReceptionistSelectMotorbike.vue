<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { ref, onMounted } from 'vue'
import { motorbikeService } from '@/api/services/motorbikeService'
import SelectMotorbike from '@/components/Receptionist/Contract/SelectMotorbike.vue'

const query = ref({
  CategoryId: '',
  Status: 0,
  Brand: '',
  Search: '',
  PageNumber: 1,
  PageSize: 12,
})

const updateQuery = async (newQuery) => {
  let changed = false
  for (const key in newQuery) {
    if ((query.value[key] ?? '') !== (newQuery[key] ?? '')) {
      changed = true
      break
    }
  }
  if (!changed) return

  Object.assign(query.value, newQuery)

  try {
    const motorbikeRes = await motorbikeService.getAll(query.value)
    motorbikes.value = motorbikeRes.data.data
    totalMotorbikes.value = motorbikeRes.data.totalCount
  } catch (error) {
    console.error('Error updating query:', error)
  }
}
const motorbikes = ref([])
const totalMotorbikes = ref(0)
const categoriesDto = ref([])
const brands = ref([])
const motorbikeStatuses = ref([])
onMounted(async () => {
  try {
    const [motorbikeRes, optionsRes] = await Promise.all([
      motorbikeService.getAll(query.value),
      motorbikeService.getFilterOptions(),
    ])
    motorbikes.value = motorbikeRes.data.data
    totalMotorbikes.value = motorbikeRes.data.totalCount
    categoriesDto.value = optionsRes.data.categoriesDto
    brands.value = optionsRes.data.brands
    motorbikeStatuses.value = optionsRes.data.motorbikeStatuses
  } catch (error) {
    console.error('Error fetching motorbikes:', error)
  }
})
</script>
<template>
  <ReceptionistLayout>
    <SelectMotorbike
      :motorbikes="motorbikes"
      :totalMotorbikes="totalMotorbikes"
      :categories="categoriesDto"
      :brands="brands"
      :motorbike-statuses="motorbikeStatuses"
      :query="query"
      @updateQuery="updateQuery"
    />
  </ReceptionistLayout>
</template>
<style></style>
