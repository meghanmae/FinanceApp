import { BudgetViewModel } from "@/viewmodels.g";

export default class BudgetService {
  constructor(budget: BudgetViewModel) {
    this.budget.value = budget;
  }

  public budget = ref(new BudgetViewModel());
}
