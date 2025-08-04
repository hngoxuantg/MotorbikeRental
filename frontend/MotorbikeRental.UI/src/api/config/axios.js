import axios from 'axios'

const apiClient = axios.create({
  baseURL: 'https://localhost:7060/api',
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
