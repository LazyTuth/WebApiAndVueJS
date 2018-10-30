import { router } from "../router";
import axios from "axios";

const state = {
  token: null,
  userFullname: null
};

const getters = {
  userFullname(state) {
    return state.userFullname;
  },
  isAuthenticated(state) {
    return state.token !== null;
  }
};

const mutations = {
  authUser(state, userData) {
    if (userData.statusCode != 201) {
      state.token = userData.token;
      state.userFullname = userData.userFullname;
    }
  },
  logOut(state) {
    (state.token = null), (state.userId = null), (state.userFullname = null);
    localStorage.removeItem("token");
    localStorage.removeItem("userFullname");
    localStorage.removeItem("expireIn");
    router.replace("/signin");
  }
};

const actions = {
  setLogoutTimer({ commit }, expirationTime) {
    setTimeout(() => {
      commit("logOut");
    }, expirationTime);
  },
  signUp({ commit }, dataAuth) {
    axios
      .post("/auth/register", dataAuth)
      .then(res => {
        commit("authUser", {
          statusCode: res.status,
          userId: res.data.userId
        });
        router.replace("/signin");
      })
      .catch(error => commit("getModelState", error));
  },
  signIn({ commit, dispatch }, dataAuth) {
    axios
      .post("/auth/login", dataAuth)
      .then(res => {
        commit("authUser", {
          statusCode: res.status,
          token: res.data.token,
          userFullname: res.data.userFullname
        });
        const date = new Date();
        let expDate = new Date(res.data.expiration);
        let expSecond = expDate - date;
        sessionStorage.setItem("token", res.data.token);
        localStorage.setItem("token", res.data.token);
        localStorage.setItem("userFullname", res.data.userFullname);
        localStorage.setItem("expireIn", expDate);

        dispatch("setLogoutTimer", expSecond);
        // router.replace("/admin/product");
        router.replace("/");
      })
      .catch(error => {
        commit("getModelState", error);
      });
  },
  keepLogin({ commit }) {
    const token = localStorage.getItem("token");
    if (!token) {
      return;
    }
    const experationDate = localStorage.getItem("expireIn");
    const now = new Date();
    if (now >= new Date(experationDate)) {
      return;
    }
    const userFullname = localStorage.getItem("userFullname");
    commit("authUser", {
      token: token,
      userFullname: userFullname
    });
  },
  logOut({ commit }) {
    commit("logOut");
    localStorage.removeItem("token");
    localStorage.removeItem("userFullname");
    localStorage.removeItem("expireIn");
    router.replace("/signin");
  }
};

export default {
  state,
  mutations,
  getters,
  actions
};
