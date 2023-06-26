<template>
  <v-container>
    <h2>Budgets</h2>
    <div class="table-container">
      <table class="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Type</th>
            <th>Amount</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="budget in budgets" :key="budget.id">
            <td>{{ budget.id }}</td>
            <td>
              <input v-model="budget.type" />
            </td>
            <td>
              <input v-model="budget.amount" />
            </td>
            <td>
              <input v-model="budget.date" />
            </td>
            <td>
              <button @click="updateBudget(budget)">Update</button>
              <button @click="deleteBudget(budget.id)">Delete</button>
            </td>
          </tr>
          <tr>
            <td></td>
            <td>
              <input v-model="newBudget.type" placeholder="Enter budget type" />
            </td>
            <td>
              <input v-model="newBudget.amount" placeholder="Enter budget amount" />
            </td>
            <td>
              <input v-model="newBudget.date" placeholder="Enter budget date" />
            </td>
            <td>
              <button @click="addBudget">Add Budget</button>
            </td>
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
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
  
<script>
import axios from 'axios';
  
  export default {
    name: 'Budgets',
    data() {
      return {
        budgets: [],
        newBudget: {
          id: "",
          type: "",
          amount: "",
          date: ""
        }
      };
    },
    methods: {
      userBudget() {
        axios.get('https://localhost:44395/api/PocketPlanner/budget').then((response) => {
            if (response.data) {
                this.budgets = response.data;
            }
          })
          .catch((error) => {
            console.error(error);
          });
      },
      updateBudget(budget) {
        axios.put(`https://localhost:44395/api/PocketPlanner/budget/${budget.id}`, budget).then(response => {
          if (response.data) {
            console.log(response); //debug
            this.budgets = response.data;
            console.log("Gudget update:", budget); //debug
          }
          this.userBudget();
        })
        .catch(error => {
          console.error(error);
        });
      },
      addBudget() {
        axios.post("https://localhost:44395/api/PocketPlanner/budget", this.newBudget).then(response => {
          if(response.data) {
            this.budgets.push(response.data);
            this.newBudget = {
              id: "",
              type: "",
              amount: "",
              date: ""
            }
          }
          this.userBudget();
        })
        .catch(error => {
          console.error(error);
        })
      },
      deleteBudget(id) {
        this.budgets.splice(id, 1);
        axios.delete(`https://localhost:44395/api/PocketPlanner/budget/${id}`).then(response => {
          if (response.data) {
            this.budgets = response.data;
          }
          this.userBudget();
        })
        .catch(error => {
          console.error(error);
        })
      }
   },
    created() {
      this.userBudget();
    }
};
</script>