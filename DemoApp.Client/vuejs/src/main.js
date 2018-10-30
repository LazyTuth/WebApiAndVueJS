import Vue from "vue";
import App from "./App.vue";
import axios from "axios";
import { router } from "./router";
import store from "./store";
import "./registerServiceWorker";
import Vuelidate from "vuelidate";

import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "@fortawesome/fontawesome-free";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "./assets/mySite.css";
import VueSweetalert2 from "vue-sweetalert2";
import Modal from "./components/modal/Modal.vue";

axios.defaults.baseURL = "http://localhost:5000/api";

Vue.config.productionTip = false;
Vue.filter("to-currency", function(value) {
  if (typeof value !== "number") {
    return value;
  }
  const formatter = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 0
  });
  return formatter.format(value);
});

Vue.component("app-modal", Modal);
Vue.use(VueSweetalert2);
Vue.use(Vuelidate);

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
