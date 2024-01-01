<template>
  <div class="d-flex align-center">
    <v-btn
      size="small"
      density="comfortable"
      color="primary"
      variant="tonal"
      icon="fa-solid fa-chevron-left"
      @click="previous"
    />
    <VueDatePicker
      v-model="budgetService!.startDate"
      :dark="darkMode"
      auto-apply
      :clearable="false"
      :enable-time-picker="false"
      :week-start="0"
      :style="{ minWidth: '13rem' }"
    >
      <template #trigger>
        <div class="text-center">
          {{ datePickerText }}
        </div>
      </template>
    </VueDatePicker>
    <v-btn
      size="small"
      density="comfortable"
      color="primary"
      variant="tonal"
      icon="fa-solid fa-chevron-right"
      class="mr-2"
      @click="next"
    />
    <v-chip
      @click="current"
      :variant="isCurrentCalc ? 'tonal' : 'text'"
      color="primary"
    >
      <v-icon class="mr-2 mb-1">
        {{ isCurrentCalc ? "fas fa-calendar-check" : "fas fa-calendar" }}
      </v-icon>
      Current Month
    </v-chip>
  </div>
</template>

<script lang="ts" setup>
import { useTheme } from "vuetify/lib/framework.mjs";
import {
  isSameMonth,
  startOfToday,
  startOfMonth,
  endOfMonth,
  format,
  addMonths,
} from "date-fns";
import VueDatePicker from "@vuepic/vue-datepicker";
import { BUDGET_SERVICE } from "@/lib/symbols";

const formatString = "MM/dd/yyyy";

const budgetService = inject(BUDGET_SERVICE);

const darkMode = computed(() => useTheme().global.name.value === "dark");

const datePickerText = computed(() => {
  return `${format(
    startOfMonth(budgetService!.startDate.value),
    formatString
  )} - ${format(budgetService!.endDate.value, formatString)}`;
});

const isCurrentCalc = computed(() => {
  return isSameMonth(budgetService!.endDate.value, new Date());
});

function next() {
  budgetService!.startDate.value = startOfMonth(
    addMonths(budgetService!.startDate.value, 1)
  );
}

function previous() {
  budgetService!.startDate.value = startOfMonth(
    addMonths(budgetService!.startDate.value, -1)
  );
}

function current() {
  budgetService!.startDate.value = startOfMonth(startOfToday());
}

watch(budgetService!.startDate, () => {
  budgetService!.endDate.value = endOfMonth(budgetService!.startDate.value);
});
</script>
