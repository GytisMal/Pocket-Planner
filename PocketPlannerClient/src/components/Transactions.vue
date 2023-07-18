<template>
  <v-container>
    <h2>Transactions</h2>
    <v-alert
      v-model="showSuccessAlert"
      type="success"
      dismissible
    >
      File uploaded successfully!
    </v-alert>
    <v-alert
      v-model="showErrorAlert"
      type="error"
      dismissible
    >
      {{ errorMessage }}
    </v-alert>
    <div>
      <input type="file" @change="handleFileUpload" />
    </div>
    <div class="table-container">
      <v-table class="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>IBAN</th>
            <th>Date</th>
            <th>Amount</th>
            <th>IsCredit</th>
            <th>Payee</th>
            <th>PayeeIBAN</th>
            <th>Description</th>
            <th>InternalDescription</th>
            <th>AccountCurrency</th>
            <th>Category</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="transaction in transactions" :key="transaction.id">
            <td>{{ transaction.id }}</td>
            <td>{{ transaction.iban }}</td>
            <td>{{ transaction.date }}</td>
            <td>{{ transaction.amount }}</td>
            <td>{{ transaction.isCredit }}</td>
            <td>{{ transaction.payee }}</td>
            <td>{{ transaction.payeeIBAN }}</td>
            <td>{{ transaction.description }}</td>
            <td>{{ transaction.internalDescription }}</td>
            <td>{{ transaction.accountCurrency }}</td>
            <td>{{ transaction.category }}</td>
          </tr>
        </tbody>
      </v-table>
    </div>
  </v-container>
</template>

<style scoped>
.table-container {
  max-height: 400px;
  overflow-y: auto;
  overflow-x: auto;
  position: sticky;
  bottom: 0;
}
.table {
  min-width: max-content;
  border-collapse: collapse;
}
.table thead th {
  background-color: #f5f5f5;
}
.table tbody tr:nth-child(odd) {
  background-color: #f9f9f9;
}
.table th,
.table td {
  padding: 8px;
  border: 1px solid #ddd;
}
.table td {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.v-text-field {
  width: 100%;
}
</style>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  name: 'Transactions',
  data() {
    return {
      selectedFile: null,
      errorMessage: '',
      showSuccessAlert: false,
      showErrorAlert: false,
    };
  },
  methods: {
    binaryToBase64(binaryString) {
      return window.btoa(binaryString);
    },
    handleFileUpload(event) {
      this.selectedFile = event.target.files[0];
      if (this.selectedFile) {
        this.uploadFile();
      }
      else {
        this.errorMessage = "Please select a file to upload.";
        this.showErrorAlert = true;
        setTimeout(() => {
          this.showErrorAlert = false
        }, 4000 );
      }
    },
    uploadFile() {
      const reader = new FileReader();
      reader.readAsBinaryString(this.selectedFile);

        reader.onload = async () => {
          try {
            const base64String = this.binaryToBase64(reader.result);
            const response = await this.$axios.post('Transactions',
              { Base64Data: base64String },
              { headers: { 'Content-Type': 'application/json' } }
            );
            this.$store.commit('setTransactions', response.data)
            this.showSuccessAlert = true;
            setTimeout(() => {
              this.showSuccessAlert = false;
            }, 4000 );
          } catch (error) {
            this.errorMessage = 'File upload failed. Please try again.';
            this.showErrorAlert = true;
            console.error(error);
            setTimeout(() => {
              this.showErrorAlert = false;
            }, 4000);
          }
        };
      } 
    },
    ...mapActions(['fetchTransactions']),
  computed: {
    ...mapState(['transactions']),
  },
};
</script>
