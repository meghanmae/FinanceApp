<template>
    <c-loader-status :loaders="{ '': [category.$delete, subCategories.$load] }" />
    <v-card :style="color">
        <v-row align="center">
            <v-col>
                <v-card-title>
                    <v-icon :color="category.color ?? 'primary'">
                        {{ category.icon }}
                    </v-icon>
                    {{ category.name }}
                </v-card-title>
                <v-card-subtitle class="text-white">
                    {{ category.description }}
                </v-card-subtitle>
            </v-col>
            <v-col align="right">
                <v-card-title>
                    <v-btn :color="category.color!" icon="fa-solid fa-pencil" size="small" variant="tonal" class="mr-3"
                        @click="editCategoryDialog = true" />
                    <UpdateCategoryDialog v-model="editCategoryDialog" :category="category" />

                    <v-btn color="error" icon="fa-solid fa-trash" variant="tonal" size="small" @click="deleteCategory()" />
                </v-card-title>
            </v-col>
        </v-row>

        <v-card-text>
            <template v-for="(subCategory, i) in subCategories.$items" :key="subCategory.subCategoryId!">
                <SubCategoryRow :color="category.color ?? 'primary'" :subCategory="subCategory" />
                <v-divider v-if="i !== subCategories.$items.length - 1" />
            </template>

            <v-btn @click="showNewSubCategoryDialog = true" class="mt-2" color="primary" variant="tonal">
                <v-icon class="mb-1 mr-1">
                    fa-solid fa-plus
                </v-icon>
                Add Sub-Category
                <UpdateSubCategoryDialog v-model="showNewSubCategoryDialog" :subCategory="newSubCategory"
                    :categoryId="category.categoryId!" @saved="loadSubCategories" :color="category.color ?? 'primary'" />
            </v-btn>
        </v-card-text>
    </v-card>
</template>

<script setup lang="ts">
import { Category } from '@/metadata.g';
import { SubCategory } from '@/models.g';
import { CategoryViewModel, SubCategoryListViewModel, SubCategoryViewModel } from '@/viewmodels.g';
import colors from "vuetify/lib/util/colors";

const proxy = getCurrentInstance()?.proxy;

const props = defineProps<{
    category: CategoryViewModel;
}>();

const editCategoryDialog = ref(false);

const showNewSubCategoryDialog = ref(false);
let newSubCategory: SubCategoryViewModel;

const subCategories = new SubCategoryListViewModel();
const datasource = new SubCategory.DataSources.SubCategoriesByBudget();
datasource.categoryId = props.category.categoryId;
subCategories.$dataSource = datasource;

const color = computed(() => {
    const color = Object.keys(colors).find(key => key.toLowerCase() === props.category.color?.replace('-', '').toLowerCase());
    return `border-left: 5px solid ${colors[color].base};`;
});

function loadSubCategories() {
    subCategories.$load();
    newSubCategory = new SubCategoryViewModel();
}
loadSubCategories();

function deleteCategory() {
    if (confirm()) {
        props.category.$delete();
    }
}
</script>