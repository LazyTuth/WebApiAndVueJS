<template>
    <div id="dashboard">
        <h1>This is Dashboard</h1>
        <p>You should only get here if authenticated</p>
        <p v-if="userFullName != null">Your Fullname: {{ userFullName }}</p>
    </div>
</template>

<script>
export default {
  computed: {
    userFullName() {
      return this.$store.getters.userFullname;
    }
  }
};
</script>
