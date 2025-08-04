import apiClient from '../config/axios'
export const discountService = {
  async getAll(query) {
    try {
      const response = await apiClient.get('/Discount', { params: query })
      return response.data;
    } catch (error) {
      console.error('Error fetching discounts:', error)
      throw error
    }
  },
  async getById(discountId) {
    try {
      const response = await apiClient.get('/Discount/' + discountId)
      return response.data
    } catch (error) {
      console.error('Error fetching discount by ID:', error)
      throw error
    }
  },
  async createDiscount(form) {
    try {
      const response = await apiClient.post('/Discount', form)
      return response.data
    } catch (error) {
      console.error('Error creating discount:', error)
      throw error
    }
  },
  async updateDiscount(discountId, form) {
    try {
      const response = await apiClient.put('/Discount/' + discountId, form)
      return response.data
    } catch (error) {
      console.error('Error updating discount:', error)
      throw error
    }
  },
  async deleteDiscount(discountId) {
    try {
      await apiClient.delete('/Discount/' + discountId)
    } catch (error) {
      console.error('Error deleting discount:', error)
      throw error
    }
  },
}
