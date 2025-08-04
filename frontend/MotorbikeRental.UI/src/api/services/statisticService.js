import apiClient from "../config/axios";

export const statisticService = {
    async getOverview() {
        const response = await apiClient.get("/Statistics/overview");
        return response.data;
    },
    async getMotorbikesStatus(){
        const response = await apiClient.get("/Statistics/motorbike-status");
        return response.data;
    },
    async getContractsStatus(){
        const response = await apiClient.get("/Statistics/contract-status");
        return response.data;
    },
    async getIncidentsStatistics(){
        const response = await apiClient.get("/Statistics/incident-statistics");
        return response.data;
    },
    async getRevenueStatistics(query){
        const response = await apiClient.get("/Statistics/revenue-by-date-range", { params : query });
        return response.data;
    },
    async getHighlightMonthly(query){
        const response = await apiClient.get("/Statistics/monthly-highlights", { params : query });
        return response.data;
    }
}