<template>
  <div id="app">
    <v-app>
      <v-app-bar app color="primary" dark>
        <v-toolbar-title class="ml-4">Pocket Planner</v-toolbar-title>
        <v-btn to="/categories" text>Categories</v-btn>
        <v-btn to="/transactions" text>Transactions</v-btn>
        <v-btn to="/budget" text>Budget</v-btn>
      </v-app-bar>
      <v-main>
        <v-container fluid>
          <router-view></router-view>
        </v-container>
      </v-main>
    </v-app>
  </div>
</template>

<style scoped>

</style>

<script>
export default {
  name: 'App',
};
</script>
