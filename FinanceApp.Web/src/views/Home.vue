<template>
  <v-container>
    <v-card>
      <v-card-title>
        My Budgets
      </v-card-title>
      <v-divider />

      <c-loader-status :loaders="{ 'no-initial-content': [budgets.$load] }">
        <v-list v-if="budgets.$items.length > 0">
          <BudgetRow v-for="budget in budgets.$items" :key="budget.budgetId!" :budget="budget" />
        </v-list>
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
        </v-btn>
      </v-card-text>
    </v-card>

    <UpdateBudgetDialog v-model="newBudgetDialog" :budget="newBudget" @saved="loadBudgets" />
  </v-container>
</template>

<script setup lang="ts">
import { BudgetListViewModel, BudgetViewModel } from '@/viewmodels.g';

const budgets = new BudgetListViewModel();
loadBudgets();

const newBudgetDialog = ref(false);
const newBudget = new BudgetViewModel();

function loadBudgets() {
  budgets.$load();
}
</script>
