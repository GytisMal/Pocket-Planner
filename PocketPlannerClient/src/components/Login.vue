<template>
    <div class="login-form">
      <v-form @submit.prevent="login">
        <v-text-field v-model="username" label="Username"></v-text-field>
        <v-text-field v-model="password" label="Password" type="password"></v-text-field>
        <v-btn type="submit" color="primary">Login</v-btn>
      </v-form>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        username: '',
        password: '',
      };
    },
    methods: {
      login() {
  
        console.log('Login data:', {
          username: this.username,
          password: this.password,
        });
      }
    }
  };
  </script>
  
  <style scoped>
  .login-form {
    max-width: 400px;
    margin: 0 auto;
    padding: 20px;
    background-color: #f5f5f5;
    border-radius: 4px;
  }
  </style>
  