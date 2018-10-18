import axios from "axios";
import authModule from "./auth.module";

const actions = {
  getProduct() {
    const config = {
      headers: {
        Authorization: `Bearer ${authModule.state.token}`
      }
    };
    return axios
      .get("/products", config)
      .then(res => res.data)
      .catch(error => {
        console.log(error.message);
      });
  }
};

export default {
  actions
};
