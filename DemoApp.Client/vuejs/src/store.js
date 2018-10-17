import Vue from "vue";
import Vuex from "vuex";
import axios from "axios";
import { router } from "./router";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    token: null,
    userId: null,
    userFullname: null
  },
  mutations: {
    authUser(state, userData) {
      if (userData.statusCode != 201) {
        state.userId = userData.userId;
        state.token = userData.token;
        state.userFullname = userData.userFullname;
      }
    },
    logOut(state) {
      (state.token = null), (state.userId = null), (state.userFullname = null);
    }
  },
  actions: {
    signUp({ commit }, dataAuth) {
      axios
        .post("/auth/register", dataAuth)
        .then(res => {
          commit("authUser", {
            statusCode: res.status,
            userId: res.data.userId
          });
        })
        .catch(error => console.log(error));
    },
    signIn({ commit }, dataAuth) {
      axios
        .post("/auth/login", dataAuth)
        .then(res => {
          commit("authUser", {
            statusCode: res.status,
            userId: res.data.userId,
            token: res.data.token,
            userFullname: res.data.userFullname
          });
          router.replace("/product");
        })
        .catch(error => console.log(error));
    },
    getProduct({ state }) {
      const config = {
        headers: {
          Authorization: `Bearer ${state.token}`
        }
      };
      return axios
        .get("/products", config)
        .then(res => res.data)
        .catch(error => {
          console.log(error.message);
        });
    },
    logOut({ commit }) {
      commit("logOut");
      router.replace("/signin");
    }
  },
  getters: {
    userFullname(state) {
      return state.userFullname;
    },
    userId(state) {
      return state.userId;
    },
    isAuthenticated(state) {
      return state.token !== null;
    }
  }
});
