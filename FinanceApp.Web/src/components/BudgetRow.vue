<template>
    <v-card class="ma-2" color="blue-grey" variant="tonal">
        <v-list-item>
            <template v-slot:title>
                <c-input :model="budget" for="name" variant="plain" class="text-white" single-line hide-details />
            </template>
            <template v-slot:subtitle>
                <c-input :model="budget" for="description" variant="plain" class="text-white" single-line hide-details />
            </template>

            <template v-slot:append>
                <v-btn color="error" icon="fa-solid fa-trash" variant="text" @click="budget.$delete()" />
            </template>
        </v-list-item>
    </v-card>
</template>

<script setup lang="ts">
import { BudgetViewModel } from '@/viewmodels.g';

const proxy = getCurrentInstance()?.proxy;

const props = defineProps<{
    budget: BudgetViewModel;
}>();

props.budget.$startAutoSave(proxy!);
</script>