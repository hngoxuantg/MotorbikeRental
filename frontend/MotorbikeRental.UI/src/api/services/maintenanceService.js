import apiClient from "../config/axios";

export const maintenanceService = {
    async getHistory(query) {
        try {
            const response = await apiClient.get('/MaintenanceRecord', { params: query });
            return response.data;
        } catch (error) {
            console.error("Error fetching maintenance history:", error);
            throw error;
        }
    },
    async getMotorbikes(query) {
        try {
            const response = await apiClient.get('/MaintenanceRecord/motorbikes', { params: query });
            return response.data;
        } catch (error) {
            console.error("Error fetching motorbikes:", error);
            throw error;
        }
    },
    async createMaintenanceRecord(form) {
        try {
            const response = await apiClient.post(`/MaintenanceRecord`, form);
            return response.data;
        } catch (error) {
            console.error("Error creating maintenance record:", error);
            throw error;
        }
    },
    async completeMaintenance(from) {
        try {
            const response = await apiClient.post(`/MaintenanceRecord/complete`, from);
            return response.data;
        } catch (error) {
            console.error("Error completing maintenance:", error);
            throw error;
        }
    }
}
