<template>
  <v-container>
    <h2>Transactions</h2>
    <div class="table-container">
      <table class="table">
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
            <td>{{ transaction.accountCurrency}}</td>
            <td>{{ transaction.category }}</td> 
          </tr>
        </tbody>
      </table>
    </div>
  </v-container>
</template>

<style>
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

export default {
  name: 'Transactions',
  data() {
    return {
      transactions: [],
    };
  },
  methods: {
    userTransactions() {
      axios
        .get('https://localhost:44395/api/PocketPlanner/transactions')
        .then((response) => {
          if (response.data) {
            this.transactions = response.data;
          }
        })
        .catch((error) => {
          console.error(error);
        });
    },
  },
  created() {
    this.userTransactions();
  },
};
</script>
