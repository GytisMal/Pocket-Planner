<template>
  <div>
    <h1>Transactions</h1>
    <input type="file" @change="handleFileUpload" />
      <p v-if="errorMessage">{{ errorMessage }}</p>
      <button @click="uploadFile">Upload</button>
    <v-data-table :headers="tableHeaders" :items="tableData" :hide-default-footer="true">
      <template v-for="header in tableHeaders" v-slot:[`item.${header.value}`]="{ item }">
        {{ item[header.value] }}
      </template>
    </v-data-table>
  </div>
</template>

<style scoped>
.table-container {
  max-height: 300px;
  overflow-y: auto;
}

.table {
  width: 100%;
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
</style>

<script>
import axios from 'axios';
import { mapActions, mapState } from 'vuex';

export default {
  name: 'Transactions',
  data() {
    return {
      tableHeaders: [],
      tableData: [],
      // transactions: [],
      // budgetSpentData: [],
      // budgetBalanceData: [],
      selectedFile: null,
      errorMessage: '',
    };
  },
  methods: {
    binaryToBase64(binaryString) {
      return window.btoa(binaryString);
    },
    handleFileUpload(event) {
      this.selectedFile = event.target.files[0];
    },
    uploadFile() {
      if (this.selectedFile) {
        const reader = new FileReader();
        reader.readAsBinaryString(this.selectedFile);

        reader.onload = async () => {
          try {
            const base64String = this.binaryToBase64(reader.result);
            const response = await axios.post(
              'https://localhost:7042/api/Transactions',
              { Base64Data: base64String },
              { headers: { 'Content-Type': 'application/json' } }
            );
            if(response.data && response.data.length > 0) {
              const firstRow = response.data[0];
              this.tableHeaders = Object.keys(firstRow).map(key => ({
                text: key,
                value: key,
              }));
              this.tableData = response.data;
              this.$store.commit('setTransactionsLoaded', true);
              this.spentBudget();
              this.budgetBalance();
            }
          } catch (error) {
            this.errorMessage = 'File upload failed. Please try again.';
            console.error(error);
          }
        };
      } else {
        this.errorMessage = 'Please select a file to upload.';
      }
    },
    spentBudget() {
      if (this.$store.state.transactionsLoaded) {
      this.$store.dispatch('fetchBudgetSpentData');
      }
    },
    ...mapActions(['fetchBudgetSpentData']),
    budgetBalance() {
      if (this.$store.state.transactionsLoaded) {
        this.$store.dispatch('fetchBudgetBalanceData');
      }
    },
    ...mapActions(['fetchBudgetBalanceData']),
  },
  created() {
    if (this.$store.state.transactionsLoaded) {
      this.spentBudget();
      this.budgetBalance();
    }
  },
  computed: {
    ...mapState(['budgetSpentData', 'transactionsLoaded', 'budgetBalanceData']),
  },
};
</script>
