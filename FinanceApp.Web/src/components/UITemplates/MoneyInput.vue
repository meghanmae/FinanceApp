<template>
  <v-text-field
    v-model="formattedValue"
    :variant="variant"
    ref="inputRef"
    class="input-sub-heading right-align-input"
    hide-details
  />
</template>

<script setup lang="ts">
import { useCurrencyInput } from "vue-currency-input";
import { watch } from "vue";

const modelValue = defineModel<number | null>();
withDefaults(
  defineProps<{
    variant?: string;
  }>(),
  {
    variant: "plain",
  }
);

const emits = defineEmits(["change"]);

const { inputRef, formattedValue, setValue } = useCurrencyInput({
  currency: "USD",
  hideCurrencySymbolOnFocus: false,
  hideGroupingSeparatorOnFocus: false,
  precision: 2,
  valueRange: { min: 0 },
});

watch(formattedValue, (newValue) => {
  emits("change", newValue);
});

watch(
  () => modelValue.value,
  (value) => {
    setValue(value ?? 0);
  }
);
</script>

<style scoped>
.right-align-input >>> input {
  text-align: right;
}
</style>
