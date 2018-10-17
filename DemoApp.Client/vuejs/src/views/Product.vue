<template>
    <div id="product-screen">
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Type</th>
                    <th>Price</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for='product in products' :key='product.productName'>
                    <td>{{ product.productName }}</td>
                    <td>{{ product.productCateName }}</td>
                    <td>{{ product.price }}</td>
                    <td>
                        <button class="btn btn-link">Edit</button>
                        <button class="btn btn-link">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
export default {
  data: function() {
    return {
      products: []
    };
  },
  methods: {
    getProduct() {
      this.$store.dispatch("getProduct").then(data => {
        this.products = data;
      });
    }
  },
  mounted() {
    this.getProduct();
  }
};
</script>
