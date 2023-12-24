<template>
  <v-select v-model="modelValue" :items="validColorsList" label="Color">
    <template #selection="{ item }">
      <v-chip :color="item.value" size="small" variant="flat">
        {{ item.value }}
      </v-chip>
    </template>
    <template #item="{ props, item }">
      <v-list-item v-bind="props" density="compact" title="">
        <v-chip v-bind="props" :color="item.value" variant="flat">
          {{ item.value }}
        </v-chip>
      </v-list-item>
    </template>
  </v-select>
</template>

<script setup lang="ts">
import colors from "vuetify/lib/util/colors";

const modelValue = defineModel<string | null>({ default: null });

function camelToKebabCase(value: string) {
  return value.replace(/([a-z])([A-Z])/g, "$1-$2").toLowerCase();
}

const validColorsList: string[] = Object.keys(colors).map((x) =>
  camelToKebabCase(x)
);
</script>
