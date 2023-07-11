<template>
  <v-container>
    <UpdateBudgetDialog ref="updateDialog" @updateBudget="updateBudget" />
    <AddBudgetDialog @addNewBudget="addNewBudget" />
    <h2>Budgets</h2>
    <div class="table-container">
      <v-table class="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Type</th>
            <th>Amount</th>
            <th>Spent</th>
            <th>Balance</th>
            <th>Date</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="budget in budgets" :key="budget && budget.id">
            <td>{{ budget.id }}</td>
            <td>
              <input v-model="budget.type" />
            </td>
            <td>
              <input v-model="budget.amount" />
            </td>
            <td>{{ getSpentAmount(budget.type) }}</td>
            <td>{{ getBalance(budget.type) }}</td>
            <td>
              <input v-model="budget.date" />
            </td>
            <td>
              <v-icon @click="openUpdateDialog(budget)">mdi-pencil</v-icon>
              <v-icon @click="deleteBudget(budget.id)">mdi-delete</v-icon>
            </td>
          </tr>
        </tbody>
      </v-table>
    </div>
  </v-container>
</template>

<style>
.table-container {
 max-height: 400px;
 overflow-y: auto;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table thead th{
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
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
  
<script>
import { mapState } from 'vuex';
import { mapActions } from 'vuex';
import AddBudgetDialog from './AddBudgetDialog.vue';
import UpdateBudgetDialog from './UpdateBudgetDialog.vue';
  
export default {
    name: 'Budgets',
    components: {
      AddBudgetDialog,
      UpdateBudgetDialog,
    },
    data() {
      return {
        budgets: [],
        budgetSpentData: {},
        budgetBalanceData: {},
      };
    },
    methods: {
      userBudget() {
        this.$axios.get('Budget').then((response) => {
            if (response.data) {
                this.budgets = response.data.data;
            }
          })
          .catch((error) => {
            console.error(error);
          });
      },
      addNewBudget(newBudget) {
        this.budgets.push(newBudget);
      },
      deleteBudget(id) {
          this.$axios.delete(`Budget/${id}`).then(response => {
            if (response.data) {
              this.budgets = response.data.data;
            }
        })
        .catch(error => {
          console.error(error);
        })
      },
      getSpentAmount(type) {
        if (this.$store.state.transactionsLoaded) {
        const spentAmount = this.$store.state.budgetSpentData[type] || 0;
        return typeof spentAmount === 'object' ? spentAmount.amount : spentAmount;
      }
        return 0;
      },
    ...mapActions(['fetchBudgetSpentData']),
    getBalance(type) {
      if(this.$store.state.transactionsLoaded) {
        const leftAmount = this.$store.state.budgetBalanceData[type] || 0;
        return typeof leftAmount === "object" ? leftAmount.amount : leftAmount;
      }
      return 0;
    },
    ...mapActions(['fetchBudgetBalanceData']),
    openUpdateDialog(budget) {
        this.$refs.updateDialog.open(budget);
      },
    updateBudget(updatedBudget) {
      const index = this.budgets.findIndex(budget => budget.id == updatedBudget.id);
      if (index !== -1) {
        this.budgets[index] = updatedBudget;
      }
    },
  },
  computed: {
    ...mapState(['budgetSpentData', 'transactionsLoaded', 'budgetBalanceData'])
  },
  created() {
    this.userBudget();
    if (this.transactionsLoaded) {
      this.fetchBudgetSpentData();
      this.fetchBudgetBalanceData();
    }
  }
};
</script>