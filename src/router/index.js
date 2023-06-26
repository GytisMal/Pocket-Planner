import { createRouter, createWebHistory } from 'vue-router';
import Login from "../components/Login.vue";
import Budgets from "../components/Budgets.vue";
import Categories from "../components/Categories.vue";
import Transactions from "../components/Transactions.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Login',
      component: Login
    },
    {
      path: "/budget",
      name: "Budgets",
      component: Budgets
    },
    {
      path: "/categories",
      name: "Categories",
      component: Categories
    },
    {
      path: "/transactions",
      name: "Transactions",
      component: Transactions
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    }
  ]
})

export default router
