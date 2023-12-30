import { BudgetViewModel } from "@/viewmodels.g";

export default class BudgetService {
  constructor(budget: BudgetViewModel) {
    this.budget.value = budget;
  }

  public budget = ref(new BudgetViewModel());

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
