<template>
  <div class="container">
    <!-- Biểu tượng xe máy -->
    <div class="icon-wrapper">
      <div class="motorcycle-icon">
        🏍️
      </div>
    </div>

    <!-- Số 404 -->
    <div class="error-number">
      404
    </div>

    <!-- Tiêu đề và mô tả -->
    <div class="content">
      <h2 class="title">Trang không tồn tại</h2>
      <p class="description">
        Trang bạn đang truy cập không tồn tại trong hệ thống.
      </p>
      <p class="sub-description">
        Vui lòng kiểm tra lại đường dẫn hoặc liên hệ IT để hỗ trợ.
      </p>
    </div>

    <!-- Các nút hành động -->
    <div class="actions">
      <button @click="goBack" class="btn btn-primary">
        ← Quay lại
      </button>
      
      <button @click="goHome" class="btn btn-secondary">
        🏠 Trang chính
      </button>
    </div>

    <!-- Thông tin hỗ trợ -->
    <div class="support-info">
      <h3 class="support-title">Cần hỗ trợ kỹ thuật?</h3>
      <div class="contact-info">
        <div class="contact-item">
          <span>🔧</span>
          <span>IT Support: Ext 101</span>
        </div>
        <div class="contact-item">
          <span>💬</span>
          <span>Slack: #it-support</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// Các function xử lý sự kiện
const goBack = () => {
  window.history.back()
}

const goHome = () => {
  window.location.href = '/'
}
</script>

<style scoped>
.container {
  min-height: 100vh;
  background: linear-gradient(135deg, #1e293b 0%, #334155 100%);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.icon-wrapper {
  margin-bottom: 2rem;
}

.motorcycle-icon {
  font-size: 6rem;
  background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%);
  width: 120px;
  height: 120px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 20px 40px rgba(59, 130, 246, 0.3);
  animation: bounce 2s ease-in-out infinite;
  border: 3px solid rgba(255, 255, 255, 0.1);
}

.error-number {
  font-size: 8rem;
  font-weight: 900;
  background: linear-gradient(135deg, #06b6d4 0%, #3b82f6 50%, #8b5cf6 100%);
  background-clip: text;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  line-height: 1;
  margin-bottom: 1.5rem;
  text-shadow: 0 0 30px rgba(59, 130, 246, 0.5);
}

.content {
  text-align: center;
  max-width: 600px;
  margin-bottom: 3rem;
}

.title {
  font-size: 2rem;
  font-weight: 600;
  color: #f1f5f9;
  margin-bottom: 1rem;
}

.description {
  font-size: 1.125rem;
  color: #cbd5e1;
  line-height: 1.6;
  margin-bottom: 0.5rem;
}

.sub-description {
  font-size: 1rem;
  color: #94a3b8;
}

.actions {
  display: flex;
  gap: 1rem;
  margin-bottom: 3rem;
  flex-wrap: wrap;
  justify-content: center;
}

.btn {
  padding: 0.75rem 1.5rem;
  font-size: 1rem;
  font-weight: 500;
  border-radius: 0.5rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  text-decoration: none;
  display: inline-block;
}

.btn-primary {
  background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%);
  color: white;
  box-shadow: 0 8px 20px rgba(59, 130, 246, 0.4);
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.btn-primary:hover {
  background: linear-gradient(135deg, #2563eb 0%, #1e40af 100%);
  transform: translateY(-2px);
  box-shadow: 0 12px 24px rgba(59, 130, 246, 0.5);
}

.btn-secondary {
  background: rgba(255, 255, 255, 0.1);
  color: #f1f5f9;
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
  backdrop-filter: blur(10px);
}

.btn-secondary:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateY(-2px);
  box-shadow: 0 12px 24px rgba(0, 0, 0, 0.4);
}

.support-info {
  background: rgba(255, 255, 255, 0.05);
  padding: 2rem;
  border-radius: 1rem;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
  border: 1px solid rgba(255, 255, 255, 0.1);
  max-width: 500px;
  width: 100%;
  backdrop-filter: blur(10px);
}

.support-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #f1f5f9;
  margin-bottom: 1rem;
  text-align: center;
}

.contact-info {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.contact-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.875rem;
  color: #cbd5e1;
  justify-content: center;
}

@keyframes bounce {
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-10px);
  }
  60% {
    transform: translateY(-5px);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .error-number {
    font-size: 6rem;
  }
  
  .title {
    font-size: 1.5rem;
  }
  
  .actions {
    flex-direction: column;
    align-items: center;
  }
  
  .btn {
    width: 200px;
  }
  
  .contact-info {
    text-align: center;
  }
}

@media (max-width: 480px) {
  .motorcycle-icon {
    font-size: 4rem;
    width: 100px;
    height: 100px;
  }
  
  .error-number {
    font-size: 4rem;
  }
  
  .support-info {
    padding: 1.5rem;
  }
}
</style>