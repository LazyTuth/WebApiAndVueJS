export const modelStateErrorMixin = {
  computed: {
    modelState() {
      return this.$store.getters.modelState;
    }
  },
  methods: {
    clearModelStateFunction() {
      this.$store.commit("clearModelState");
    }
  }
};
