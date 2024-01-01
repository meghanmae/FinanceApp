<template>
  <v-row
    dense
    align="center"
    @mouseover="showActionButton = true"
    @mouseleave="showActionButton = false"
  >
    <v-col>
      <c-input
        :id="id"
        :model="transaction"
        for="description"
        label=""
        @keyup.enter="saveItem"
        variant="underlined"
        hide-details
        class="input-sub-heading"
      />
    </v-col>
    <v-col cols="2" align="right">
      <MoneyInput
        v-model="transaction.amount"
        variant="underlined"
        @keyup.enter="saveItem"
      />
    </v-col>
    <v-col cols="auto" align="right">
      <v-btn
        v-if="transaction.transactionId"
        :class="[
          showActionButton && transaction.transactionId ? '' : 'hidden-element',
          'ml-1',
        ]"
        color="error"
        icon="fa-solid fa-trash"
        variant="tonal"
        size="small"
        @click="deleteTransaction()"
      />
      <v-btn
        v-else
        color="primary"
        icon="fa-solid fa-plus"
        variant="tonal"
        size="small"
        @click="saveItem()"
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { BUDGET_SERVICE } from "@/lib/symbols";
import { TransactionViewModel } from "@/viewmodels.g";

const props = defineProps<{
  transaction: TransactionViewModel;
  subCategoryId: number;
  id?: string | null;
}>();

const emit = defineEmits<{
  (e: "addedNew"): void;
}>();

const showActionButton = ref(false);
const budgetService = inject(BUDGET_SERVICE);

if (props.transaction.transactionId) {
  props.transaction.$useAutoSave();
}

function focus() {
  if (props.id) {
    const element = document.getElementById(props.id);
    element?.focus();
  }
}

function deleteTransaction() {
  if (confirm()) {
    props.transaction.$delete();
  }
}

function saveItem() {
  if (!props.transaction.transactionId) {
    props.transaction.subCategoryId = props.subCategoryId;
    props.transaction.budgetId = budgetService!.budget.value.budgetId!;
    props.transaction.$save();

    emit("addedNew");

    nextTick(() => focus());
  }
}
</script>
