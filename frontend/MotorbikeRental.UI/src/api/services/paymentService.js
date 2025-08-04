import apiClient from '../config/axios.js'

export const paymentService = {
    async previewPayment(contractId){
        try {
            const response = await apiClient.post(`/Payment/${contractId}/preview-payment`);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async processPayment(form){
        try{
            const response = await apiClient.post('/Payment/process-payment', form);
            return response.data;
        }catch (error) {
            console.log(error);
            throw error;
        }
    },
    async getById(id){
        try{
            const response = await apiClient.get(`/Payment/${id}`);
            return response.data;
        }catch (error) {
            console.log(error);
            throw error;
        }
    }
}