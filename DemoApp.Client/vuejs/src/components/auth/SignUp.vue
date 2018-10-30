<template>
    <div id="signup">
        <div class="signup-form">
          <form @submit.prevent="onSubmit">
              <div class="input" :class="{invalid: $v.firstname.$error}">
                  <label for="firstname">Firstname</label>
                  <input type="text" id="firstname" @blur="$v.firstname.$touch()" v-model="firstname">
                  <p v-if="!$v.firstname.maxLen" 
                      class="validate-message">This field cannot have more than {{$v.firstname.$params.maxLen.max}} characters.</p>
                  <p class="validate-message" 
                      :style="[($v.firstname.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.firstname.required">This field must not be empty.</p>
              </div>
              <div class="input" :class="{invalid: $v.lastname.$error}">
                  <label for="lastname">Lastname</label>
                  <input type="text" id="lastname" @blur="$v.lastname.$touch()" v-model="lastname">
                  <p v-if="!$v.lastname.maxLen" 
                      class="validate-message">This field cannot have more than {{$v.lastname.$params.maxLen.max}} characters.</p>
                  <p class="validate-message" 
                      :style="[($v.lastname.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.lastname.required">This field must not be empty.</p>
              </div>
              <div class="input" :class="{invalid: $v.email.$error}">
                  <label for="email">Email</label>
                  <input type="email" id="email" @blur="$v.email.$touch()" v-model="email">
                  <p class="validate-message" v-if="!$v.email.email">Please provide valid email address.</p>
                  <p class="validate-message" 
                      :style="[($v.email.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.email.required">This field must not be empty.</p>
              </div>
              <div class="input" :class="{invalid: $v.username.$error}">
                  <label for="username">Username</label>
                  <input type="text" id="username" @blur="$v.username.$touch()" v-model="username">
                  <p class="validate-message" v-if="!$v.username.minLen">Username must larger than {{$v.username.$params.minLen.min}} characters.</p>
                  <p class="validate-message" v-if="!$v.username.maxLen">Username Length must be between {{$v.username.$params.minLen.min}} and {{$v.username.$params.maxLen.max}} characters.</p>
                  <p class="validate-message" 
                      :style="[($v.username.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.username.required">This field must not be empty.</p>
              </div>
              <div class="input" :class="{invalid: $v.password.$error}">
                  <label for="password">Password</label>
                  <input type="password" id="password" @blur="$v.password.$touch()" v-model="password">
                  <p class="validate-message" v-if="!$v.password.minLen">Password must be at least {{$v.password.$params.minLen.min}} characters.</p>
                  <p class="validate-message" 
                      :style="[($v.password.$error) ? {'display': 'block'} : {'display': 'none'}]" 
                      v-if="!$v.password.required">This field must not be empty.</p>
              </div>
              <div class="input" :class="{invalid: $v.confirmPassword.$error}">
                  <label for="confirm-password">Confirm password</label>
                  <input type="password" @blur="$v.confirmPassword.$touch()" id="confirm-password" v-model="confirmPassword">
                  <p v-if="!$v.confirmPassword.sameAs && $v.confirmPassword.$model != ''">Confirm Password does not match.</p>
              </div>
              <div class="submit">
                  <button type="submit" :disabled="$v.$invalid" @click="$v.$touch()">Submit</button>
              </div>
          </form>
        </div>
    </div>
</template>

<script>
import { modelStateErrorMixin } from "../../mixins/modelStateErrorMixin.js";
import {
  required,
  email,
  maxLength,
  minLength,
  sameAs
} from "vuelidate/lib/validators";
export default {
  name: "signup",
  data: function() {
    return {
      firstname: "",
      lastname: "",
      email: "",
      username: "",
      password: "",
      confirmPassword: ""
    };
  },
  validations: {
    email: {
      required,
      email
    },
    firstname: {
      maxLen: maxLength(20),
      required
    },
    lastname: {
      required,
      maxLen: maxLength(50)
    },
    username: {
      minLen: minLength(4),
      maxLen: maxLength(10),
      required
    },
    password: {
      minLen: minLength(6),
      required
    },
    confirmPassword: {
      sameAs: sameAs("password")
    }
  },
  methods: {
    onSubmit() {
      const formData = {
        firstname: this.firstname,
        lastname: this.lastname,
        email: this.email,
        username: this.username,
        password: this.password,
        confirmPassword: this.confirmPassword
      };
      this.$store.dispatch("signUp", formData);
    }
  },
  mixins: [modelStateErrorMixin],
  mounted() {
    this.clearModelStateFunction();
  }
};
</script>

<style scoped>
.signup-form {
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

.input.inline label {
  display: inline;
}

.input input {
  font: inherit;
  width: 100%;
  padding: 6px 12px;
  box-sizing: border-box;
  border: 1px solid #ccc;
}

.input.inline input {
  width: auto;
}

.input input:focus {
  outline: none;
  border: 1px solid #521751;
  background-color: #eee;
}

.input select {
  border: 1px solid #ccc;
  font: inherit;
}

.hobbies button {
  border: 1px solid #521751;
  background: #521751;
  color: white;
  padding: 6px;
  font: inherit;
  cursor: pointer;
}

.hobbies button:hover,
.hobbies button:active {
  background-color: #8d4288;
}

.hobbies input {
  width: 90%;
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
