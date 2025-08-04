import apiClient from '../config/axios.js'
export const motorbikeService = {
  async getAll(query) {
    try {
      const response = await apiClient.get('/Motorbike', { params: query })
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getFilterOptions() {
    try {
      const response = await apiClient.get('Motorbike/filter-options')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getById(id) {
    try {
      const response = await apiClient.get('/Motorbike/' + id)
      const d = response.data.data
      // Chỉ lấy các trường cần thiết
      return {
        motorbikeId: d.motorbikeId,
        motorbikeName: d.motorbikeName,
        categoryId: d.categoryId,
        categoryName: d.categoryName,
        hourlyRate: d.hourlyRate,
        dailyRate: d.dailyRate,
        licensePlate: d.licensePlate,
        brand: d.brand,
        year: d.year,
        color: d.color,
        engineCapacity: d.engineCapacity,
        chassisNumber: d.chassisNumber,
        engineNumber: d.engineNumber,
        description: d.description,
        motorbikeConditionStatus: d.motorbikeConditionStatus,
        imageUrl: d.imageUrl,
        mileage: d.mileage,
        status: d.status,
      }
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async createMotorbike(form) {
    const formData = new FormData()
    formData.append('MotorbikeName', form.MotorbikeName)
    formData.append('CategoryId', form.CategoryId)
    formData.append('HourlyRate', form.HourlyRate)
    formData.append('DailyRate', form.DailyRate)
    formData.append('LicensePlate', form.LicensePlate)
    formData.append('Brand', form.Brand)
    formData.append('Year', form.Year)
    formData.append('Color', form.Color)
    formData.append('EngineCapacity', form.EngineCapacity)
    formData.append('ChassisNumber', form.ChassisNumber)
    formData.append('EngineNumber', form.EngineNumber)
    formData.append('Description', form.Description)
    formData.append('MotorbikeConditionStatus', form.MotorbikeConditionStatus)
    formData.append('Mileage', form.Mileage)
    if (form.FormFile) {
      formData.append('FormFile', form.FormFile)
    }
    for (let pair of formData.entries()) {
      console.log(pair[0] + ': ' + pair[1])
    }
    try {
      const response = await apiClient.post('/Motorbike', formData, {
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
  async updateMotorbike(form) {
    const formData = new FormData()
    formData.append('MotorbikeId', form.motorbikeId)
    formData.append('MotorbikeName', form.motorbikeName)
    formData.append('CategoryId', Number(form.categoryId))
    formData.append('HourlyRate', form.hourlyRate)
    formData.append('DailyRate', form.dailyRate)
    formData.append('LicensePlate', form.licensePlate)
    formData.append('Brand', form.brand)
    formData.append('Year', form.year)
    formData.append('Color', form.color)
    formData.append('EngineCapacity', form.engineCapacity)
    formData.append('ChassisNumber', form.chassisNumber)
    formData.append('EngineNumber', form.engineNumber)
    formData.append('Description', form.description)
    formData.append('Status', Number(form.status))
    formData.append('MotorbikeConditionStatus', Number(form.motorbikeConditionStatus))
    formData.append('Mileage', form.mileage)
    if (form.formFile) {
      formData.append('FormFile', form.formFile)
    }
    try {
      const response = await apiClient.put('/Motorbike/' + form.motorbikeId, formData, {
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
  async deleteMotorbike(id) {
    try {
      const response = await apiClient.delete('/Motorbike/' + id)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
}
