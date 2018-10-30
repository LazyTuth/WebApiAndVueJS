<template>
    <div id="signin">
        <div class="signin-form">
          <form @submit.prevent="onSubmit">
              <div class="input" :class="{invalid: $v.username.$error}">
                  <label for="username">UserName</label>
                  <input type="text" id="username" @blur="$v.username.$touch()" v-model="username">
                  <p class="validate-message" 
                      :style="[($v.username.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.username.required">This field must not be empty.</p>
              </div>
              <div class="input" :class="{invalid: $v.password.$error}">
                  <label for="password">Password</label>
                  <input type="password" id="password" @blur="$v.password.$touch()" v-model="password">
                  <p class="validate-message" 
                      :style="[($v.password.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.password.required">This field must not be empty.</p>
              </div>
              <div class="submit">
                  <button :disabled="$v.$invalid" @click="$v.$touch()" type="submit">Submit</button>
              </div>
          </form>
        </div>
    </div>
</template>

<script>
import { modelStateErrorMixin } from "../../mixins/modelStateErrorMixin.js";
import { required } from "vuelidate/lib/validators";
export default {
  name: "signin",
  data: function() {
    return {
      username: "",
      password: ""
    };
  },
  methods: {
    onSubmit() {
      const formData = {
        username: this.username,
        password: this.password
      };
      this.$store.dispatch("signIn", formData);
    }
  },
  validations: {
    username: {
      required
    },
    password: {
      required
    }
  },
  mixins: [modelStateErrorMixin],
  mounted() {
    this.clearModelStateFunction();
  }
};
</script>

<style scoped>
.signin-form {
  width: 400px;
  margin: 30px auto;
  border: 1px solid #eee;
  padding: 20px;
  box-shadow: 0 2px 3px #ccc;
}

.input {
  margin: 10px auto;
}

.input label {
  display: block;
  color: #4e4e4e;
  margin-bottom: 6px;
}

.input input {
  font: inherit;
  width: 100%;
  padding: 6px 12px;
  box-sizing: border-box;
  border: 1px solid #ccc;
}

.input input:focus {
  outline: none;
  border: 1px solid #521751;
  background-color: #eee;
}

.submit button {
  border: 1px solid #521751;
  color: #521751;
  padding: 10px 20px;
  font: inherit;
  cursor: pointer;
}

.submit button:hover,
.submit button:active {
  background-color: #521751;
  color: white;
}

.submit button[disabled],
.submit button[disabled]:hover,
.submit button[disabled]:active {
  border: 1px solid #ccc;
  background-color: transparent;
  color: #ccc;
  cursor: not-allowed;
}
</style>
