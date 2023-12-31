<template>
  <v-container>
    <c-loader-status
      :loaders="{ 'no-initial-content': [budget.$load, categories.$load] }"
    >
      <BudgetRow :budget="budget" @deleted="routeHome" />

      <c-loader-status :loaders="{ '': [newCategory.$save] }" />

      <v-divider class="my-2" />
      <h3>
        Categories
        <v-btn
          @click="addCategory"
          icon="fa-solid fa-plus"
          size="x-small"
          color="primary"
          class="ml-1"
        />
        <UpdateCategoryDialog
          v-model="showNewCategoryDialog"
          :category="newCategory"
          @saved="loadCategories"
        />
      </h3>
      <v-expansion-panels multiple>
        <v-expansion-panel
          v-for="category in categories.$items"
          :key="category.categoryId!"
        >
          <CategoryRow :category="category" />
        </v-expansion-panel>
      </v-expansion-panels>
    </c-loader-status>
  </v-container>
</template>

<script setup lang="ts">
import BudgetService from "@/services/BudgetService";
import {
  BudgetViewModel,
  CategoryListViewModel,
  CategoryViewModel,
} from "@/viewmodels.g";
import { BUDGET_SERVICE } from "@/lib/symbols";
import { useRouter } from "vue-router";
import { Category } from "@/models.g";

const props = defineProps<{
  budgetId: number;
}>();

const { budgetId } = toRefs(props);
const router = useRouter();

const showNewCategoryDialog = ref(false);
let newCategory: CategoryViewModel;

const budget = new BudgetViewModel();
budget.$load(props.budgetId);

provide(BUDGET_SERVICE, new BudgetService(budget));

const categories = new CategoryListViewModel();
const datasource = new Category.DataSources.CategoriesByBudget();
datasource.budgetId = budgetId.value;
categories.$dataSource = datasource;
function loadCategories() {
  categories.$load();
  newCategory = new CategoryViewModel();
}
loadCategories();

function addCategory() {
  showNewCategoryDialog.value = true;
}

function routeHome() {
  router.push("/");
}
</script>
