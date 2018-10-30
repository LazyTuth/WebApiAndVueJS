<template>
    <div id="productDetail">
        <div class="card">
			<div class="container-fliud">
				<div class="wrapper row">
					<div class="preview col-md-6">
						<div class="preview-pic tab-content">
                            <div v-for="(image, index) in listProductImages" :key="index" class="tab-pane" :class="{active: index == selectedImage}" style="text-align: center;">
                                <img :id="['pic' + productId]" :src="rootApiUrl + imageUrl + '/' + image" :alt="productName">
                            </div>
						</div>
						<ul class="preview-thumbnail nav nav-tabs">
                            <li v-for="(image, index) in listProductImages" :key="index" :class="{active: index == selectedImage}">
                                <a @click="selectDisplayImage(index)">
                                    <img :src="rootApiUrl + imageUrl + '/' + image" :alt="productName">
                                </a>
                            </li>
						</ul>
						
					</div>
					<div class="details col-md-6">
						<h3 class="product-title">{{ productName }}</h3>
						<p class="product-description">{{ description }}</p>
						<h4 class="price">current price: <span>{{ price | to-currency }}</span></h4>
						<div class="action">
							<button class="add-to-cart btn btn-default" type="button">add to cart</button>
						</div>
					</div>
				</div>
			</div>
		</div>
        <hr>
        <div id="product-detail-comment">
            <h3>Product's Comment</h3>
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">
                        Comment 1
                    </h5>
                    <p class="card-text">
                        Comming soon !!!
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
  data() {
    return {
      productId: null,
      productName: null,
      description: null,
      price: null,
      listProductImages: [],
      imageUrl: null,
      rootApiUrl: "http://localhost:5000/",
      selectedImage: null
    };
  },
  methods: {
    async getProductData(productId) {
      const self = this;
      await this.$store
        .dispatch("getProductById", { id: productId })
        .then(res => {
          self.productId = res.id;
          self.productName = res.productName;
          self.description = res.description;
          self.price = res.price;
          self.listProductImages = res.listImages;
          self.imageUrl = res.imageUrl;
        });
    },
    selectDisplayImage(index) {
      this.selectedImage = index;
    }
  },
  mounted() {
    this.productId = this.$route.params.productId;
    this.getProductData(this.$route.params.productId);
    this.selectedImage = 0;
  }
};
</script>

<style scoped>
img {
  max-width: 100%;
}

.preview {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -webkit-flex-direction: column;
  -ms-flex-direction: column;
  flex-direction: column;
}
@media screen and (max-width: 996px) {
  .preview {
    margin-bottom: 20px;
  }
}

.preview-pic {
  -webkit-box-flex: 1;
  -webkit-flex-grow: 1;
  -ms-flex-positive: 1;
  flex-grow: 1;
}
.preview-pic img {
  max-width: 300px;
}

.preview-thumbnail.nav-tabs {
  border: none;
  margin-top: 15px;
}
.preview-thumbnail.nav-tabs li {
  width: 18%;
  margin-right: 2.5%;
}
.preview-thumbnail.nav-tabs li img {
  max-width: 100%;
  display: block;
}
.preview-thumbnail.nav-tabs li a {
  padding: 0;
  margin: 0;
}
.preview-thumbnail.nav-tabs li:last-of-type {
  margin-right: 0;
}

.preview-thumbnail > li.active {
  border: solid 2px #6b5b95;
}

.tab-content {
  overflow: hidden;
}
.tab-content img {
  width: 100%;
  -webkit-animation-name: opacity;
  animation-name: opacity;
  -webkit-animation-duration: 0.3s;
  animation-duration: 0.3s;
}

.card {
  margin-top: 50px;
  background: #eee;
  padding: 3em;
  line-height: 1.5em;
}

@media screen and (min-width: 997px) {
  .wrapper {
    display: -webkit-box;
    display: -webkit-flex;
    display: -ms-flexbox;
    display: flex;
  }
}

.details {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -webkit-flex-direction: column;
  -ms-flex-direction: column;
  flex-direction: column;
}

.product-title,
.price {
  text-transform: UPPERCASE;
  font-weight: bold;
}

.price span {
  color: #6b5b95;
}

.product-title,
.product-description,
.price {
  margin-bottom: 15px;
}

.product-title {
  margin-top: 0;
}

.add-to-cart {
  background: #6b5b95;
  padding: 1.2em 1.5em;
  border: none;
  text-transform: UPPERCASE;
  font-weight: bold;
  color: #fff;
  -webkit-transition: background 0.3s ease;
  transition: background 0.3s ease;
}
.add-to-cart:hover {
  background: #453a5f;
  color: #fff;
}

@-webkit-keyframes opacity {
  0% {
    opacity: 0;
    -webkit-transform: scale(3);
    transform: scale(3);
  }
  100% {
    opacity: 1;
    -webkit-transform: scale(1);
    transform: scale(1);
  }
}

@keyframes opacity {
  0% {
    opacity: 0;
    -webkit-transform: scale(3);
    transform: scale(3);
  }
  100% {
    opacity: 1;
    -webkit-transform: scale(1);
    transform: scale(1);
  }
}
</style>
