<template>
  <div class="registration-form">
    <v-form @submit.prevent="register">
      <v-text-field v-model="username" label="Username"></v-text-field>
      <v-text-field v-model="email" label="Email"></v-text-field>
      <v-text-field v-model="password" label="Password" type="password"></v-text-field>
      <v-text-field v-model="confirmPassword" label="Confirm Password" type="password"></v-text-field>
      <v-btn type="submit" color="primary">Registration</v-btn>
    </v-form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      username: '',
      email: '',
      password: '',
      confirmPassword: ''
    };
  },
  methods: {
    register() {

      console.log('Registration data:', {
        username: this.username,
        email: this.email,
        password: this.password,
        confirmPassword: this.confirmPassword
      });
    }
  }
};
</script>

<style scoped>
.registration-form {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  background-color: #f5f5f5;
  border-radius: 4px;
}
</style>
