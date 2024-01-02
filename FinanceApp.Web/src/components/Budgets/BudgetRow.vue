<template>
  <c-loader-status :loaders="{ '': [budget.$delete] }" />
  <v-card>
    <v-sheet height="10px" :color="budget.color!" />

    <v-row align="center" no-gutters="">
      <v-col cols="7" md="6">
        <v-card-title>
          <Link
            :to="`/budget/${budget.budgetId}`"
            :text="budget.name!"
            :color="budget.color!"
          />
        </v-card-title>
        <v-card-subtitle>
          {{ budget.description ?? "No Description" }}
        </v-card-subtitle>
      </v-col>

      <v-col
        :align="display.mdAndUp.value ? 'right' : ''"
        cols="12"
        md=""
        order="3"
        order-md="2"
      >
        <v-card-title>
          {{ Helpers.formatCurrency(budget.allocation ?? 0) }} Allocated
        </v-card-title>
      </v-col>

      <v-col align="right" cols="5" md="auto" order="2" order-md="3">
        <v-card-title>
          <v-btn
            :color="budget.color!"
            icon="fa-solid fa-pencil"
            size="small"
            variant="tonal"
            class="mr-3"
            @click="editBudgetDialog = true"
          />
          <UpdateBudgetDialog v-model="editBudgetDialog" :budget="budget" />

          <v-btn
            color="error"
            icon="fa-solid fa-trash"
            variant="tonal"
            size="small"
            @click="deleteBudget()"
          />
        </v-card-title>
      </v-col>
    </v-row>

    <v-card-text class="text-white">
      {{ budget.description }}
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import Helpers from "@/services/Helpers";
import { BudgetViewModel } from "@/viewmodels.g";
import { useDisplay } from "vuetify";

const props = defineProps<{
  budget: BudgetViewModel;
}>();

const display = useDisplay();
const editBudgetDialog = ref(false);

const emit = defineEmits<{
  (e: "deleted"): void;
}>();

function deleteBudget() {
  if (confirm()) {
    props.budget.$delete();
    emit("deleted");
  }
}
</script>
