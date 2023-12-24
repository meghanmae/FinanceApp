<template>
  <c-loader-status :loaders="{ '': [category.$delete, subCategories.$load] }" />
  <v-card :style="color">
    <v-card variant="tonal" class="px-2" :color="category.color ?? 'primary'">
      <v-row
        dense
        align="center"
        @mouseover="showDelete = true"
        @mouseleave="showDelete = false"
      >
        <v-col cols="auto">
          <v-menu :close-on-content-click="false">
            <template v-slot:activator="{ props }">
              <v-btn
                :color="category.color ?? 'primary'"
                variant="tonal"
                :icon="category.icon!"
                v-bind="props"
              />
            </template>
            <v-card>
              <v-card-text>
                <IconPicker
                  v-model="category.icon"
                  :color="category.color ?? 'primary'"
                />
                <ColorPicker v-model="category.color" />
              </v-card-text>
            </v-card>
          </v-menu>
        </v-col>
        <v-col>
          <c-input
            :model="category"
            for="name"
            label=""
            variant="plain"
            hide-details
            class="input-heading mb-n5"
          />
          <c-input
            :model="category"
            for="description"
            label=""
            placeholder="no description"
            variant="plain"
            hide-details
            class="input-sub-heading"
          />
        </v-col>
        <v-col align="right">
          <v-btn
            v-if="showDelete"
            color="error"
            icon="fa-solid fa-trash"
            variant="tonal"
            size="small"
            @click="deleteCategory()"
          />
        </v-col>
      </v-row>
    </v-card>

    <div class="px-2">
      <template
        v-for="(subCategory, i) in subCategories.$items"
        :key="subCategory.subCategoryId!"
      >
        <SubCategoryRow
          :color="category.color ?? 'primary'"
          :subCategory="subCategory"
        />
        <v-divider v-if="i !== subCategories.$items.length - 1" />
      </template>

      <v-btn
        @click="showNewSubCategoryDialog = true"
        class="my-2"
        color="primary"
        variant="tonal"
      >
        <v-icon class="mb-1 mr-1"> fa-solid fa-plus </v-icon>
        Add Sub-Category
        <UpdateSubCategoryDialog
          v-model="showNewSubCategoryDialog"
          :subCategory="newSubCategory"
          :categoryId="category.categoryId!"
          @saved="loadSubCategories"
          :color="category.color ?? 'primary'"
        />
      </v-btn>
    </div>
  </v-card>
</template>

<script setup lang="ts">
import { SubCategory } from "@/models.g";
import {
  CategoryViewModel,
  SubCategoryListViewModel,
  SubCategoryViewModel,
} from "@/viewmodels.g";
import colors from "vuetify/lib/util/colors";

const props = defineProps<{
  category: CategoryViewModel;
}>();

const { category } = toRefs(props);
const showDelete = ref(false);

const showNewSubCategoryDialog = ref(false);
let newSubCategory: SubCategoryViewModel;

const subCategories = new SubCategoryListViewModel();
const datasource = new SubCategory.DataSources.SubCategoriesByBudget();
datasource.categoryId = category.value.categoryId;
subCategories.$dataSource = datasource;

const color = computed(() => {
  const color = Object.keys(colors).find(
    (key) =>
      key.toLowerCase() === props.category.color?.replace("-", "").toLowerCase()
  );
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
