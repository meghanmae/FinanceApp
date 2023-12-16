<template>
    <v-dialog max-width="800" v-model="modelValue">
        <v-card>
            <v-card-item class="bg-primary pb-2">
                <v-card-title>
                    {{ newBudget ? 'Create a New Budget' : 'Edit Budget' }}
                </v-card-title>
            </v-card-item>
            <c-loader-status :loaders="{ '': [budget.$save] }" />
            <v-card-text>
                <c-input :model="budget" for="name" autofocus @keyup.enter="save" />
                <c-input :model="budget" for="description" @keyup.enter="save" />
                <c-input :model="budget" for="color" @keyup.enter="save" />
            </v-card-text>
            <v-card-actions>
                <v-spacer />
                <v-btn color="primary" variant="text" @click="modelValue = false">
                    Close
                </v-btn>
                <v-btn color="primary" variant="flat" @click="save">
                    Save
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { BudgetViewModel } from '@/viewmodels.g';

const props = defineProps<{
    budget: BudgetViewModel;
}>();

const modelValue = defineModel<boolean>({ default: false });

const newBudget = computed(() => !props.budget.budgetId);

const emit = defineEmits<{
    (e: "saved"): void
}>();

async function save() {
    if (props.budget.name) {
        modelValue.value = false;
        await props.budget.$save();
        emit("saved");
    }
}
</script>