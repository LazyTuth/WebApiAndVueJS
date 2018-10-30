<template>
    <div id="dashboard">
      <h1>Happy Shopping</h1>
      <hr>
      <div class="row">
        <div class="col-md-12 col-sm-6">
          <div class="form-inline" style="margin-bottom: 20px;">
            <span class="filter-text">Product Category</span>
            <select class="form-control" name="productCate" id="productCate" v-model="selectedProductCate" @change="onChangeProductCate($event)">
              <option v-for="prd in productCateList" :value="prd.cateCode" :key="prd.cateCode">{{prd.description}}</option>
            </select>
          </div>
        </div>
      </div>
      <div class="row">
        <div v-for="product in filteredProduct" :key="product.id" class="col-md-3 col-sm-6" style="margin-bottom: 5px;">
          <div class="product-grid">
            <div class="product-image">
              <a>
                <img class="pic-1" :src="rootApiUrl + product.imageUrl + '/' + product.listImages[0]" :alt="product.productName" />
                <img class="pic-2" :src="rootApiUrl + product.imageUrl + '/' + product.listImages[1]" :alt="product.productName" />
              </a>
              <ul class="social">
                <li>
                  <a data-tip="Quick View" @click="goToDetail(product.id)"><i class="fa fa-search"></i></a>
                </li>
                <li>
                  <a data-tip="Add to Wishlist"><i class="fa fa-shopping-bag"></i></a>
                </li>
                <li>
                  <a data-tip="Add to Cart"><i class="fa fa-shopping-cart"></i></a>
                </li>
              </ul>
              <span class="product-new-label">Sale</span>
              <span class="product-discount-label">20%</span>
            </div>
            <div class="product-content">
              <h3 class="title">
                <a>{{ product.productName }}</a>
              </h3>
              <div class="price">
                {{ product.price | to-currency }}
                <span>$20000</span>
              </div>
              <a class="add-to-cart">ADD TO CART</a>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script>
export default {
  data() {
    return {
      productList: [],
      productImages1: null,
      productImages2: null,
      rootApiUrl: "http://localhost:5000/",
      selectedProductCate: "all",
      productCateList: []
    };
  },
  computed: {
    filteredProduct() {
      const self = this;
      if (this.selectedProductCate != "all") {
        return this.productList.filter(e => {
          return e.productCateCode == self.selectedProductCate;
        });
      } else {
        return this.productList;
      }
    }
  },
  methods: {
    async getListProduct() {
      const self = this;
      await this.$store
        .dispatch("getProductForShoppingPage")
        .then(res => {
          self.productList = res;
        })
        .catch(err => console.log(err));
    },
    async getListProductCategory() {
      const self = this;
      await this.$store.dispatch("getProductCategory").then(res => {
        res.unshift({ cateCode: "all", description: "All" });
        self.productCateList = res;
      });
    },
    goToDetail(productId) {
      this.$router.replace({
        name: "productDetail",
        params: { productId: productId }
      });
    },
    onChangeProductCate(e) {
      this.selectedProductCate = e.target.value;
    }
  },
  mounted() {
    this.getListProduct();
    this.getListProductCategory();
  }
};
</script>

<style scoped>
.filter-text {
  font-style: italic;
  font-size: 14px;
  margin-right: 10px;
}
</style>
