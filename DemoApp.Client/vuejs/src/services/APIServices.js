import axios from "axios";
const API_URL = "http://localhost:5000";

export class APIServices {
  constructor() {}

  getAllProduct() {
    const url = `${API_URL}/api/products`;
    return axios.get(url).then(response => response.data);
  }
  getSingleProduct(key) {
    const url = `${API_URL}/api/products/${key}`;
    return axios.get(url).then(response => response.data);
  }
}
