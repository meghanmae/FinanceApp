<template>
  <v-row
    dense
    align="center"
    @mouseover="showDelete = true"
    @mouseleave="showDelete = false"
  >
    <v-col>
      <c-input
        :model="transaction"
        for="description"
        label=""
        variant="underlined"
        hide-details
        class="input-sub-heading"
      />
    </v-col>
    <v-col cols="2" align="right">
      <MoneyInput v-model="transaction.amount" variant="underlined" />
    </v-col>
    <v-col cols="auto" align="right">
      <v-btn
        :class="[showDelete ? '' : 'hidden-element', 'ml-1']"
        color="error"
        icon="fa-solid fa-trash"
        variant="tonal"
        size="small"
        @click="deleteTransaction()"
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { TransactionViewModel } from "@/viewmodels.g";

const props = defineProps<{
  transaction: TransactionViewModel;
}>();

const showDelete = ref(false);

function deleteTransaction() {
  if (confirm()) {
    props.transaction.$delete();
  }
}
</script>
