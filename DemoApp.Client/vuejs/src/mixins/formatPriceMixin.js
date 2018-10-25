export const formatPriceMixin = {
  computed: {
    newDisplay: {
      get: function() {
        if (this.price) {
          let input = this.price.toString().replace(/[\D\s_]+/g, "");
          input = input ? parseInt(input, 10) : 0;

          return input === 0 ? "" : input.toLocaleString("en-US");
        }
      },
      set: function() {}
    }
  }
};
