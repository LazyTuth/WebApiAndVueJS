import axios from "axios";
import authModule from "./auth.module";

const state = {
  pageNumber: null
};
const mutations = {
  setPageNumber(state, data) {
    state.pageNumber = data.page;
  }
};
const actions = {
  async getProductPagination({ commit }, dataPagination) {
    const config = {
      headers: {
        Authorization: `Bearer ${authModule.state.token}`
      },
      params: {
        pageNumber: dataPagination.page
      }
    };
    return await axios
      .get("/products", config)
      .then(res => {
        if (!dataPagination.page) {
          dataPagination.page = 1;
        }
        commit("setPageNumber", dataPagination);
        return Promise.resolve(res.data);
      })
      .catch(err => console.log(err.message));
  },
  async createProduct(context, dataCreated) {
    const config = {
      headers: {
        Authorization: `Bearer ${authModule.state.token}`
      }
    };
    return await axios
      .post("/products/create", dataCreated, config)
      .then(res => {
        return Promise.resolve(res.status);
      })
      .catch(err => console.log(err.message));
  },
  async getProductById(context, data) {
    const config = {
      headers: {
        Authorization: `Bearer ${authModule.state.token}`
      }
    };
    return await axios.get(`/products/${data.id}`, config).then(res => {
      return Promise.resolve(res.data);
    });
  },
  async updateProduct(context, dataUpdated) {
    const config = {
      headers: {
        Authorization: `Bearer ${authModule.state.token}`
      }
    };
    return await axios
      .patch("products/edit", dataUpdated, config)
      .then(res => {
        return Promise.resolve(res.status);
      })
      .catch(err => console.log(err));
  },
  async getProductCategory() {
    const config = {
      headers: {
        Authorization: `Bearer ${authModule.state.token}`
      }
    };
    return await axios.get("/category", config).then(res => {
      return Promise.resolve(res.data);
    });
  },
  async deleteProduct(context, data) {
    const config = {
      headers: {
        Authorization: `Bearer ${authModule.state.token}`
      },
      params: {
        productId: data
      }
    };
    return await axios
      .delete("products/delete", config)
      .then(res => {
        return Promise.resolve(res.status);
      })
      .catch(err => console.log(err));
  }
};

export default {
  state,
  mutations,
  actions
};
