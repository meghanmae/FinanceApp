<template>
  <c-loader-status :loaders="{ '': [subCategory.$delete] }" />
  <v-row
    dense
    align="center"
    @mouseover="showDelete = true"
    @mouseleave="showDelete = false"
  >
    <v-col>
      <c-input
        :model="subCategory"
        for="name"
        label=""
        variant="plain"
        hide-details
        class="input-heading my-n3"
      />
      <c-input
        :model="subCategory"
        for="description"
        label=""
        placeholder="no description"
        variant="plain"
        hide-details
        class="input-sub-heading"
      />
    </v-col>
    <v-col cols="2" align="right">
      <MoneyInput v-model="subCategory.allocation" class="mb-n3" />
    </v-col>
    <v-col align="right" cols="auto" class="mb-n11 ml-5">
      <c-input :model="subCategory" for="isStatic" />
    </v-col>
    <v-col cols="auto" align="right">
      <v-btn
        :class="[showDelete ? '' : 'hidden-element', 'mt-5 ml-1']"
        color="error"
        icon="fa-solid fa-trash"
        variant="tonal"
        size="small"
        @click="deleteSubCategory()"
      />
    </v-col>
  </v-row>

  <v-card-text v-if="!subCategory.isStatic">
    <TransactionRow
      v-for="transaction in transactions.$items"
      :key="transaction.transactionId!"
      :transaction="transaction"
      :subCategoryId="subCategory.subCategoryId!"
    />
    <TransactionRow
      :id="`new-transaction-${subCategory.subCategoryId}`"
      :transaction="newTransaction"
      :subCategoryId="subCategory.subCategoryId!"
      @addedNew="loadTransactions"
    />

    <div align="right" class="mr-12 text-subtitle-1">
      <strong>Total Spent:</strong>
      {{ BudgetService.formatCurrency(totalSpent) }}
      <br />
      <span :style="`color: ${color}`">
        <strong>{{ BudgetService.formatCurrency(totalLeft) }}</strong>
      </span>
      Left this month
    </div>
  </v-card-text>
</template>

<script setup lang="ts">
import { Transaction } from "@/models.g";
import {
  SubCategoryViewModel,
  TransactionListViewModel,
  TransactionViewModel,
} from "@/viewmodels.g";
import BudgetService from "@/services/BudgetService";

const props = defineProps<{
  subCategory: SubCategoryViewModel;
  color: string;
}>();

const { subCategory } = toRefs(props);

const showDelete = ref(false);

props.subCategory.$useAutoSave();

let newTransaction: TransactionViewModel;

const transactions = new TransactionListViewModel();
const datasource = new Transaction.DataSources.TransactionsByBudget();
datasource.subCategoryId = subCategory.value.subCategoryId;
transactions.$dataSource = datasource;

const totalSpent = computed(() => {
  let total = 0;
  if (transactions.$items.length > 0) {
    total =
      transactions.$items
        .map((x) => x.amount)
        .reduce((accumulator, currentValue) => {
          return (accumulator ?? 0) + (currentValue ?? 0);
        }) ?? 0;
  }

  return total;
});

const totalLeft = computed(() => {
  return (subCategory.value.allocation ?? 0) - totalSpent.value;
});

const color = computed(() => {
  if (totalLeft.value <= 0) {
    return "#F44336";
  } else if (totalLeft.value <= (subCategory.value.allocation ?? 0) * 0.2) {
    return "#FFEB3B";
  } else {
    return "#4CAF50";
  }
});

function loadTransactions() {
  transactions.$load();
  newTransaction = new TransactionViewModel();
}
loadTransactions();

function deleteSubCategory() {
  if (confirm()) {
    props.subCategory.$delete();
  }
}
</script>
