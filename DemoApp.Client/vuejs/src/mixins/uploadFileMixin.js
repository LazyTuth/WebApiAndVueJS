export const uploadFileMixin = {
  data() {
    return {
      uploadedFiles: [],
      uploadError: null,
      currentStatus: null,
      uploadFieldName: "photos",
      listFileToUpload: [],
      statusInitial: 0,
      statusSaving: 1,
      statusSuccess: 2,
      statusFailed: 3
    };
  },
  computed: {
    isInitial() {
      return this.currentStatus === this.statusInitial;
    },
    isSaving() {
      return this.currentStatus === this.statusSaving;
    },
    isSuccess() {
      return this.currentStatus === this.statusSuccess;
    },
    isFailed() {
      return this.currentStatus === this.statusFailed;
    }
  },
  methods: {
    reset() {
      this.currentStatus = this.statusInitial;
      this.uploadedFiles = [];
      this.uploadError = null;
      this.listFileToUpload = [];
    },
    filesChange(fieldName, fileList) {
      // handle file changes
      if (!fileList.length) {
        return;
      }
      this.listFileToUpload = Object.values(fileList);
      this.currentStatus = this.statusSaving;
      const self = this;
      setTimeout(() => {
        self.listFileToUpload.forEach(element => {
          const reader = new FileReader();
          reader.onload = function(e) {
            const image = document.createElement("img");
            image.src = e.target.result;
            image.style = "width: 100px;";
            document.getElementById("preloadFiles").appendChild(image);
          };
          reader.readAsDataURL(element);
        });
        this.currentStatus = this.statusSuccess;
      }, 2000);
    }
  }
};
