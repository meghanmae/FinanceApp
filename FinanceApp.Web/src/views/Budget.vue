<template>
  <v-container>
    <c-loader-status
      :loaders="{ 'no-initial-content': [budget.$load, categories.$load] }"
    >
      <v-card>
        <v-sheet height="10px" :color="budget.color!" />
        <v-row align="center">
          <v-col>
            <v-card-text>
              <c-input
                :model="budget"
                for="name"
                label=""
                variant="plain"
                hide-details
                class="input-heading"
              />
              <c-input
                :model="budget"
                for="description"
                label=""
                variant="plain"
                hide-details
                class="input-sub-heading"
              />
            </v-card-text>
          </v-col>
          <v-col cols="auto">
            <v-menu :close-on-content-click="false">
              <template v-slot:activator="{ props }">
                <TotalDisplay
                  v-bind="props"
                  :subCategories="budgetService.allSubCategories.value"
                  fontClass="text-subtitle-1"
                  :allocation="budget.allocation"
                  totalLeftText="to allocate"
                  allocatedText="allocated"
                  :addAllocationsOnly="true"
                />
              </template>
              <v-card>
                <v-card-text>
                  Budget Amount
                  <MoneyInput
                    v-model="budget.allocation"
                    textClass="text-subcategory-1"
                  />
                </v-card-text>
              </v-card>
            </v-menu>
          </v-col>
        </v-row>
      </v-card>

      <DateNavigator class="mt-4" />

      <v-row class="mt-1">
        <v-col xs="12" sm="6" md="4">
          <v-card class="sticky-banner">
            <CategoriesPieChart :categories="categories.$items" />
          </v-card>
        </v-col>

        <v-col cols="12" md="8">
          <c-loader-status :loaders="{ '': [newCategory.$save] }" />
          <h3 class="mb-2">
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
        </v-col>
      </v-row>
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
import { Category } from "@/models.g";

const props = defineProps<{
  budgetId: number;
}>();

const { budgetId } = toRefs(props);

const showNewCategoryDialog = ref(false);
let newCategory: CategoryViewModel;

const budget = new BudgetViewModel();
budget.$load(props.budgetId);
budget.$useAutoSave();

const budgetService = new BudgetService(budget);
provide(BUDGET_SERVICE, budgetService);

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
</script>
