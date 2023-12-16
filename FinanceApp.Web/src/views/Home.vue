<template>
  <v-container>
    <h3>
      My Budgets
    </h3>

    <c-loader-status :loaders="{ 'no-initial-content': [budgets.$load] }">
      <BudgetRow v-if="budgets.$items.length > 0" v-for="budget in budgets.$items" :key="budget.budgetId!"
        :budget="budget" />
      <v-card-subtitle v-else class="mt-2">
        You have no budgets, try adding one!
      </v-card-subtitle>
    </c-loader-status>

    <v-card-text>
      <v-btn color="primary" @click="newBudgetDialog = true">
        <v-icon class="mb-1 mr-1">
          fa-solid fa-plus
        </v-icon>
        New Budget
        <UpdateBudgetDialog v-model="newBudgetDialog" :budget="newBudget" @saved="loadBudgets" />
      </v-btn>
    </v-card-text>

  </v-container>
</template>

<script setup lang="ts">
import { BudgetListViewModel, BudgetViewModel } from '@/viewmodels.g';

const newBudgetDialog = ref(false);
const budgets = new BudgetListViewModel();
let newBudget = new BudgetViewModel();

loadBudgets();

function loadBudgets() {
  newBudget = new BudgetViewModel();
  budgets.$load();
}
</script>
