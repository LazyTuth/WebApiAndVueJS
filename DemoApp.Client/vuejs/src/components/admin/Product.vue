<template>
    <div id="product-screen">
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th class="table-header-custom">Product</th>
                    <th class="table-header-custom">Type</th>
                    <th class="table-header-custom">Price</th>
                    <th class="table-header-custom" style="width: 110px;">
                        <span class="fas fa-plus-circle btn-icon-color" @click="onClickShowModal('create')" style="font-size: 18px;" title="Add New"></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr v-if="lstDataObjects.length > 0" v-for='product in lstDataObjects' :key='product.productName'>
                    <td>{{ product.productName }}</td>
                    <td>{{ product.productCateName }}</td>
                    <td>{{ product.price | to-currency }}</td>
                    <td>
                        <button @click="onClickShowModal('edit', product.id)" class="btn btn-link"><span class="far fa-edit btn-icon-color" title="Edit"></span></button>
                        <button @click="onDeleteButtonClick(product.id)" class="btn btn-link"><span class="far fa-trash-alt btn-icon-color" title="Remove"></span></button>
                    </td>
                </tr>
                <tr v-if="lstDataObjects.length <= 0">
                    <td colspan="4">No Data</td>
                </tr>
            </tbody>
        </table>
        <div class="flex_column_custom">
            <div class="custom-paging-info">
                Items {{firstRowOnPage}} to {{lastRowOnPage}} of {{totalRows}}
            </div>
            <div class="custom-paging-bar pull-right">
                <ul class="pagination" style="margin: 0px;">
                    <li @click="onChangePageNumber(pageNumber - 1)" class="page-item" v-if="hasPreviousPage"><a class="page-link">&larr; Previous</a></li>
                    <li @click="onChangePageNumber(pageNumber - 1)" :class="{active: activePageId === pageNumber - 1}" class="page-item" v-if="hasPreviousPage"><a class="page-link">{{pageNumber - 1}}</a></li>
                    <li @click="onChangePageNumber(pageNumber)" :class="{active: activePageId === pageNumber}" class="page-item active"><a class="page-link">{{pageNumber}}</a></li>
                    <li @click="onChangePageNumber(pageNumber + 1)" :class="{active: activePageId === pageNumber + 1}" class="page-item" v-if="hasNextPage"><a class="page-link">{{pageNumber + 1}}</a></li>
                    <li @click="onChangePageNumber(pageNumber + 1)" class="page-item" v-if="hasNextPage"><a class="page-link">Next &rarr;</a></li>
                </ul>
            </div>
        </div>
        
        <app-modal v-if="showModal">
          <h5 slot="header" class="modal-title">{{modalTitle}}</h5>
          <div slot="body">
            <form id="formCreateProduct" @submit.prevent="onFormSubmit">
              <div class="form-group">
                <label for="productName">Product Name</label>
                <input type="text" class="form-control" id="productName" name="productName" v-model="productName">
              </div>
              <div class="form-group">
                <label for="description">Description</label>
                <input type="text" class="form-control" id="description" name="description" v-model="description">
              </div>
              <div class="form-group currency-input">
                <label for="price">Price</label>
                <input type="text" class="form-control" id="price" name="price" @keyup="onKeyUpFunction($event)" v-model="newDisplay">
                <span class="currency-symbol">$</span>
              </div>
              <div class="form-group">
                <label for="price">Images</label>
                <input type="text" class="form-control" id="images" name="images" v-model="imagesUrl">
              </div>
              <div class="form-group">
                <label for="productCate">Product Category</label>
                <select class="form-control" name="productCate" id="productCate" v-model="selected" @change="onChangeProductCate($event)">
                  <option v-for="prd in dataProductCate" :value="prd.cateCode" :key="prd.cateCode">{{prd.description}}</option>
                </select>
              </div>
              <hr>
              <div class="form-inline" style="justify-content: flex-end;">
                <button type="submit" class="btn btn-primary" style="margin-right: 10px;">Save changes</button>
                <button @click="closeModal()" type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
              </div>
            </form>
          </div>
          
        </app-modal>
    </div>
</template>

<style>
.flex_column_custom {
  display: flex;
  justify-content: flex-end;
}

.custom-paging-info {
  display: flex;
  align-items: center;
  margin-right: 15px;
}

.custom-paging-bar {
  display: flex;
  align-items: center;
}

.pagination > li {
  -webkit-user-select: none; /* Chrome/Safari */
  -moz-user-select: none; /* Firefox */
  -ms-user-select: none; /* IE10+ */

  /* Rules below not implemented in browsers yet */
  -o-user-select: none;
  user-select: none;
}

.btn-icon-color {
  color: #6b5b95;
}
.btn-icon-color:hover {
  color: #453a5f;
  cursor: pointer;
}

.table-header-custom {
  text-align: center;
}

.currency-input {
  position: relative;
}
.currency-symbol {
  position: absolute;
  left: 0;
  top: 73%;
  width: 1rem;
  text-align: right;
  transform: translateY(-50%);
  opacity: 0.5;
}
.currency-input > input {
  padding-left: 1rem;
  width: 100%;
}
</style>

<script>
import { modalMixin } from "../../mixins/modalMixin.js";
import { pagingMixin } from "../../mixins/pagingMixin.js";
import { formatPriceMixin } from "../../mixins/formatPriceMixin.js";
export default {
  data: function() {
    return {
      lstDataObjects: [],
      productSelectedId: null,
      productName: null,
      description: null,
      price: null,
      imagesUrl: null,
      isInputActive: false,
      selected: null,
      dataProductCate: [],
      modalTitle: null
    };
  },
  methods: {
    async onChangePageNumber(page) {
      const self = this;
      await this.$store
        .dispatch("getProductPagination", {
          page: page
        })
        .then(res => {
          if (res) {
            if (res.items) {
              self.lstDataObjects = res.items;
              self.firstRowOnPage = res.paging.firstRowOnPage;
              self.lastRowOnPage = res.paging.lastRowOnPage;
              self.totalRows = res.paging.totalItems;
              self.hasNextPage = res.paging.hasNextPage;
              self.hasPreviousPage = res.paging.hasPreviousPage;
              self.pageNumber = res.paging.pageNumber;
              self.activePageId = res.paging.pageNumber;
            }
          }
        })
        .catch(err => console.log(err));
    },
    async onFormSubmit() {
      const formData = {
        ProductName: this.productName,
        Description: this.description,
        Price: parseInt(this.price),
        ImageUrl: this.imagesUrl,
        ProductCateCode: this.selected
      };
      if (this.isModalCreate) {
        await this.$store.dispatch("createProduct", formData).then(res => {
          if (res === 201) {
            this.closeModal();
            this.onChangePageNumber(this.pageNumber);
          }
        });
      } else {
        formData.productId = this.productSelectedId;
        await this.$store.dispatch("updateProduct", formData).then(res => {
          if (res === 201) {
            this.closeModal();
            this.onChangePageNumber(this.pageNumber);
          }
        });
      }
    },
    async onClickShowModal(type, productId) {
      const self = this;
      await this.$store.dispatch("getProductCategory").then(res => {
        self.dataProductCate = res;
      });
      if (type !== "create") {
        this.modalTitle = "Edit Product";
        this.isModalCreate = false;
        await this.$store
          .dispatch("getProductById", { id: productId })
          .then(res => {
            self.productSelectedId = productId;
            self.productName = res.productName;
            self.description = res.description;
            self.price = res.price;
            self.imagesUrl = res.imageUrl;
            self.selected = res.productCateCode;
          })
          .catch(err => console.log(err));
      } else {
        this.modalTitle = "Create Product";
        this.selected = this.dataProductCate[0].cateCode;
      }
      this.showModal = true;
    },
    onDeleteButtonClick(id) {
      const self = this;
      this.$swal({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
      }).then(result => {
        if (result.value) {
          self.$store.dispatch("deleteProduct", id).then(res => {
            if (res === 204) {
              self.$swal("Deleted!", "Your file has been deleted.", "success");
              this.onChangePageNumber(this.pageNumber);
            }
          });
        }
      });
    },
    onKeyUpFunction(e) {
      this.price = e.target.value;
    },
    onChangeProductCate(e) {
      this.selected = e.target.value;
    },
    clearData() {
      this.productSelectedId = null;
      this.productName = null;
      this.description = null;
      this.price = null;
      this.imagesUrl = null;
      this.selected = null;
      this.isModalCreate = true;
    }
  },
  mixins: [modalMixin, pagingMixin, formatPriceMixin],
  mounted() {
    this.onChangePageNumber();
  }
};
</script>
