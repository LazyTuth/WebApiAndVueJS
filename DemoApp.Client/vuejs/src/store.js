import Vue from "vue";
import Vuex from "vuex";
import auth from "./storeModules/auth.module";
import product from "./storeModules/product.module";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    modelState: null
  },
  mutations: {
    getModelState(state, modelStateData) {
      state.modelState = modelStateData.response.data;
    },
    clearModelState(state) {
      state.modelState = null;
    }
  },
  getters: {
    modelState(state) {
      return state.modelState;
    }
  },
  modules: {
    auth,
    product
  }
});
