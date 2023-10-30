import * as $metadata from './metadata.g'
import * as $models from './models.g'
import { AxiosClient, ModelApiClient, ServiceApiClient, ItemResult, ListResult } from 'coalesce-vue/lib/api-client'
import { AxiosPromise, AxiosResponse, AxiosRequestConfig } from 'axios'

export class ApplicationUserApiClient extends ModelApiClient<$models.ApplicationUser> {
  constructor() { super($metadata.ApplicationUser) }
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


