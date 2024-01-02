<template>
  <v-dialog max-width="800" v-model="modelValue">
    <v-card>
      <v-card-item :class="`bg-${category.color} pb-2`">
        <v-card-title>
          {{ newCategory ? "Create a New Category" : "Edit Category" }}
        </v-card-title>
      </v-card-item>
      <c-loader-status :loaders="{ '': [category.$save] }" />
      <v-card-title>
        <v-row dense>
          <v-col cols="2">
            <IconPicker
              v-model="category.icon"
              :color="category.color ?? 'primary'"
            />
          </v-col>
          <v-col>
            <c-input
              :model="category"
              for="name"
              class="text-white"
              single-line
              hide-details
            />
          </v-col>
          <v-col cols="3">
            <ColorPicker v-model="category.color" />
          </v-col>
          <v-col cols="12">
            <c-input
              :model="category"
              for="description"
              class="text-white"
              single-line
              hide-details
            />
          </v-col>
        </v-row>
      </v-card-title>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" variant="text" @click="modelValue = false">
          Close
        </v-btn>
        <v-btn color="primary" variant="flat" @click="save"> Save </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { BUDGET_SERVICE } from "@/lib/symbols";
import { CategoryViewModel } from "@/viewmodels.g";

const props = defineProps<{
  category: CategoryViewModel;
}>();
const modelValue = defineModel<boolean>({ default: false });

const budgetService = inject(BUDGET_SERVICE);

const newCategory = computed(() => !props.category.categoryId);

const emit = defineEmits<{
  (e: "saved"): void;
}>();

async function save() {
  if (props.category.name) {
    props.category.budgetId = budgetService!.budget.value.budgetId!;
    await props.category.$save();
    modelValue.value = false;
    emit("saved");
  }
}
</script>
