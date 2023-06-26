<template>
  <div id="app">
    <v-app>
      <v-app-bar app color="primary" dark>
        <v-toolbar-title class="ml-4">Pocket Planner</v-toolbar-title>
        <v-spacer></v-spacer>
         <v-router-link to="/budgets" class="mr-4">My Budget</v-router-link>
        <v-router-link to="/registration" class="mr-4">Registration</v-router-link>
        <v-router-link to="/login">Login</v-router-link>
      </v-app-bar>
      <v-main>
        <v-container fluid>
          <RegistrationPage/>
          <Transactions/>
          <Categories />
          <Budgets />
        </v-container>
      </v-main>
    </v-app>
  </div>
</template>

<script>
import RegistrationPage from './components/RegistrationPage.vue';
import Transactions from './components/Transactions.vue';
import Categories from './components/Categories.vue';
import Budgets from './components/Budgets.vue';

export default {
  components: {
    RegistrationPage,
    Transactions,
    Categories,
    Budgets
  }
};
</script>

<style scoped>

</style>