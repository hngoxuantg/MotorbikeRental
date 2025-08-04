<script setup>
</script>

<template>
  <div id="app">
    <nav>
    </nav>
    <RouterView />
  </div>
</template>
<style>
/* Global reset - loại bỏ margin/padding của browser */
html, body {
  margin: 0 !important;
  padding: 0 !important;
  width: 100%;
  height: 100%;
  overflow-x: hidden;
}

#app {
  margin: 0 !important;
  padding: 0 !important;
  width: 100%;
  min-height: 100vh;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
</style>