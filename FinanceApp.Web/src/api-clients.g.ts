import * as $metadata from './metadata.g'
import * as $models from './models.g'
import { AxiosClient, ModelApiClient, ServiceApiClient, ItemResult, ListResult } from 'coalesce-vue/lib/api-client'
import { AxiosPromise, AxiosResponse, AxiosRequestConfig } from 'axios'

export class ApplicationUserApiClient extends ModelApiClient<$models.ApplicationUser> {
  constructor() { super($metadata.ApplicationUser) }
}


export class BudgetApiClient extends ModelApiClient<$models.Budget> {
  constructor() { super($metadata.Budget) }
}


export class BudgetUserApiClient extends ModelApiClient<$models.BudgetUser> {
  constructor() { super($metadata.BudgetUser) }
}


export class CategoryApiClient extends ModelApiClient<$models.Category> {
  constructor() { super($metadata.Category) }
}


export class CustomCalculationApiClient extends ModelApiClient<$models.CustomCalculation> {
  constructor() { super($metadata.CustomCalculation) }
}


export class SubCategoryApiClient extends ModelApiClient<$models.SubCategory> {
  constructor() { super($metadata.SubCategory) }
}


export class SubCategoryCustomCalculationApiClient extends ModelApiClient<$models.SubCategoryCustomCalculation> {
  constructor() { super($metadata.SubCategoryCustomCalculation) }
}


export class TransactionApiClient extends ModelApiClient<$models.Transaction> {
  constructor() { super($metadata.Transaction) }
}


export class TransactionsServiceApiClient extends ServiceApiClient<typeof $metadata.TransactionsService> {
  constructor() { super($metadata.TransactionsService) }
  public historicalTransactions(budgetId: number | null, years: number | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.MonthlyTransactionsDto[]>> {
    const $method = this.$metadata.methods.historicalTransactions
    const $params =  {
      budgetId,
      years,
    }
    return this.$invoke($method, $params, $config)
  }
  
}


export class UserServiceApiClient extends ServiceApiClient<typeof $metadata.UserService> {
  constructor() { super($metadata.UserService) }
  public getLoggedInUser($config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.ApplicationUser>> {
    const $method = this.$metadata.methods.getLoggedInUser
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
}


