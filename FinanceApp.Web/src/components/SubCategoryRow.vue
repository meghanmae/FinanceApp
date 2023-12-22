<template>
    <c-loader-status :loaders="{ '': [subCategory.$delete] }" />
    <v-sheet>
        <v-row align="center">
            <v-col>
                <v-card-title>
                    {{ subCategory.name }} | {{ subCategory.allocation }}

                    <v-btn @click="showNewTransactionDialog = true" icon="fa-solid fa-plus" size="x-small" :color="color"
                        class="ml-1" />
                    <UpdateTransactionDialog v-model="showNewTransactionDialog" :transaction="newTransaction"
                        :subCategoryId="subCategory.subCategoryId!" @saved="loadTransactions" />
                </v-card-title>
                <v-card-subtitle class="text-white">
                    {{ subCategory.description }}
                </v-card-subtitle>
            </v-col>
            <v-col align="right">
                <v-card-title>
                    <v-btn icon="fa-solid fa-pencil" size="small" variant="tonal" class="mr-3"
                        @click="editSubCategoryDialog = true" />
                    <UpdateSubCategoryDialog v-model="editSubCategoryDialog" :subCategory="subCategory"
                        :categoryId="subCategory.categoryId!" :color="color" />

                    <v-btn color="error" icon="fa-solid fa-trash" variant="tonal" size="small"
                        @click="deleteSubCategory()" />
                </v-card-title>
            </v-col>
        </v-row>

        <v-card-text>
            <TransactionRow v-for="transaction in transactions.$items" :key="transaction.transactionId!"
                :transaction="transaction" />
        </v-card-text>
    </v-sheet>
</template>

<script setup lang="ts">
import { Transaction } from '@/models.g';
import { SubCategoryViewModel, TransactionListViewModel, TransactionViewModel } from '@/viewmodels.g';

const proxy = getCurrentInstance()?.proxy;

const props = defineProps<{
    subCategory: SubCategoryViewModel;
    color: string;
}>();

const editSubCategoryDialog = ref(false);

const showNewTransactionDialog = ref(false);
let newTransaction: TransactionViewModel;

const transactions = new TransactionListViewModel();
const datasource = new Transaction.DataSources.TransactionsByBudget();
datasource.subCategoryId = props.subCategory.subCategoryId;
transactions.$dataSource = datasource;

function loadTransactions() {
    transactions.$load();
    newTransaction = new TransactionViewModel();
}
loadTransactions();

function deleteSubCategory() {
    if (confirm()) {
        props.subCategory.$delete();
    }
}
</script>