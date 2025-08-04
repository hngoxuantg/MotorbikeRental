export function getFullPath(path) {
  if (!path) return '/placeholder-bike.jpg'
  const rawBaseUrl = import.meta.env.VITE_API_URL
  const baseUrl = rawBaseUrl.replace(/\/$/, '')
  const cleanPath = path.startsWith('/') ? path : '/' + path
  return baseUrl + cleanPath
}