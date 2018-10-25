export const pagingMixin = {
  data() {
    return {
      pageNumber: null,
      firstRowOnPage: null,
      lastRowOnPage: null,
      totalRows: null,
      hasPreviousPage: false,
      hasNextPage: false,
      activePageId: 1
    };
  },
  methods: {
    pagingOnClick(pageNumber) {
      this.activePageId = pageNumber;
      this.pageNumber = pageNumber;
    }
  }
};
