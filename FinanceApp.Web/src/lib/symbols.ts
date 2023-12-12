import BudgetService from "@/services/BudgetService";

// Local Storage Symbols
export const SYSTEM_THEME = "SYSTEM_THEME";

// Unique symbols used for injection
export const BUDGET_SERVICE = Symbol(
    "BUDGET_SERVICE"
) as InjectionKey<BudgetService>;