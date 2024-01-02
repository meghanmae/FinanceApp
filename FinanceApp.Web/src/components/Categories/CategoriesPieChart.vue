<template>
  <apexchart type="pie" :options="chartOptions" :series="series" />
</template>

<script setup lang="ts">
import Helpers from "@/services/Helpers";
import { CategoryViewModel, SubCategoryViewModel } from "@/viewmodels.g";
import colors from "vuetify/lib/util/colors";

const props = defineProps<{
  categories: CategoryViewModel[];
}>();

const sortedCategories = computed(() => {
  return props.categories.sort(
    (a, b) =>
      totalAllocated(b.subCategories ?? []) -
      totalAllocated(a.subCategories ?? [])
  );
});

const series = sortedCategories.value.map((x) =>
  totalAllocated(x.subCategories ?? [])
);

const labels = sortedCategories.value.map((x) => x.name);
const chartColors = sortedCategories.value.map(
  (x) => colors[Helpers.toCamelCase(x.color!)].base
);

const chartOptions = {
  chart: {
    width: 380,
  },
  labels: labels,
  colors: chartColors,
  legend: {
    show: false,
  },
  theme: {
    mode: "light",
  },
  responsive: [
    {
      breakpoint: 480,
      options: {
        chart: {
          width: 200,
        },
      },
    },
  ],
};

function totalAllocated(subCategories: SubCategoryViewModel[]) {
  return subCategories.length > 0
    ? subCategories
        .map((x) => x.allocation)
        .reduce((accumulator, currentValue) => {
          return (accumulator ?? 0) + (currentValue ?? 0);
        }) ?? 0
    : 0;
}
</script>
