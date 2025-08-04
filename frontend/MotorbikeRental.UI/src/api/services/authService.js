import apiClient from '../config/axios.js'

export const login = async (credentials) => {
    try{
        const response = await apiClient.post('/Auth', credentials)
        return response.data;
    }catch (error) {
        return error.response?.data || { success: false, message: 'Login failed' };
    }
}