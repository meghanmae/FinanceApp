import * as $metadata from './metadata.g'
import * as $models from './models.g'
import * as $apiClients from './api-clients.g'
import { ViewModel, ListViewModel, ServiceViewModel, DeepPartial, defineProps } from 'coalesce-vue/lib/viewmodel'

export interface ApplicationUserViewModel extends $models.ApplicationUser {
  applicationUserId: string | null;
  azureObjectId: string | null;
  name: string | null;
  email: string | null;
  budgetUsers: BudgetUserViewModel[] | null;
}
export class ApplicationUserViewModel extends ViewModel<$models.ApplicationUser, $apiClients.ApplicationUserApiClient, string> implements $models.ApplicationUser  {
  
  
  public addToBudgetUsers(initialData?: DeepPartial<$models.BudgetUser> | null) {
    return this.$addChild('budgetUsers', initialData) as BudgetUserViewModel
  }
  
  constructor(initialData?: DeepPartial<$models.ApplicationUser> | null) {
    super($metadata.ApplicationUser, new $apiClients.ApplicationUserApiClient(), initialData)
  }
}
defineProps(ApplicationUserViewModel, $metadata.ApplicationUser)

export class ApplicationUserListViewModel extends ListViewModel<$models.ApplicationUser, $apiClients.ApplicationUserApiClient, ApplicationUserViewModel> {
  
  constructor() {
    super($metadata.ApplicationUser, new $apiClients.ApplicationUserApiClient())
  }
}


export interface BudgetViewModel extends $models.Budget {
  budgetId: number | null;
  name: string | null;
  color: string | null;
  allocation: number | null;
  description: string | null;
  budgetUsers: BudgetUserViewModel[] | null;
  categories: CategoryViewModel[] | null;
}
export class BudgetViewModel extends ViewModel<$models.Budget, $apiClients.BudgetApiClient, number> implements $models.Budget  {
  
  constructor(initialData?: DeepPartial<$models.Budget> | null) {
    super($metadata.Budget, new $apiClients.BudgetApiClient(), initialData)
  }
}
defineProps(BudgetViewModel, $metadata.Budget)

export class BudgetListViewModel extends ListViewModel<$models.Budget, $apiClients.BudgetApiClient, BudgetViewModel> {
  
  constructor() {
    super($metadata.Budget, new $apiClients.BudgetApiClient())
  }
}


export interface BudgetUserViewModel extends $models.BudgetUser {
  budgetUserId: number | null;
  applicationUserId: string | null;
  applicationUser: ApplicationUserViewModel | null;
  budgetId: number | null;
}
export class BudgetUserViewModel extends ViewModel<$models.BudgetUser, $apiClients.BudgetUserApiClient, number> implements $models.BudgetUser  {
  
  constructor(initialData?: DeepPartial<$models.BudgetUser> | null) {
    super($metadata.BudgetUser, new $apiClients.BudgetUserApiClient(), initialData)
  }
}
defineProps(BudgetUserViewModel, $metadata.BudgetUser)

export class BudgetUserListViewModel extends ListViewModel<$models.BudgetUser, $apiClients.BudgetUserApiClient, BudgetUserViewModel> {
  
  constructor() {
    super($metadata.BudgetUser, new $apiClients.BudgetUserApiClient())
  }
}


export interface CategoryViewModel extends $models.Category {
  categoryId: number | null;
  name: string | null;
  description: string | null;
  color: string | null;
  icon: string | null;
  subCategories: SubCategoryViewModel[] | null;
  budgetId: number | null;
}
export class CategoryViewModel extends ViewModel<$models.Category, $apiClients.CategoryApiClient, number> implements $models.Category  {
  
  
  public addToSubCategories(initialData?: DeepPartial<$models.SubCategory> | null) {
    return this.$addChild('subCategories', initialData) as SubCategoryViewModel
  }
  
  constructor(initialData?: DeepPartial<$models.Category> | null) {
    super($metadata.Category, new $apiClients.CategoryApiClient(), initialData)
  }
}
defineProps(CategoryViewModel, $metadata.Category)

export class CategoryListViewModel extends ListViewModel<$models.Category, $apiClients.CategoryApiClient, CategoryViewModel> {
  
  constructor() {
    super($metadata.Category, new $apiClients.CategoryApiClient())
  }
}


export interface CustomCalculationViewModel extends $models.CustomCalculation {
  customCalculationId: number | null;
  name: string | null;
  description: string | null;
  subCategoryCustomCalculations: SubCategoryCustomCalculationViewModel[] | null;
  budgetId: number | null;
}
export class CustomCalculationViewModel extends ViewModel<$models.CustomCalculation, $apiClients.CustomCalculationApiClient, number> implements $models.CustomCalculation  {
  
  
  public addToSubCategoryCustomCalculations(initialData?: DeepPartial<$models.SubCategoryCustomCalculation> | null) {
    return this.$addChild('subCategoryCustomCalculations', initialData) as SubCategoryCustomCalculationViewModel
  }
  
  constructor(initialData?: DeepPartial<$models.CustomCalculation> | null) {
    super($metadata.CustomCalculation, new $apiClients.CustomCalculationApiClient(), initialData)
  }
}
defineProps(CustomCalculationViewModel, $metadata.CustomCalculation)

export class CustomCalculationListViewModel extends ListViewModel<$models.CustomCalculation, $apiClients.CustomCalculationApiClient, CustomCalculationViewModel> {
  
  constructor() {
    super($metadata.CustomCalculation, new $apiClients.CustomCalculationApiClient())
  }
}


export interface SubCategoryViewModel extends $models.SubCategory {
  subCategoryId: number | null;
  name: string | null;
  description: string | null;
  allocation: number | null;
  categoryId: number | null;
  category: CategoryViewModel | null;
  
  /** A category that would not have transactions assoicated with it */
  isStatic: boolean | null;
  subCategoryCustomCalculations: SubCategoryCustomCalculationViewModel[] | null;
  transactions: TransactionViewModel[] | null;
  budgetId: number | null;
}
export class SubCategoryViewModel extends ViewModel<$models.SubCategory, $apiClients.SubCategoryApiClient, number> implements $models.SubCategory  {
  
  
  public addToSubCategoryCustomCalculations(initialData?: DeepPartial<$models.SubCategoryCustomCalculation> | null) {
    return this.$addChild('subCategoryCustomCalculations', initialData) as SubCategoryCustomCalculationViewModel
  }
  
  
  public addToTransactions(initialData?: DeepPartial<$models.Transaction> | null) {
    return this.$addChild('transactions', initialData) as TransactionViewModel
  }
  
  constructor(initialData?: DeepPartial<$models.SubCategory> | null) {
    super($metadata.SubCategory, new $apiClients.SubCategoryApiClient(), initialData)
  }
}
defineProps(SubCategoryViewModel, $metadata.SubCategory)

export class SubCategoryListViewModel extends ListViewModel<$models.SubCategory, $apiClients.SubCategoryApiClient, SubCategoryViewModel> {
  
  constructor() {
    super($metadata.SubCategory, new $apiClients.SubCategoryApiClient())
  }
}


export interface SubCategoryCustomCalculationViewModel extends $models.SubCategoryCustomCalculation {
  subCategoryCustomCalculationId: number | null;
  subCategoryId: number | null;
  subCategory: SubCategoryViewModel | null;
  customCalculationId: number | null;
  customCalculation: CustomCalculationViewModel | null;
  budgetId: number | null;
}
export class SubCategoryCustomCalculationViewModel extends ViewModel<$models.SubCategoryCustomCalculation, $apiClients.SubCategoryCustomCalculationApiClient, number> implements $models.SubCategoryCustomCalculation  {
  
  constructor(initialData?: DeepPartial<$models.SubCategoryCustomCalculation> | null) {
    super($metadata.SubCategoryCustomCalculation, new $apiClients.SubCategoryCustomCalculationApiClient(), initialData)
  }
}
defineProps(SubCategoryCustomCalculationViewModel, $metadata.SubCategoryCustomCalculation)

export class SubCategoryCustomCalculationListViewModel extends ListViewModel<$models.SubCategoryCustomCalculation, $apiClients.SubCategoryCustomCalculationApiClient, SubCategoryCustomCalculationViewModel> {
  
  constructor() {
    super($metadata.SubCategoryCustomCalculation, new $apiClients.SubCategoryCustomCalculationApiClient())
  }
}


export interface TransactionViewModel extends $models.Transaction {
  transactionId: number | null;
  description: string | null;
  amount: number | null;
  subCategoryId: number | null;
  subCategory: SubCategoryViewModel | null;
  transactionDate: Date | null;
  budgetId: number | null;
}
export class TransactionViewModel extends ViewModel<$models.Transaction, $apiClients.TransactionApiClient, number> implements $models.Transaction  {
  
  constructor(initialData?: DeepPartial<$models.Transaction> | null) {
    super($metadata.Transaction, new $apiClients.TransactionApiClient(), initialData)
  }
}
defineProps(TransactionViewModel, $metadata.Transaction)

export class TransactionListViewModel extends ListViewModel<$models.Transaction, $apiClients.TransactionApiClient, TransactionViewModel> {
  
  constructor() {
    super($metadata.Transaction, new $apiClients.TransactionApiClient())
  }
}


export class TransactionsServiceViewModel extends ServiceViewModel<typeof $metadata.TransactionsService, $apiClients.TransactionsServiceApiClient> {
  
  public get historicalTransactions() {
    const historicalTransactions = this.$apiClient.$makeCaller(
      this.$metadata.methods.historicalTransactions,
      (c, budgetId: number | null, years: number | null) => c.historicalTransactions(budgetId, years),
      () => ({budgetId: null as number | null, years: null as number | null, }),
      (c, args) => c.historicalTransactions(args.budgetId, args.years))
    
    Object.defineProperty(this, 'historicalTransactions', {value: historicalTransactions});
    return historicalTransactions
  }
  
  constructor() {
    super($metadata.TransactionsService, new $apiClients.TransactionsServiceApiClient())
  }
}


export class UserServiceViewModel extends ServiceViewModel<typeof $metadata.UserService, $apiClients.UserServiceApiClient> {
  
  public get getLoggedInUser() {
    const getLoggedInUser = this.$apiClient.$makeCaller(
      this.$metadata.methods.getLoggedInUser,
      (c) => c.getLoggedInUser(),
      () => ({}),
      (c, args) => c.getLoggedInUser())
    
    Object.defineProperty(this, 'getLoggedInUser', {value: getLoggedInUser});
    return getLoggedInUser
  }
  
  constructor() {
    super($metadata.UserService, new $apiClients.UserServiceApiClient())
  }
}


const viewModelTypeLookup = ViewModel.typeLookup = {
  ApplicationUser: ApplicationUserViewModel,
  Budget: BudgetViewModel,
  BudgetUser: BudgetUserViewModel,
  Category: CategoryViewModel,
  CustomCalculation: CustomCalculationViewModel,
  SubCategory: SubCategoryViewModel,
  SubCategoryCustomCalculation: SubCategoryCustomCalculationViewModel,
  Transaction: TransactionViewModel,
}
const listViewModelTypeLookup = ListViewModel.typeLookup = {
  ApplicationUser: ApplicationUserListViewModel,
  Budget: BudgetListViewModel,
  BudgetUser: BudgetUserListViewModel,
  Category: CategoryListViewModel,
  CustomCalculation: CustomCalculationListViewModel,
  SubCategory: SubCategoryListViewModel,
  SubCategoryCustomCalculation: SubCategoryCustomCalculationListViewModel,
  Transaction: TransactionListViewModel,
}
const serviceViewModelTypeLookup = ServiceViewModel.typeLookup = {
  TransactionsService: TransactionsServiceViewModel,
  UserService: UserServiceViewModel,
}

