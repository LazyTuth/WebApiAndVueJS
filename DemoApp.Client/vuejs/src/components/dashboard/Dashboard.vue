<template>
    <div id="dashboard">
      <h1>Happy Shopping</h1>
      <hr>
      <div class="row">
        <div v-for="product in productList" :key="product.id" class="col-md-3 col-sm-6" style="margin-bottom: 5px;">
          <div class="product-grid">
            <div class="product-image">
              <a>
                <img class="pic-1" :src="rootApiUrl + product.imageUrl + '/' + product.listImages[0]" :alt="product.productName" />
                <img class="pic-2" :src="rootApiUrl + product.imageUrl + '/' + product.listImages[1]" :alt="product.productName" />
              </a>
              <ul class="social">
                <li>
                  <a data-tip="Quick View"><i class="fa fa-search"></i></a>
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
      rootApiUrl: "http://localhost:5000/"
    };
  },
  methods: {
    async getListProduct() {
      const self = this;
      await this.$store
        .dispatch("getProductForShoppingPage")
        .then(res => {
          debugger;
          self.productList = res;
        })
        .catch(err => console.log(err));
    }
  },
  mounted() {
    this.getListProduct();
  }
};
</script>
