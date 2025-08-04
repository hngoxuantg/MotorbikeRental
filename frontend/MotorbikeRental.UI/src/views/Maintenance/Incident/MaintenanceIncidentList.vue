<script setup>
import MaintenanceLayout from '@/views/layouts/Maintenance/MaintenanceLayout.vue'
import ListIncident from '@/components/Maintenance/Incident/ListIncident.vue'
import { onMounted, ref } from 'vue'
import { incidentService } from '@/api/services/incidentService'
import { useRouter } from 'vue-router'

const router = useRouter()
const incidents = ref([])
const isLoading = ref(true)
onMounted(async () => {
    try {
        const params = {
            MotorbikeStatus: 5,
            IsResolved: false,
        }
        const response = await incidentService.getAll(params);
        incidents.value = response.data;
    } catch (error) {
        console.error('Error fetching incidents:', error);
    } finally {
        isLoading.value = false;
    }
})

function handleIncident(incidentId) {
    router.push( '/Maintenance/incident/complete/' + incidentId)
}
</script>
<template>
    <MaintenanceLayout>
        <ListIncident :incidents="incidents" :isLoading="isLoading" @handleIncident="handleIncident" />
    </MaintenanceLayout>
</template>