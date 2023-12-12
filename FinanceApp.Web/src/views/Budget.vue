<template>
    <c-loader-status :loaders="{ 'no-initial-content': [budget.$load] }">
        Budget {{ budget.name }}

        <br />

        <c-loader-status :loaders="{ '': [newCategory.$save] }" />
        Categories:
        <c-input :model="newCategory" for="name" variant="plain" class="text-white" single-line hide-details />
        <v-btn @click="addCategory">
            Add Category
            <UpdateCategoryDialog v-model="showNewCategoryDialog" :category="newCategory" />
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
import BudgetService from '@/services/BudgetService';
import { BudgetViewModel, CategoryListViewModel, CategoryViewModel } from '@/viewmodels.g';
import { BUDGET_SERVICE } from "@/lib/symbols";

const props = defineProps<{
    budgetId: number;
}>();

const showNewCategoryDialog = ref(false);
const newCategory = ref(new CategoryViewModel());

const budget = new BudgetViewModel();
budget.$load(props.budgetId);

provide(BUDGET_SERVICE, new BudgetService(budget))

const categories = new CategoryListViewModel();
categories.$load();

function addCategory() {
    showNewCategoryDialog.value = true;
}
</script>