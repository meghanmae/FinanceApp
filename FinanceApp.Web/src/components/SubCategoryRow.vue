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
      v-for="transaction in subCategory.transactions"
      :key="transaction.transactionId!"
      :transaction="transaction"
      :subCategoryId="subCategory.subCategoryId!"
    />

    <TransactionRow
      :id="`new-transaction-${subCategory.subCategoryId}`"
      :transaction="newTransaction"
      :subCategoryId="subCategory.subCategoryId!"
      @addedNew="refreshSubcategory"
    />

    <TotalDisplay :subCategories="subCategories" />
  </v-card-text>
</template>

<script setup lang="ts">
import { BUDGET_SERVICE } from "@/lib/symbols";
import { SubCategoryViewModel, TransactionViewModel } from "@/viewmodels.g";

const props = defineProps<{
  subCategory: SubCategoryViewModel;
  color: string;
}>();

const showDelete = ref(false);

const budgetService = inject(BUDGET_SERVICE);

props.subCategory.$useAutoSave();

const subCategories: SubCategoryViewModel[] = [props.subCategory];

let newTransaction: TransactionViewModel = new TransactionViewModel();

function refreshSubcategory() {
  props.subCategory.$load(props.subCategory.subCategoryId!);
  nextTick(() => (newTransaction = new TransactionViewModel()));
}

function deleteSubCategory() {
  if (confirm()) {
    props.subCategory.$delete();
    budgetService!.allSubCategories.value =
      budgetService!.allSubCategories.value.filter(
        (x) => x.subCategoryId !== props.subCategory.subCategoryId
      );
  }
}

watch(
  () => props.subCategory.allocation,
  () => {
    budgetService?.updateSubCategories([props.subCategory]);
  }
);
</script>
