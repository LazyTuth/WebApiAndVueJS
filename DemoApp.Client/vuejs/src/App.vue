<template>
  <div id="app">
    <app-header />
    <div class="container">
      <router-view/>
    </div>
  </div>
</template>

<script>
import Header from "./components/header/Header";
export default {
  name: "app",
  components: {
    "app-header": Header
  },
  created() {
    this.$store.dispatch("keepLogin");
  }
};
</script>

<style>
body,
html {
  margin: 0;
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
}
</style>
