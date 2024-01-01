<template>
  <v-select v-model="modelValue" :items="validIconsList" label="Icon">
    <template #selection="{ item }">
      <v-icon :color="color">
        {{ item.title }}
      </v-icon>
    </template>
    <template #item="{ props, item }">
      <v-list-item v-bind="props" density="compact" title="">
        <v-icon :color="color">
          {{ item.title }}
        </v-icon>
      </v-list-item>
    </template>
  </v-select>
</template>

<script setup lang="ts">
defineProps<{
  color: string;
}>();

const modelValue = defineModel<string | null>({ default: null });

const validIconsList = [
  "fa-solid fa-dollar-sign",
  "fa-solid fa-house",
  "fa-solid fa-hospital",
  "fa-solid fa-utensils",
  "fa-solid fa-umbrella-beach",
  "fa-solid fa-umbrella",
  "fa-solid fa-suitcase-rolling",
  "fa-solid fa-envelope-open-text",
  "fa-solid fa-screwdriver-wrench",
  "fa-solid fa-wallet",
  "fa-solid fa-baby-carriage",
  "fa-solid fa-champagne-glasses",
  "fa-regular fa-gem",
  "fa-solid fa-credit-card",
  "fa-solid fa-piggy-bank",
  "fa-solid fa-building-columns",
  "fa-solid fa-hand-holding-heart",
  "fa-solid fa-gifts",
  "fa-solid fa-guitar",
  "fa-solid fa-basket-shopping",
  "fa-solid fa-graduation-cap",
  "fa-solid fa-gas-pump",
  "fa-solid fa-car-rear",
  "fa-solid fa-caravan",
  "fa-solid fa-mobile-screen",
  "fa-solid fa-headphones-simple",
];
</script>
