import apiClient from '../config/axios.js'
export const categoryService = {
  async getAll() {
    try {
      const response = await apiClient.get('/Category')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async createCategory(form){
    try{
      const response = await apiClient.post('/Category', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async deleteCategory(id) {
    try {
      const response = await apiClient.delete('/Category/' + id)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getById(id) {
    try {
      const response = await apiClient.get('/Category/' + id)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async updateCategory(id, formData) {
    try {
      const response = await apiClient.put('/Category/' + id, formData)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  }
}
