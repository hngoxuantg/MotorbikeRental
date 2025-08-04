<template>
  <div class="forbidden-container">
    <div class="forbidden-card">
      <h1>403</h1>
      <h2>Không có quyền truy cập</h2>
      <p>Bạn không có quyền truy cập vào trang này.</p>
      <router-link to="/" class="back-link">Quay về trang chủ</router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Forbidden'
}
</script>

<style scoped>
.forbidden-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #ff9966 0%, #ff5e62 100%);
}
.forbidden-card {
  background: #fff;
  padding: 2.5rem 2rem;
  border-radius: 18px;
  box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.2);
  text-align: center;
}
.forbidden-card h1 {
  font-size: 4rem;
  color: #ff5e62;
  margin-bottom: 0.5rem;
}
.forbidden-card h2 {
  color: #333;
  margin-bottom: 1rem;
}
.forbidden-card p {
  color: #666;
  margin-bottom: 1.5rem;
}
.back-link {
  color: #fff;
  background: #ff5e62;
  padding: 0.6rem 1.2rem;
  border-radius: 8px;
  text-decoration: none;
  transition: background 0.2s;
}
.back-link:hover {
  background: #ff9966;
}
</style>