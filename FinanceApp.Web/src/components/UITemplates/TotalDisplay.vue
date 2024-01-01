<template>
  <div align="right" :class="[fontClass, 'mr-12 mt-2']">
    <strong>
      {{ BudgetService.formatCurrency(totalSpent) }}
    </strong>
    <span class="text-grey">
      of {{ BudgetService.formatCurrency(totalAllocated) }}
      {{ allocatedText }}
    </span>
    <br />
    <span :style="`color: ${color}`">
      <strong>{{ BudgetService.formatCurrency(totalLeft) }}</strong>
    </span>
    Left {{ totalLeftText }}
  </div>
</template>

<script setup lang="ts">
import { SubCategoryViewModel } from "@/viewmodels.g";
import BudgetService from "@/services/BudgetService";

const props = withDefaults(
  defineProps<{
    subCategories: SubCategoryViewModel[];
    fontClass?: string;
    totalLeftText?: string;
    allocatedText?: string;
    allocation?: number | null;
    addAllocationsOnly?: boolean;
  }>(),
  {
    fontClass: "text-caption",
    totalLeftText: "this month",
    allocatedText: "spent",
    allocation: null,
    addAllocationsOnly: false,
  }
);

const totalSpent = computed(() => {
  let total = 0;
  for (const subCategory of props.subCategories) {
    if (subCategory.isStatic || props.addAllocationsOnly) {
      total += subCategory.allocation ?? 0;
    } else {
      if (subCategory.transactions!.length > 0) {
        total +=
          subCategory
            .transactions!.map((x) => x.amount)
            .reduce((accumulator, currentValue) => {
              return (accumulator ?? 0) + (currentValue ?? 0);
            }) ?? 0;
      }
    }
  }

  return total;
});

const totalAllocated = computed(() => {
  if (props.allocation !== null) return props.allocation;

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
