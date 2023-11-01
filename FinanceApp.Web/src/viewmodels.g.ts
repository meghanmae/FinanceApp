import * as $metadata from './metadata.g'
import * as $models from './models.g'
import * as $apiClients from './api-clients.g'
import { ViewModel, ListViewModel, ServiceViewModel, DeepPartial, defineProps } from 'coalesce-vue/lib/viewmodel'

export interface ApplicationUserViewModel extends $models.ApplicationUser {
  applicationUserId: string | null;
  azureObjectId: string | null;
  name: string | null;
  email: string | null;
}
export class ApplicationUserViewModel extends ViewModel<$models.ApplicationUser, $apiClients.ApplicationUserApiClient, string> implements $models.ApplicationUser  {
  
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
}
const listViewModelTypeLookup = ListViewModel.typeLookup = {
  ApplicationUser: ApplicationUserListViewModel,
}
const serviceViewModelTypeLookup = ServiceViewModel.typeLookup = {
  UserService: UserServiceViewModel,
}

