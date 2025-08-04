import apiClient from '../config/axios'
export const incidentService = {
  async create(form) {
    const formData = new FormData()

    formData.append('ContractId', form.contractId)
    formData.append('IncidentDate', form.incidentDate)
    formData.append('Type', form.type)
    formData.append('Severity', form.severity)

    if (form.description !== null && form.description !== undefined)
      formData.append('Description', form.description)
    formData.append('DamageCost', form.damageCost)
    if (form.resolvedDate) formData.append('ResolvedDate', form.resolvedDate)
    formData.append('ReportedByEmployeeId', form.reportedByEmployeeId)

    if (form.images && form.images.length > 0) {
      form.images.forEach((file, index) => {
        formData.append('Images', file)
      })
    }
    const response = await apiClient.post('/Incident', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    })
    return response.data
  },
  async getByContractId(contractId) {
    try {
      const response = await apiClient.get('/Incident/' + contractId + '/by-contractId')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getAll(query) {
    try {
      const response = await apiClient.get('/Incident', { params: query })
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async complete(incidentId, form) {
    try {
      const response = await apiClient.post('/Incident/' + incidentId + '/complete', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async deleteIncident(incidentId) {
    try {
      const response = await apiClient.delete('/Incident/' + incidentId)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async updateBefore(form) {
    try {
      const response = await apiClient.post('/Incident/update-before', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getIncidentById(incidentId){
    try {
      const response = await apiClient.get('/Incident/' + incidentId + '/by-id')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  }
}
