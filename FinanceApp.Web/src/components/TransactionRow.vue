<template>
    <c-loader-status :loaders="{ '': [transaction.$delete] }" />
    <v-sheet>
        <v-row align="center">
            <v-col>
                <v-card-title>
                    {{ transaction.description }} | {{ transaction.amount }}
                </v-card-title>
            </v-col>
            <v-col align="right">
                <v-card-title>
                    <v-btn icon="fa-solid fa-pencil" size="small" variant="tonal" class="mr-3"
                        @click="editTransactionDialog = true" />
                    <UpdateTransactionDialog v-model="editTransactionDialog" :transaction="transaction"
                        :subCategoryId="transaction.subCategoryId!" />

                    <v-btn color="error" icon="fa-solid fa-trash" variant="tonal" size="small"
                        @click="deleteTransaction()" />
                </v-card-title>
            </v-col>
        </v-row>

    </v-sheet>
</template>

<script setup lang="ts">
import { TransactionViewModel } from '@/viewmodels.g';

const proxy = getCurrentInstance()?.proxy;

const props = defineProps<{
    transaction: TransactionViewModel;
}>();

const editTransactionDialog = ref(false);

function deleteTransaction() {
    if (confirm()) {
        props.transaction.$delete();
    }
}
</script>