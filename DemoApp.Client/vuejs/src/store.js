import Vue from "vue";
import Vuex from "vuex";
import axios from "axios";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {},
  mutations: {
    authUser(state, userData) {
      if (userData.statusCode != 201) {
        state.userId = userData.userId;
        state.token = userData.token;
      }
    }
  },
  actions: {
    signUp({ commit }, dataAuth) {
      axios
        .post("/auth/register", dataAuth)
        .then(res => {
          debugger;
          console.log("response data register" + res);
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
          debugger;
          console.log("response data login " + res);
          commit("authUser", {
            statusCode: res.status,
            userId: res.data.userId,
            token: res.data.token
          });
        })
        .catch(error => console.log(error));
    }
  },
  getters: {}
});
