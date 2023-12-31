import * as metadata from './metadata.g'
import { Model, DataSource, convertToModel, mapToModel } from 'coalesce-vue/lib/model'

export interface ApplicationUser extends Model<typeof metadata.ApplicationUser> {
  applicationUserId: string | null
  azureObjectId: string | null
  name: string | null
  email: string | null
  budgetUsers: BudgetUser[] | null
}
export class ApplicationUser {
  
  /** Mutates the input object and its descendents into a valid ApplicationUser implementation. */
  static convert(data?: Partial<ApplicationUser>): ApplicationUser {
    return convertToModel(data || {}, metadata.ApplicationUser) 
  }
  
  /** Maps the input object and its descendents to a new, valid ApplicationUser implementation. */
  static map(data?: Partial<ApplicationUser>): ApplicationUser {
    return mapToModel(data || {}, metadata.ApplicationUser) 
  }
  
  /** Instantiate a new ApplicationUser, optionally basing it on the given data. */
  constructor(data?: Partial<ApplicationUser> | {[k: string]: any}) {
    Object.assign(this, ApplicationUser.map(data || {}));
  }
}


export interface Budget extends Model<typeof metadata.Budget> {
  budgetId: number | null
  name: string | null
  color: string | null
  allocation: number | null
  description: string | null
  budgetUsers: BudgetUser[] | null
  categories: Category[] | null
}
export class Budget {
  
  /** Mutates the input object and its descendents into a valid Budget implementation. */
  static convert(data?: Partial<Budget>): Budget {
    return convertToModel(data || {}, metadata.Budget) 
  }
  
  /** Maps the input object and its descendents to a new, valid Budget implementation. */
  static map(data?: Partial<Budget>): Budget {
    return mapToModel(data || {}, metadata.Budget) 
  }
  
  /** Instantiate a new Budget, optionally basing it on the given data. */
  constructor(data?: Partial<Budget> | {[k: string]: any}) {
    Object.assign(this, Budget.map(data || {}));
  }
}
export namespace Budget {
  export namespace DataSources {
    
    export class BudgetsForUser implements DataSource<typeof metadata.Budget.dataSources.budgetsForUser> {
      readonly $metadata = metadata.Budget.dataSources.budgetsForUser
    }
  }
}


export interface BudgetUser extends Model<typeof metadata.BudgetUser> {
  budgetUserId: number | null
  applicationUserId: string | null
  applicationUser: ApplicationUser | null
  budgetId: number | null
}
export class BudgetUser {
  
  /** Mutates the input object and its descendents into a valid BudgetUser implementation. */
  static convert(data?: Partial<BudgetUser>): BudgetUser {
    return convertToModel(data || {}, metadata.BudgetUser) 
  }
  
  /** Maps the input object and its descendents to a new, valid BudgetUser implementation. */
  static map(data?: Partial<BudgetUser>): BudgetUser {
    return mapToModel(data || {}, metadata.BudgetUser) 
  }
  
  /** Instantiate a new BudgetUser, optionally basing it on the given data. */
  constructor(data?: Partial<BudgetUser> | {[k: string]: any}) {
    Object.assign(this, BudgetUser.map(data || {}));
  }
}


export interface Category extends Model<typeof metadata.Category> {
  categoryId: number | null
  name: string | null
  description: string | null
  color: string | null
  icon: string | null
  subCategories: SubCategory[] | null
  budgetId: number | null
}
export class Category {
  
  /** Mutates the input object and its descendents into a valid Category implementation. */
  static convert(data?: Partial<Category>): Category {
    return convertToModel(data || {}, metadata.Category) 
  }
  
  /** Maps the input object and its descendents to a new, valid Category implementation. */
  static map(data?: Partial<Category>): Category {
    return mapToModel(data || {}, metadata.Category) 
  }
  
  /** Instantiate a new Category, optionally basing it on the given data. */
  constructor(data?: Partial<Category> | {[k: string]: any}) {
    Object.assign(this, Category.map(data || {}));
  }
}
export namespace Category {
  export namespace DataSources {
    
    export class CategoriesByBudget implements DataSource<typeof metadata.Category.dataSources.categoriesByBudget> {
      readonly $metadata = metadata.Category.dataSources.categoriesByBudget
      budgetId: number | null = null
      
      constructor(params?: Omit<Partial<CategoriesByBudget>, '$metadata'>) {
        if (params) Object.assign(this, params);
      }
    }
  }
}


export interface CustomCalculation extends Model<typeof metadata.CustomCalculation> {
  customCalculationId: number | null
  name: string | null
  description: string | null
  subCategoryCustomCalculations: SubCategoryCustomCalculation[] | null
  budgetId: number | null
}
export class CustomCalculation {
  
  /** Mutates the input object and its descendents into a valid CustomCalculation implementation. */
  static convert(data?: Partial<CustomCalculation>): CustomCalculation {
    return convertToModel(data || {}, metadata.CustomCalculation) 
  }
  
  /** Maps the input object and its descendents to a new, valid CustomCalculation implementation. */
  static map(data?: Partial<CustomCalculation>): CustomCalculation {
    return mapToModel(data || {}, metadata.CustomCalculation) 
  }
  
  /** Instantiate a new CustomCalculation, optionally basing it on the given data. */
  constructor(data?: Partial<CustomCalculation> | {[k: string]: any}) {
    Object.assign(this, CustomCalculation.map(data || {}));
  }
}
export namespace CustomCalculation {
  export namespace DataSources {
    
    export class CustomCalculationsByBudget implements DataSource<typeof metadata.CustomCalculation.dataSources.customCalculationsByBudget> {
      readonly $metadata = metadata.CustomCalculation.dataSources.customCalculationsByBudget
      budgetId: number | null = null
      
      constructor(params?: Omit<Partial<CustomCalculationsByBudget>, '$metadata'>) {
        if (params) Object.assign(this, params);
      }
    }
  }
}


export interface SubCategory extends Model<typeof metadata.SubCategory> {
  subCategoryId: number | null
  name: string | null
  description: string | null
  allocation: number | null
  categoryId: number | null
  category: Category | null
  
  /** A category that would not have transactions assoicated with it */
  isStatic: boolean | null
  subCategoryCustomCalculations: SubCategoryCustomCalculation[] | null
  transactions: Transaction[] | null
  budgetId: number | null
}
export class SubCategory {
  
  /** Mutates the input object and its descendents into a valid SubCategory implementation. */
  static convert(data?: Partial<SubCategory>): SubCategory {
    return convertToModel(data || {}, metadata.SubCategory) 
  }
  
  /** Maps the input object and its descendents to a new, valid SubCategory implementation. */
  static map(data?: Partial<SubCategory>): SubCategory {
    return mapToModel(data || {}, metadata.SubCategory) 
  }
  
  /** Instantiate a new SubCategory, optionally basing it on the given data. */
  constructor(data?: Partial<SubCategory> | {[k: string]: any}) {
    Object.assign(this, SubCategory.map(data || {}));
  }
}
export namespace SubCategory {
  export namespace DataSources {
    
    export class SubCategoriesByBudget implements DataSource<typeof metadata.SubCategory.dataSources.subCategoriesByBudget> {
      readonly $metadata = metadata.SubCategory.dataSources.subCategoriesByBudget
      categoryId: number | null = null
      budgetId: number | null = null
      
      constructor(params?: Omit<Partial<SubCategoriesByBudget>, '$metadata'>) {
        if (params) Object.assign(this, params);
      }
    }
  }
}


export interface SubCategoryCustomCalculation extends Model<typeof metadata.SubCategoryCustomCalculation> {
  subCategoryCustomCalculationId: number | null
  subCategoryId: number | null
  subCategory: SubCategory | null
  customCalculationId: number | null
  customCalculation: CustomCalculation | null
  budgetId: number | null
}
export class SubCategoryCustomCalculation {
  
  /** Mutates the input object and its descendents into a valid SubCategoryCustomCalculation implementation. */
  static convert(data?: Partial<SubCategoryCustomCalculation>): SubCategoryCustomCalculation {
    return convertToModel(data || {}, metadata.SubCategoryCustomCalculation) 
  }
  
  /** Maps the input object and its descendents to a new, valid SubCategoryCustomCalculation implementation. */
  static map(data?: Partial<SubCategoryCustomCalculation>): SubCategoryCustomCalculation {
    return mapToModel(data || {}, metadata.SubCategoryCustomCalculation) 
  }
  
  /** Instantiate a new SubCategoryCustomCalculation, optionally basing it on the given data. */
  constructor(data?: Partial<SubCategoryCustomCalculation> | {[k: string]: any}) {
    Object.assign(this, SubCategoryCustomCalculation.map(data || {}));
  }
}
export namespace SubCategoryCustomCalculation {
  export namespace DataSources {
    
    export class SubCategoriesByBudget implements DataSource<typeof metadata.SubCategoryCustomCalculation.dataSources.subCategoriesByBudget> {
      readonly $metadata = metadata.SubCategoryCustomCalculation.dataSources.subCategoriesByBudget
      budgetId: number | null = null
      
      constructor(params?: Omit<Partial<SubCategoriesByBudget>, '$metadata'>) {
        if (params) Object.assign(this, params);
      }
    }
  }
}


export interface Transaction extends Model<typeof metadata.Transaction> {
  transactionId: number | null
  description: string | null
  amount: number | null
  subCategoryId: number | null
  subCategory: SubCategory | null
  budgetId: number | null
}
export class Transaction {
  
  /** Mutates the input object and its descendents into a valid Transaction implementation. */
  static convert(data?: Partial<Transaction>): Transaction {
    return convertToModel(data || {}, metadata.Transaction) 
  }
  
  /** Maps the input object and its descendents to a new, valid Transaction implementation. */
  static map(data?: Partial<Transaction>): Transaction {
    return mapToModel(data || {}, metadata.Transaction) 
  }
  
  /** Instantiate a new Transaction, optionally basing it on the given data. */
  constructor(data?: Partial<Transaction> | {[k: string]: any}) {
    Object.assign(this, Transaction.map(data || {}));
  }
}
export namespace Transaction {
  export namespace DataSources {
    
    export class TransactionsByBudget implements DataSource<typeof metadata.Transaction.dataSources.transactionsByBudget> {
      readonly $metadata = metadata.Transaction.dataSources.transactionsByBudget
      subCategoryId: number | null = null
      budgetId: number | null = null
      
      constructor(params?: Omit<Partial<TransactionsByBudget>, '$metadata'>) {
        if (params) Object.assign(this, params);
      }
    }
  }
}


