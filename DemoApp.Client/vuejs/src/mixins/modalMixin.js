export const modalMixin = {
  data() {
    return {
      showModal: false,
      isModalCreate: true
    };
  },
  methods: {
    closeModal() {
      this.showModal = false;
      this.clearData();
      this.reset();
    }
  }
};
