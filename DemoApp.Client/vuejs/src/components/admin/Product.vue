<template>
    <div id="product-screen">
      <h1>Product management</h1>
      <hr>
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
      <div class="flex_column_custom" v-if="lstDataObjects.length > 0">
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
            <div class="form-group" :class="{invalid: $v.productName.$error}">
              <label for="productName">Product Name</label>
              <input type="text" class="form-control" id="productName" name="productName" @blur="$v.productName.$touch()" v-model="productName">
              <p class="validate-message" v-if="!$v.productName.minLen">Product Name must larger than {{$v.productName.$params.minLen.min}} characters.</p>
              <p class="validate-message" v-if="!$v.productName.maxLen">Product Name Length must be between {{$v.productName.$params.minLen.min}} and {{$v.productName.$params.maxLen.max}} characters.</p>
              <p class="validate-message" 
                    :style="[($v.productName.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                    v-if="!$v.productName.required">This field must not be empty.</p>
            </div>
            <div class="form-group" :class="{invalid: $v.description.$error}">
              <label for="description">Description</label>
              <input type="text" class="form-control" id="description" name="description" @blur="$v.description.$touch()" v-model="description">
              <p v-if="!$v.description.maxLen" 
                    class="validate-message">This field cannot have more than {{$v.description.$params.maxLen.max}} characters.</p>
            </div>
            <div class="form-group" :class="{invalid: $v.newDisplay.$error}">
              <div class="currency-input">
                <label for="price">Price</label>
                <input type="text" class="form-control" id="price" name="price" @keyup="onKeyUpFunction($event)" @blur="$v.newDisplay.$touch()" v-model="newDisplay">
                <span class="currency-symbol">$</span>
              </div>
              <div>
                <p class="validate-message" 
                      :style="[($v.newDisplay.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.newDisplay.required">This field must not be empty.</p>
              </div>
            </div>
            <div v-if="(isModalCreate && !isReUploadImages)||(!isModalCreate && isReUploadImages)" class="form-group">
              <label for="price">Images</label>
              <!-- <input type="text" class="form-control" id="images" name="images" v-model="imagesUrl"> -->
              <div class="dropbox" v-if="isInitial || isSaving">
                <input type="file" multiple name="Image" ref="image" :disabled="isSaving" @change="filesChange($event.target.name, $event.target.files); fileCount = $event.target.files.length"
                  accept="image/*" class="input-file">
                <p v-if="isInitial">
                  Drag your file(s) here to begin<br> or click to browse
                </p>
                <p v-if="isSaving">
                  Uploading {{ fileCount }} files...
                </p>
              </div>

              <div v-if="isSuccess">
                <p>
                  <a href="javascript:void(0)" @click="reset()">Upload again</a>
                </p>
                <div class="list-unstyled" id="preloadFiles"></div>
              </div>
            </div>
            <div v-if="justForEditPopUp" class="form-group">
              <label for="price">Images</label>
              <div>
                <p>
                  <a href="javascript:void(0)" @click="reset(); justForEditPopUp = false; isReUploadImages = true">Upload again ? It will remove all Old images</a>
                </p>
                <div class="list-unstyled">
                  <img v-for="image in listProductImagesEdit" style="width:100px;" :key="image" :src="rootApiUrl + imagesUrl + '/' + image" :alt="productName">
                </div>
              </div>
            </div>
            <div class="form-group">
              <label for="productCate">Product Category</label>
              <select class="form-control" name="productCate" id="productCate" v-model="selected" @change="onChangeProductCate($event)">
                <option v-for="prd in dataProductCate" :value="prd.cateCode" :key="prd.cateCode">{{prd.description}}</option>
              </select>
            </div>
            <hr>
            <div class="form-inline" style="justify-content: flex-end;">
              <button type="submit" @click="$v.$touch()" class="btn btn-primary" style="margin-right: 10px;">Save changes</button>
              <button @click="closeModal()" type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            </div>
          </form>
        </div>
        
      </app-modal>
    </div>
</template>

<style scoped>
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

.dropbox {
  outline: 2px dashed whitesmoke; /* the dash box */
  outline-offset: -10px;
  background: #6b5b95;
  color: whitesmoke;
  padding: 10px 10px;
  min-height: 200px; /* minimum height */
  position: relative;
  cursor: pointer;
}

.input-file {
  opacity: 0; /* invisible but it's there! */
  width: 100%;
  height: 200px;
  position: absolute;
  cursor: pointer;
}

.dropbox:hover {
  background: #453a5f; /* when mouse over to the drop zone, change color */
}

.dropbox p {
  font-size: 1.2em;
  text-align: center;
  padding: 50px 0;
}
</style>

<script>
import { modalMixin } from "../../mixins/modalMixin.js";
import { pagingMixin } from "../../mixins/pagingMixin.js";
import { formatPriceMixin } from "../../mixins/formatPriceMixin.js";
import { uploadFileMixin } from "../../mixins/uploadFileMixin.js";
import { required, maxLength, minLength } from "vuelidate/lib/validators";
export default {
  data: function() {
    return {
      rootApiUrl: "http://localhost:5000/",
      lstDataObjects: [],
      productSelectedId: null,
      productName: null,
      description: null,
      price: null,
      imagesUrl: null,
      isInputActive: false,
      selected: null,
      dataProductCate: [],
      modalTitle: null,
      isReUploadImages: false,
      justForEditPopUp: false,
      listProductImagesEdit: []
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
      const formDataTest = new FormData();
      if (this.listFileToUpload.length > 0) {
        this.listFileToUpload.forEach(element => {
          formDataTest.append(element.name, element);
        });
      }

      formDataTest.append("ProductName", this.productName);
      formDataTest.append("Description", this.description);
      formDataTest.append("Price", parseInt(this.price));
      formDataTest.append("ProductCateCode", this.selected);
      if (this.isModalCreate) {
        await this.$store.dispatch("createProduct", formDataTest).then(res => {
          if (res === 201) {
            this.closeModal();
            this.onChangePageNumber(this.pageNumber);
          }
        });
      } else {
        //formData.productId = this.productSelectedId;
        formDataTest.append("ProductId", this.productSelectedId);
        formDataTest.append("isAddNewFiles", this.isReUploadImages);
        await this.$store.dispatch("updateProduct", formDataTest).then(res => {
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
        this.justForEditPopUp = true;
        await this.$store
          .dispatch("getProductById", { id: productId })
          .then(res => {
            self.productSelectedId = productId;
            self.productName = res.productName;
            self.description = res.description;
            self.price = res.price;
            self.imagesUrl = res.imageUrl;
            self.selected = res.productCateCode;
            self.listProductImagesEdit = res.listImages;
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
      this.isReUploadImages = false;
      this.justForEditPopUp = false;
      this.listProductImagesEdit = [];
    }
  },
  validations: {
    productName: {
      required,
      minLen: minLength(5),
      maxLen: maxLength(50)
    },
    description: {
      maxLen: maxLength(100)
    },
    newDisplay: {
      required
    }
  },
  mixins: [modalMixin, pagingMixin, formatPriceMixin, uploadFileMixin],
  mounted() {
    this.onChangePageNumber();
    this.reset();
  }
};
</script>
