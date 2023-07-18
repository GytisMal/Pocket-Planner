<template>
    <v-dialog v-model="dialog" persistent width="600">
        <v-card>
            <v-card-title>
                <span class="text-h5"> Update Category </span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-row>
                        <v-col cols="12">
                            <v-text-field v-model="category.name" label="Name*" required></v-text-field>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field v-model="category.pattern" label="Pattern*" required></v-text-field>
                        </v-col>
                    </v-row>
                </v-container>
                <small>* Indicates Required Field</small>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" text @click="dialog = false"> Close </v-btn>
                <v-btn color="blue-darken-1" text @click="updateCategory"> Save Changes </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>

export default {
    name: "UpdateCategoriesDialog",
    data() {
        return {
            dialog: false,
            category: null,
        }
    },
    methods: {
        open(category) {
            this.category = { ...category };
            this.dialog = true;
        },
        updateCategory() {
            this.$axios.put(`Categories/${this.category.id}`, this.category).then(response => {
                if(response.data) {
                    this.$emit('updateCategory', response.data.data);
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