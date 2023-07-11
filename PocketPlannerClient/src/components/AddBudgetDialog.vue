<template>
    <v-row justify="center">
      <v-dialog
        v-model="dialog"
        persistent
        width="1024"
      >
        <template v-slot:activator="{ props }">
          <v-btn
            color="primary"
            v-bind="props"
          >
            Add Budget
          </v-btn>
        </template>
        <v-card>
          <v-card-title>
            <span class="text-h5">Budget</span>
          </v-card-title>
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12" sm="6" md="4">
                  <v-text-field v-model="newBudget.type" label="Type*" required  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                  <v-text-field v-model="newBudget.amount" label="Amount*" required  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                  <v-text-field v-model="newBudget.date" label="Date*" required  ></v-text-field>
                </v-col>              
              </v-row>
            </v-container>
            <small>*indicates required field</small>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue-darken-1" variant="text" @click="dialog = false">Close</v-btn>
            <v-btn color="blue-darken-1" variant="text" @click="addBudget">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
  </template>

<script>
export default {
    name: 'AddBudgetDialog',
    data() {
      return {
        dialog: false,
        newBudget: {
          type: "",
          amount: null,
          date: "",
        }
      };
    },
    methods: {
      addBudget() {
        this.$axios.post("Budget", this.newBudget).then(response => {
          if(response.data) {
            this.$emit("addNewBudget", response.data.data);
            this.newBudget = {
              type: "",
              amount: null,
              date: ""
            }
          }
          this.dialog = false;
        })
        .catch(error => {
          console.error(error);
        })
      },
    }
 };
</script>