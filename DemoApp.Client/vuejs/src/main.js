import Vue from "vue";
import App from "./App.vue";
import axios from "axios";
import { router } from "./router";
import store from "./store";
import "./registerServiceWorker";

import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";

axios.defaults.baseURL = "http://localhost:5000/api";

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
