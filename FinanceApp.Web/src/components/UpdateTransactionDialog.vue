<template>
    <v-dialog max-width="800" v-model="modelValue">
        <v-card>
            <v-card-item class="bg-primary pb-2">
                <v-card-title>
                    {{ newTransaction ? 'Create a New Transaction' : 'Edit Transaction' }}
                </v-card-title>
            </v-card-item>
            <c-loader-status :loaders="{ '': [transaction.$save] }" />
            <v-card-text>
                <c-input :model="transaction" for="amount" variant="plain" class="text-white" single-line hide-details />
                <c-input :model="transaction" for="description" variant="plain" class="text-white" single-line
                    hide-details />
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
import { BUDGET_SERVICE } from '@/lib/symbols';
import { TransactionViewModel } from '@/viewmodels.g';

const props = defineProps<{
    subCategoryId: number;
    transaction: TransactionViewModel;
}>();

const modelValue = defineModel<boolean>({ default: false });

const budgetService = inject(BUDGET_SERVICE)

const newTransaction = computed(() => !props.transaction.transactionId);

const emit = defineEmits<{
    (e: "saved"): void
}>();

async function save() {
    if (props.transaction.amount) {
        props.transaction.budgetId = budgetService?.budget.value.budgetId!;
        props.transaction.subCategoryId = props.subCategoryId;

        await props.transaction.$save();
        modelValue.value = false;
        emit("saved");
    }
}
</script>