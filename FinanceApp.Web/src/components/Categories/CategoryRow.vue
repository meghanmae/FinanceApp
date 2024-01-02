<template>
  <v-expansion-panel-title
    class="pa-0"
    hide-actions
    :color="isDarkMode ? 'black' : 'white'"
  >
    <template v-slot:default="{ expanded }">
      <v-card
        variant="tonal"
        class="px-2"
        :color="category.color ?? 'primary'"
        width="100%"
      >
        <v-row
          class="my-1"
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
                  size="small"
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
            <div v-if="expanded">
              <c-input
                @click.stop
                @keyup.space.prevent
                :model="category"
                for="name"
                label=""
                variant="underlined"
                hide-details
                class="input-heading"
              />
              <c-input
                @click.stop
                @keyup.space.prevent
                :model="category"
                for="description"
                label=""
                placeholder="no description"
                persistent-placeholder
                variant="underlined"
                hide-details
                class="input-sub-heading mb-2"
              />
            </div>
            <div v-else class="my-3">
              <strong class="input-heading">
                {{ category.name }}
              </strong>
              <br />
              <strong class="input-sub-heading">
                {{ category.description }}
              </strong>
            </div>
          </v-col>
          <v-col align="right" cols="12" sm="">
            <div class="d-inline-flex">
              <TotalDisplay :subCategories="subCategories.$items" />
              <v-btn
                :class="[
                  showDelete || display.smAndDown.value ? '' : 'hidden-element',
                  'ml-n10 mt-2',
                ]"
                color="error"
                icon="fa-solid fa-trash"
                variant="tonal"
                size="small"
                @click="deleteCategory()"
              />
            </div>
          </v-col>
        </v-row>
      </v-card>
    </template>
  </v-expansion-panel-title>
  <v-expansion-panel-text :style="color">
    <c-loader-status
      :loaders="{ '': [category.$delete, subCategories.$load] }"
    />
    <div class="ma-n3">
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
  </v-expansion-panel-text>
</template>

<script setup lang="ts">
import { BUDGET_SERVICE } from "@/lib/symbols";
import { SubCategory } from "@/models.g";
import {
  CategoryViewModel,
  SubCategoryListViewModel,
  SubCategoryViewModel,
} from "@/viewmodels.g";
import { useTheme } from "vuetify/lib/framework.mjs";
import colors from "vuetify/lib/util/colors";
import { useDisplay } from "vuetify";

const props = defineProps<{
  category: CategoryViewModel;
}>();

const { category } = toRefs(props);
const showDelete = ref(false);

props.category.$useAutoSave();

const budgetService = inject(BUDGET_SERVICE);

const display = useDisplay();

const showNewSubCategoryDialog = ref(false);
let newSubCategory: SubCategoryViewModel;

const subCategories = new SubCategoryListViewModel();
const datasource = new SubCategory.DataSources.SubCategoriesByBudget();
datasource.categoryId = category.value.categoryId;
datasource.startDate = budgetService!.startDate.value;
datasource.endDate = budgetService!.endDate.value;
subCategories.$dataSource = datasource;

const isDarkMode = computed(() => {
  return useTheme().current.value.dark;
});

const color = computed(() => {
  const color = Object.keys(colors).find(
    (key) =>
      key.toLowerCase() === props.category.color?.replace("-", "").toLowerCase()
  );
  return `border-left: 5px solid ${colors[color].base};`;
});

function loadSubCategories() {
  subCategories
    .$load()
    .then((x) =>
      budgetService!.updateSubCategories(x.data.list as SubCategoryViewModel[])
    );
  newSubCategory = new SubCategoryViewModel();
}
loadSubCategories();

function deleteCategory() {
  if (confirm()) {
    props.category.$delete();
  }
}

watch(budgetService!.startDate, () => {
  datasource.startDate = budgetService!.startDate.value;
  datasource.endDate = budgetService!.endDate.value;
  loadSubCategories();
});
</script>
