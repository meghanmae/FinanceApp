<template>
    <v-container>
        <c-loader-status :loaders="{ 'no-initial-content': [budget.$load] }">
            <BudgetRow :budget="budget" @deleted="routeHome" />

            <c-loader-status :loaders="{ '': [newCategory.$save] }" />

            <v-divider class="my-2" />
            <h3>
                Categories
            </h3>
            <CategoryRow v-for="category in categories.$items" :key="category.categoryId!" :category="category" />

            <v-btn @click="addCategory" color="primary" class="mt-2">
                Add Category
                <UpdateCategoryDialog v-model="showNewCategoryDialog" :category="newCategory" />
            </v-btn>
        </c-loader-status>
    </v-container>
</template>

<script setup lang="ts">
import BudgetService from '@/services/BudgetService';
import { BudgetViewModel, CategoryListViewModel, CategoryViewModel } from '@/viewmodels.g';
import { BUDGET_SERVICE } from "@/lib/symbols";
import { useRouter } from 'vue-router';

const props = defineProps<{
    budgetId: number;
}>();

const router = useRouter();

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

function routeHome() {
    router.push('/');
}
</script>