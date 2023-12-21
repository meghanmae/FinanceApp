<template>
    <c-loader-status :loaders="{ '': [category.$delete, subCategories.$load] }" />
    <v-card :color="category.color!" variant="tonal">
        <v-card :color="category.color!" variant="tonal">
            <v-row align="center">
                <v-col>
                    <v-card-title>
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

                        <v-btn color="error" icon="fa-solid fa-trash" variant="tonal" size="small"
                            @click="deleteCategory()" />
                    </v-card-title>
                </v-col>
            </v-row>
        </v-card>

        <v-sheet :color="category.color!" height="5px" />

        <v-card-text>
            <SubCategoryRow v-for="subCategory in subCategories.$items" :key="subCategory.subCategoryId!"
                :subCategory="subCategory" />

            <v-btn @click="showNewSubCategoryDialog = true" class="mt-2">
                Add SubCategory
                <UpdateSubCategoryDialog v-model="showNewSubCategoryDialog" :subCategory="newSubCategory"
                    :categoryId="category.categoryId!" @saved="loadSubCategories" />
            </v-btn>
        </v-card-text>
    </v-card>
</template>

<script setup lang="ts">
import { SubCategory } from '@/models.g';
import { CategoryViewModel, SubCategoryListViewModel, SubCategoryViewModel } from '@/viewmodels.g';

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