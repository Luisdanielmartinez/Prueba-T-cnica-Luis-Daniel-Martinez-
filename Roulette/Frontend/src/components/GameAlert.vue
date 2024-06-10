<template>
    <div v-if="visible" class="alert alert-danger alert-dismissible fade show" role="alert">
      {{ message }}
      <button type="button" class="btn-close" @click="visible = false" aria-label="Close"></button>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      message: {
        type: String,
        required: true,
      },
    },
    data() {
      return {
        visible: true,
      };
    },
    watch: {
      message() {
        this.visible = true;
      }
    }
  };
  </script>
  
  <style scoped>
  .alert {
    position: fixed;
    top: 20px;
    right: 20px;
    z-index: 1050;
  }
  </style>
  