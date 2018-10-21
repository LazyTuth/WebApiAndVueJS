import Vue from "vue";
import Router from "vue-router";
import SignIn from "./components/auth/SignIn.vue";
import SignUp from "./components/auth/SignUp.vue";
import Dashboard from "./components/dashboard/Dashboard.vue";
import Admin from "./components/admin/Admin.vue";
import Product from "./components/admin/Product.vue";

Vue.use(Router);

function deleteLocalStorage() {
  localStorage.removeItem("token");
  localStorage.removeItem("userFullname");
  localStorage.removeItem("expireIn");
}

export const router = new Router({
  mode: "history",
  base: process.env.BASE_URL,
  routes: [
    {
      path: "/",
      name: "dashboard",
      component: Dashboard,
      beforeEnter(from, to, next) {
        if (localStorage.getItem("token")) {
          const now = new Date();
          if (now < new Date(localStorage.expireIn)) {
            next();
          } else {
            deleteLocalStorage();
            next("/signin");
          }
        } else {
          deleteLocalStorage();
          next("/signin");
        }
      }
    },
    {
      path: "/admin",
      component: Admin,
      children: [
        {
          path: "product",
          name: "product",
          component: Product
        }
      ],
      beforeEnter(from, to, next) {
        if (localStorage.getItem("token")) {
          const now = new Date();
          if (now < new Date(localStorage.expireIn)) {
            next();
          } else {
            deleteLocalStorage();
            next("/signin");
          }
        } else {
          deleteLocalStorage();
          next("/signin");
        }
      }
    },
    {
      path: "/signin",
      name: "signin",
      component: SignIn
    },
    {
      path: "/signup",
      name: "signup",
      component: SignUp
    },
    {
      path: "*",
      redirect: "/"
    }
  ]
});
