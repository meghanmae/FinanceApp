<template>
    <v-dialog max-width="800" v-model="modelValue">
        <v-card>
            <v-card-item class="bg-primary pb-2">
                <v-card-title>
                    {{ newSubCategory ? 'Create a New Sub-Category' : 'Edit Sub-Category' }}
                </v-card-title>
            </v-card-item>
            <c-loader-status :loaders="{ '': [subCategory.$save] }" />
            <v-card-text>
                <c-input :model="subCategory" for="name" variant="plain" class="text-white" single-line hide-details />
                <c-input :model="subCategory" for="allocation" variant="plain" class="text-white" single-line
                    hide-details />
                <c-input :model="subCategory" for="description" variant="plain" class="text-white" single-line
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
import { SubCategoryViewModel } from '@/viewmodels.g';

const props = defineProps<{
    categoryId: number;
    subCategory: SubCategoryViewModel;
}>();

const modelValue = defineModel<boolean>({ default: false });

const budgetService = inject(BUDGET_SERVICE)

const newSubCategory = computed(() => !props.subCategory.subCategoryId);

const emit = defineEmits<{
    (e: "saved"): void
}>();

async function save() {
    if (props.subCategory.name) {
        props.subCategory.budgetId = budgetService?.budget.value.budgetId!;
        props.subCategory.categoryId = props.categoryId;

        await props.subCategory.$save();
        modelValue.value = false;
        emit("saved");
    }
}
</script>