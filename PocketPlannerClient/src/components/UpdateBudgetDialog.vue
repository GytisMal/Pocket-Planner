<template>
    <v-dialog v-model="dialog" persistent width="600">
        <v-card>
            <v-card-title>
                <span class="text-h5"> Update Budget </span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-row>
                        <v-col cols="12">
                            <v-text-field v-model="budget.type" label="Type*" required></v-text-field>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field v-model="budget.amount" label="Amount*" required></v-text-field>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field v-model="budget.date" label="Date*" required></v-text-field>
                        </v-col>
                    </v-row>
                </v-container>
                <small>* Indicates Required Field</small>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" text @click="dialog = false"> Close </v-btn>
                <v-btn color="blue-darken-1" text @click="updateBudget"> Save Changes</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import axios from "axios";

export default {
    name: 'UpdateBudgetDialog',
    data() {
      return {
        dialog: false,
        budget: null,
      };
    },
    methods: {
        open(budget) {
            this.budget = { ...budget };
            this.dialog = true;
        },
        updateBudget() {
            this.$axios.put(`Budget/${this.budget.id}`, this.budget).then(response => {
                if(response.data) {
                    this.$emit('updateBudget', response.data.data);
                }
                this.dialog = false;
            })
            .catch(error => {
                console.error(error)
            });
        },
    }
};
</script>