<template>
    <header id="header">
        <nav class="navbar navbar-expand-lg navbar-light bg-light custom-navbar">
            <a class="navbar-brand" href="#">Demo App</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                    <router-link class="nav-item" tag="li" active-class="active" exact to="/">
                      <a class="nav-link">Dashboard</a>
                    </router-link>
                    <li v-if="isAuthenticated" class="nav-item dropdown">
                      <a class="nav-link dropdown-toggle" 
                                    id="navbarDropdown" 
                                    role="button" 
                                    data-toggle="dropdown" 
                                    aria-haspopup="true" 
                                    aria-expanded="false">Admin</a>
                      <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                          <router-link class="dropdown-item" to="admin/product">Product</router-link>
                      </div>
                    </li>
                </ul>
                <ul class="navbar-nav ml-auto">
                    <router-link v-if="!isAuthenticated" class="nav-item" tag="li" active-class="active" exact to="/signup">
                      <a class="nav-link">Sign Up</a>
                    </router-link>
                    <li v-if="!isAuthenticated" class="nav-item"><a class="nav-link">|</a></li>
                    <router-link v-if="!isAuthenticated" class="nav-item" tag="li" active-class="active" exact to="/signin">
                      <a class="nav-link">Sign In</a>
                    </router-link>
                    <li v-if="isAuthenticated" class="nav-item">
                      <a class="nav-link">Hello {{ userFullName }}</a>
                    </li>
                    <li v-if="isAuthenticated" class="nav-item"><a class="nav-link">|</a></li>
                    <li v-if="isAuthenticated" class="nav-item">
                      <button class="nav-link logout" @click="onLogout">Logout</button>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
</template>

<script>
import { mapGetters } from "vuex";
export default {
  computed: {
    ...mapGetters(["isAuthenticated"]),
    userFullName() {
      return this.$store.getters.userFullname;
    }
  },
  methods: {
    onLogout() {
      this.$store.dispatch("logOut");
    }
  }
};
</script>


<style scoped>
.custom-navbar {
  background-color: #521751 !important;
}
.custom-navbar > a {
  color: whitesmoke !important;
}
.nav-link {
  color: whitesmoke !important;
}
.logout {
  background-color: transparent;
  border: none;
  font: inherit;
  color: whitesmoke;
  cursor: pointer;
}
.nav-link:hover {
  color: rgb(116, 113, 113) !important;
}
</style>
