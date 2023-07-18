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
            <td>{{ budget.type }}</td>
            <td>{{ budget.amount }}</td>
            <td>{{ getSpentAmount(budget.type) }}</td>
            <td>{{ getBalance(budget.type) }}</td>
            <td>{{ budget.date }}</td>
            <td>
              <v-icon @click="openUpdateDialog(budget)">mdi-pencil</v-icon>
              <v-icon @click="deleteBudget(budget.id)">mdi-delete</v-icon>
            </td>
          </tr>
        </tbody>
      </v-table>
    </div>
  </v-container>
  <canvas id="budgetChart"></canvas>
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
import { Chart, BarController, BarElement, CategoryScale, LinearScale } from 'chart.js';
import 'chartjs-plugin-datalabels'; 

Chart.register(BarController, BarElement, CategoryScale, LinearScale); 
  
export default {
    name: 'Budgets',
    components: {
      AddBudgetDialog,
      UpdateBudgetDialog,
    },
    data() {
      return {
        budgets: [],
      };
    },
    methods: {
      userBudget() {
        this.$axios.get('Budget').then((response) => {
            if (response.data) {
                this.budgets = response.data.data;
                this.createChart(); 
            }
          })
          .catch((error) => {
            console.error(error);
          });
      },
      addNewBudget(newBudget) {
        this.budgets.push(newBudget);
        this.createChart();
      },
      deleteBudget(id) {
          this.$axios.delete(`Budget/${id}`).then(response => {
            if (response.data) {
              this.budgets = response.data.data;
              this.createChart();
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
    createChart() {
      const labels = this.budgets.map(budget => budget.type);
      const plannedData = this.budgets.map(budget => budget.amount);
      const spentData = labels.map(label => this.getSpentAmount(label));
      const balanceData = labels.map(label => this.getBalance(label));

      if (this.budgetChart) {
        this.budgetChart.destroy();
      }

      this.budgetChart = new Chart(document.getElementById('budgetChart'), {
        type: 'bar',
        data: {
          labels: labels,
          datasets: [{
            label: 'Planned',
            data: plannedData,
            backgroundColor: 'rgba(0, 123, 255, 0.2)', 
            borderColor: 'rgba(0, 123, 255, 1)', 
            borderWidth: 1
          }, {
            label: 'Spent',
            data: spentData,
            backgroundColor: 'rgba(75, 192, 192, 0.2)', 
            borderColor: 'rgba(75, 192, 192, 1)', 
            borderWidth: 1
          },{
            label: 'Balance',
            data: balanceData,
            backgroundColor: 'rgba(255, 99, 132, 0.2)', 
            borderColor: 'rgba(255, 99, 132, 1)', 
            borderWidth: 1
          }]
        },
        options: {
          responsive: true,
          scales: {
            y: {
              beginAtZero: true
            }
          },
          plugins: {
            title: {
              display: true,
              text: 'Budget Overview',
            },
            legend: {
              display: true,
              position: 'bottom',
            },
            tooltip: {
              callbacks: {
                title: function(tooltipItem) {
                  return tooltipItem[0].label;
                },
                label: function(tooltipItem) {
                  return tooltipItem.raw;
                }
              }
            },
            datalabels: {
              color: '#000000',
              anchor: 'end',
              align: 'top',
              formatter: function(value, context) {
                return value;
              },
              display: function(context) {
                return context.dataset.data[context.dataIndex] > 0; // Display labels only for positive values
              },
              font: {
                weight: 'bold'
              }
            }
          }
        }
      });
    }
  },
  computed: {
    ...mapState(['transactionsLoaded', 'budgetSpentData', 'budgetBalanceData'])
  },
  mounted() {
    this.userBudget();
    if (this.transactionsLoaded) {
      this.fetchBudgetSpentData();
      this.fetchBudgetBalanceData();
      this.createChart();
    }
  }
};
</script>