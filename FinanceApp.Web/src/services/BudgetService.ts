import { BudgetViewModel, SubCategoryViewModel } from "@/viewmodels.g";
import { startOfToday, endOfMonth } from "date-fns";

export default class BudgetService {
  constructor(budget: BudgetViewModel) {
    this.budget.value = budget;
  }

  public startDate = ref(startOfToday());
  public endDate = ref(endOfMonth(startOfToday()));

  public budget = ref(new BudgetViewModel());
  public allSubCategories = ref([] as SubCategoryViewModel[]);

  public updateSubCategories(subCategories: SubCategoryViewModel[]) {
    const ids = subCategories.map((x) => x.subCategoryId);

    // Filter out all the old subcategories that match
    this.allSubCategories.value = this.allSubCategories.value.filter(
      (x) => !ids.includes(x.subCategoryId)
    );

    this.allSubCategories.value =
      this.allSubCategories.value.concat(subCategories);
  }

  public static formatCurrency(
    amount: number,
    currency = "USD",
    locale = "en-US"
  ) {
    return new Intl.NumberFormat(locale, {
      style: "currency",
      currency: currency,
    }).format(amount);
  }
}
