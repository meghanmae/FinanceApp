<template>
    <v-dialog max-width="800" v-model="modelValue">
        <v-card>
            <v-card-item class="bg-primary pb-2">
                <v-card-title>
                    {{ newCategory ? 'Create a New Category' : 'Edit Category' }}
                </v-card-title>
            </v-card-item>
            <c-loader-status :loaders="{ '': [category.$save] }" />
            <v-card-text>
                <c-input :model="category" for="name" variant="plain" class="text-white" single-line hide-details />
                <c-input :model="category" for="color" variant="plain" class="text-white" single-line hide-details />
                <c-input :model="category" for="icon" variant="plain" class="text-white" single-line hide-details />
                <c-input :model="category" for="description" variant="plain" class="text-white" single-line hide-details />
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
import { CategoryViewModel } from '@/viewmodels.g';

const props = defineProps<{
    category: CategoryViewModel;
}>();

const modelValue = defineModel<boolean>({ default: false });

const budgetService = inject(BUDGET_SERVICE)

const newCategory = computed(() => !props.category.categoryId);

const emit = defineEmits<{
    (e: "saved"): void
}>();

function save() {
    if (props.category.name) {
        props.category.budgetId = budgetService?.budget.value.budgetId;
        props.category.$save();
        modelValue.value = false;
        emit("saved");
    }
}
</script>