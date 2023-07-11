import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'
import { loadFonts } from './plugins/webfontloader'
import store from './Store/Store.js';
import axios from './plugins/axios'

loadFonts()

createApp(App)
  .use(store)
  .use(router)
  .use(vuetify)
  .use(axios, {
    baseUrl: 'https://localhost:7159/api/',
  })
  .mount('#app')
