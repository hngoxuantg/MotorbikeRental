import apiClient from '../config/axios'

export const employeeService = {
  async getAll(query) {
    try {
      const response = await apiClient.get('/Employee', { params: query })
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getRoles() {
    try {
      const response = await apiClient.get('/Employee/roles')
      return response.data.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async createEmployee(form) {
    var formData = new FormData()
    formData.append('FullName', form.FullName)
    formData.append('DateOfBirth', form.DateOfBirth)
    formData.append('Address', form.Address)
    formData.append('StartDate', form.StartDate)
    formData.append('Salary', form.Salary)
    formData.append('Status', form.Status)
    if (form.FormFile) {
      formData.append('FormFile', form.FormFile)
    }
    try {
      const response = await apiClient.post('/Employee', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async updateEmployee(id, form) {
    var formData = new FormData()
    formData.append('employeeId', id)
    formData.append('fullName', form.fullName)
    formData.append('dateOfBirth', form.dateOfBirth)
    formData.append('address', form.address)
    formData.append('startDate', form.startDate)
    formData.append('salary', form.salary)
    formData.append('status', form.status)
    if (form.formFile) {
      formData.append('formFile', form.formFile)
    }
    try {
      const response = await apiClient.put('/Employee/' + id, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
      })
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async deleteEmployee(id) {
    try {
      const response = await apiClient.delete('/Employee/' + id)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async deleteAvatar(id){
    try {
      const response = await apiClient.delete('/Employee/' + id + '/avatar')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getEmployeeById(id) {
    try {
      const response = await apiClient.get('/Employee/' + id)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async createUserCredential(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId , form)  
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
}
