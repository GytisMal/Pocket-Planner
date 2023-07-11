<template>
  <div>
    <UpdateCategoriesDialog ref="updateDialog" @updateCategory="updateCategory" />
    <h2>Categories</h2>
    <div class="table-container">
      <v-table class="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Pattern</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="category in categories" :key="category && category.id">
            <td>{{ category.id }}</td>
            <td>
              {{ category.name }}
            </td>
            <td>
              {{ category.pattern }}
            </td>
            <td>
              <v-icon @click="openUpdateDialog(category)">mdi-pencil</v-icon>
              <v-icon @click="deleteCategory(category.id)">mdi-delete</v-icon>
            </td>
          </tr>
          <tr>
            <td></td>
            <td>
              <input v-model="newCategory.name" placeholder="Name*" required fill />
            </td>
            <td>
              <input v-model="newCategory.pattern" placeholder="Pattern*" required fill />
            </td>
            <td>
              <v-icon class="plus-icon" @click="addCategory">mdi-plus</v-icon>
            </td>
          </tr>
        </tbody>
      </v-table>
    </div>
  </div>
</template>

<style scoped>
.plus-icon {
  color: #000000;
  font-size: 24px;
}
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
import UpdateCategoriesDialog from "./UpdateCategoriesDialog.vue";

export default {
  name: "Categories",
  components: {
    UpdateCategoriesDialog,
  },
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
    openUpdateDialog(category) {
      this.$refs.updateDialog.open(category)
    },
    updateCategory(updatedCategory) {
      const index = this.categories.findIndex(category => category.id == updatedCategory.id);
      if (index != -1) {
        this.categories[index] = updatedCategory;
      }
    },
    loadCategories() {
      this.$axios.get("Categories")
        .then(response => {
          if (response.data) {
            this.categories = response.data.data;
          }
        })
        .catch(error => {
          console.error(error);
        });
    },
    addCategory() {
      if(!this.newCategory.name || !this.newCategory.pattern) {     
        return
      }
      this.$axios.post("Categories", this.newCategory).then(response => {
        if (response.data) {
          this.categories.push(response.data.data);
          this.newCategory = {
            id: "",
            name: "",
            pattern: ""
          };
        }
      })
      .catch(error => {
        console.error(error);
      });
  },  
    deleteCategory(id) {
    this.$axios.delete(`Categories/${id}`)
      .then(response => {
        if (response.data) {
          this.categories = response.data;
        };
        this.loadCategories();
      })
      .catch(error => {
        console.error(error);
      });
    },
  },
  created() {
    this.loadCategories();
  }
};
</script>
