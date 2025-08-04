import apiClient from "../config/axios";
export const customerService = {
    async getAll(query){
        try{
            const response = await apiClient.get('/Customer', { params: query });
            return response.data;
        } catch (error) {
            console.error('Error fetching customers:', error);
            throw error;
        }
    },
    async getById(customersId){
        try{
            const response = await apiClient.get('/Customer/' + customersId);
            return response.data;
        } catch (error) {
            console.error('Error fetching customer by ID:', error);
            throw error;
        }
    },
    async createCustomer(form){
        try{
            const response = await apiClient.post('/Customer', form);
            return response.data;
        } catch (error) {
            console.error('Error creating customer:', error);
            throw error;
        }
    },
    async updateCustomer(customersId, form){
        try{
            const response = await apiClient.put('/Customer/' + customersId, form);
            return response.data;
        } catch (error) {
            console.error('Error updating customer:', error);
            throw error;
        }
    },
    async deleteCustomer(customerId){
        try{
            await apiClient.delete('/Customer/' + customerId);
        } catch (error) {
            console.error('Error deleting customer:', error);
            throw error;
        }
    }
}