import apiClient from "../config/axios";
export const contractService = {
    async getAll(query) {
        try{
            const response = await apiClient.get('/Contract/', { params: query });
            return response.data;
        }catch (error) {
            console.log(error);
            throw error;
        }
    },
    async calculateContractPrice(form) {
        try {
            const response = await apiClient.post('/Contract/calculate-price', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async createContract(form) {
        try {
            const response = await apiClient.post('/Contract', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async updateContractStatusActive(contractId) {
        try {
            const response = await apiClient.post('/Contract/' + contractId + '/active');
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async cancelContractByCustomer(contractId) {
        try {
            const response = await apiClient.post('/Contract/' + contractId + '/cancel-contract');
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async updateContractBeforeActivation(form) {
        try {
            const response = await apiClient.put('/Contract/before-activation', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async getById(contractId) {
        try{
            const response = await apiClient.get('/Contract/' + contractId);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async deleteContract(contractId) {
        try {
            await apiClient.delete('/Contract/' + contractId);
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async contractSettlement(form, contractId) {
        try {
            const response = await apiClient.post('/Contract/' + contractId + '/settlement', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    }
}