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

  <v-card-text>
    <TransactionRow
      v-for="transaction in transactions.$items"
      :key="transaction.transactionId!"
      :transaction="transaction"
    />
  </v-card-text>
</template>

<script setup lang="ts">
import { Transaction } from "@/models.g";
import {
  SubCategoryViewModel,
  TransactionListViewModel,
  TransactionViewModel,
} from "@/viewmodels.g";

const props = defineProps<{
  subCategory: SubCategoryViewModel;
  color: string;
}>();

const showDelete = ref(false);

const showNewTransactionDialog = ref(false);
let newTransaction: TransactionViewModel;

const transactions = new TransactionListViewModel();
const datasource = new Transaction.DataSources.TransactionsByBudget();
datasource.subCategoryId = props.subCategory.subCategoryId;
transactions.$dataSource = datasource;

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
