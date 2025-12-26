import axios from 'axios'
import { API_BASE_URL } from '@/config/env'

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})
apiClient.interceptors.request.use((config) => {
  console.log('Sending request:', config.method?.toUpperCase(), config.url)
  
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})
apiClient.interceptors.response.use(
  (response) => {
    console.log('Received response:', response.status, response.data)
    return response
  },
  (error) => {
    console.error('API error:', error.message)
    return Promise.reject(error)
  },
)
export default apiClient
