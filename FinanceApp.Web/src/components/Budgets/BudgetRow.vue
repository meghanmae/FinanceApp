<template>
  <c-loader-status :loaders="{ '': [budget.$delete] }" />
  <v-card :color="budget.color!" variant="tonal">
    <v-card :color="budget.color!" variant="tonal">
      <v-row align="center">
        <v-col>
          <v-card-title>
            <Link
              :to="`/budget/${budget.budgetId}`"
              :text="budget.name!"
              :color="budget.color!"
            />
          </v-card-title>
        </v-col>
        <v-col align="right">
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
    </v-card>

    <v-sheet :color="budget.color!" height="5px" />

    <v-card-text class="text-white">
      {{ budget.description }}
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { BudgetViewModel } from "@/viewmodels.g";

const props = defineProps<{
  budget: BudgetViewModel;
}>();

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
