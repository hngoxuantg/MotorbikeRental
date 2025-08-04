import apiClient from '../config/axios'
export const userCredentialsService = {
  async getById(employeeId) {
    try {
      const response = await apiClient.get('/UserCredentials/' + employeeId)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editUserName(employeeId, form) {
    try{
        const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-user-name', form)
        return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editEmail(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-email', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editPhoneNumber(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-phone-number', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editPassword(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-password', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editRole(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-role', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async deleteUserCredentials(employeeId) {
    try {
      await apiClient.delete('/UserCredentials/' + employeeId)
    } catch (error) {
      console.log(error)
      throw error
    }
  }
}
