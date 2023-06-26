<template>
    <div>
      <h2>Categories</h2>
      <div class="table-container">
        <table class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Pattern</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="category in categories" :key="category.id">
              <td>{{ category.id }}</td>
              <td>
                <input v-model="category.name" />
              </td>
              <td>
                <input v-model="category.pattern" />
              </td>
              <td>
                <button @click="updateCategory(category)">Update</button>
                <button @click="deleteCategory(category.id)">Delete</button>
              </td>
            </tr>
            <tr>
              <td></td>
              <td>
                <input v-model="newCategory.name" placeholder="Enter category name" />
              </td>
              <td>
                <input v-model="newCategory.pattern" placeholder="Enter category pattern" />
              </td>
              <td>
                <button @click="addCategory">Add Category</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
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
import axios from "axios";

export default {
  name: "Categories",
  data() {
    return {
      categories: [],
      newCategory: {
        id: "",
        name: "",
        pattern: ""
      }
    };
  },
  methods: {
    updateCategory(category) {
      axios.put(`https://localhost:44395/api/PocketPlanner/categories/${category.id}`, category)
        .then(response => {
          console.log(response);
          if (response.data) {
            this.categories = response.data;
            console.log("Category updated:", category);
        }
        this.loadCategories();
        })
        .catch(error => {
          console.error(error);
        });
    },
    loadCategories() {
      axios.get("https://localhost:44395/api/PocketPlanner/categories")
        .then(response => {
          if (response.data) {
            this.categories = response.data;
          }
        })
        .catch(error => {
          console.error(error);
        });
    },
    addCategory() {
      axios.post("https://localhost:44395/api/PocketPlanner/categories", this.newCategory)
        .then(response => {
          if (response.data) {
            this.categories.push(response.data);
            this.newCategory = {
              id: "",
              name: "",
              pattern: ""
            };
          }
          this.loadCategories();
        })
        .catch(error => {
          console.error(error);
        });
    },
    deleteCategory(id) {
      this.categories.splice(id, 1);
      axios.delete(`https://localhost:44395/api/PocketPlanner/categories/${id}`)
        .then(response => {
          if (response.data) {
            this.categories = response.data;
          }
          this.loadCategories();
        })
        .catch(error => {
          console.error(error);
        });
    }
  },
  created() {
    this.loadCategories();
  }
};
</script>
  