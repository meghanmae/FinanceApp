<template>
  <apexchart :options="chartOptions" :series="series" />
</template>

<script setup lang="ts">
import { BUDGET_SERVICE } from "@/lib/symbols";
import { TransactionsServiceViewModel } from "@/viewmodels.g";
import colors from "vuetify/lib/util/colors";
import Helpers from "@/services/Helpers";

const budgetService = inject(BUDGET_SERVICE);

const transactionService = new TransactionsServiceViewModel();
transactionService.historicalTransactions(
  budgetService!.budget.value.budgetId,
  3
);

const series: any = ref([]);

watch(
  () => transactionService.historicalTransactions.result,
  () => {
    if (!transactionService.historicalTransactions.result) {
      return [];
    }

    for (const historicalTransaction of transactionService
      .historicalTransactions.result) {
      series.value.push({
        name: historicalTransaction.subCategoryName,
        type: "line",
        data: historicalTransaction.amount?.map((x) => x),
        color:
          colors[Helpers.toCamelCase(historicalTransaction.categoryColor!)]
            .base,
      });
    }
  }
);

const chartOptions = ref({
  chart: {
    height: 350,
    type: "line",
    stacked: false,
  },
  stroke: {
    width: [2, 2, 2],
    curve: "smooth",
  },
});
</script>
