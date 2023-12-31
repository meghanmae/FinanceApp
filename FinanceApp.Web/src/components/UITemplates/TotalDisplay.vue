<template>
  <div align="right" class="mr-12 text-caption mt-2">
    <strong>Total Spent:</strong>
    {{ BudgetService.formatCurrency(totalSpent) }}
    <br />
    <span :style="`color: ${color}`">
      <strong>{{ BudgetService.formatCurrency(totalLeft) }}</strong>
    </span>
    Left this month
  </div>
</template>

<script setup lang="ts">
import { SubCategoryViewModel } from "@/viewmodels.g";
import BudgetService from "@/services/BudgetService";

const props = defineProps<{
  subCategories: SubCategoryViewModel[];
}>();

const totalSpent = computed(() => {
  let total = 0;
  for (const subCategory of props.subCategories) {
    if (!subCategory.isStatic) {
      if (subCategory.transactions!.length > 0) {
        total +=
          subCategory
            .transactions!.map((x) => x.amount)
            .reduce((accumulator, currentValue) => {
              return (accumulator ?? 0) + (currentValue ?? 0);
            }) ?? 0;
      }
    } else {
      total += subCategory.allocation ?? 0;
    }
  }

  return total;
});

const totalAllocated = computed(() => {
  return props.subCategories.length > 0
    ? props.subCategories
        .map((x) => x.allocation)
        .reduce((accumulator, currentValue) => {
          return (accumulator ?? 0) + (currentValue ?? 0);
        }) ?? 0
    : 0;
});

const totalLeft = computed(() => {
  return totalAllocated.value - totalSpent.value;
});

const color = computed(() => {
  if (totalLeft.value <= 0) {
    return "#F44336";
  } else if (totalLeft.value <= totalLeft.value * 0.2) {
    return "#FFEB3B";
  } else {
    return "#4CAF50";
  }
});
</script>
