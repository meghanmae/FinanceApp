<template>
    <c-loader-status :loaders="{ 'no-initial-content': [budget.$load] }">
        Budget {{ budget.name }}

        <br />

        <c-loader-status :loaders="{ '': [newCategory.$save] }" />
        Categories:
        <c-input :model="newCategory" for="name" variant="plain" class="text-white" single-line hide-details />
        <c-input :model="newCategory" for="color" variant="plain" class="text-white" single-line hide-details />
        <c-input :model="newCategory" for="icon" variant="plain" class="text-white" single-line hide-details />
        <c-input :model="newCategory" for="description" variant="plain" class="text-white" single-line hide-details />
        <v-btn @click="addCategory">
            Add Category
        </v-btn>

        <v-card v-for="category in categories.$items" :key="category.categoryId!">
            <v-card-title>
                {{ category.name }}
            </v-card-title>
            <v-card-subtitle>
                {{ category.description }}
            </v-card-subtitle>
        </v-card>
    </c-loader-status>
</template>

<script setup lang="ts">
import { BudgetViewModel, CategoryListViewModel, CategoryViewModel } from '@/viewmodels.g';

const props = defineProps<{
    budgetId: number;
}>();

const budget = new BudgetViewModel();
budget.$load(props.budgetId);


let newCategory = new CategoryViewModel();
const categories = new CategoryListViewModel();
categories.$load();

function addCategory() {
    console.log(newCategory);
    newCategory.$save();

    categories.$load();
}
</script>