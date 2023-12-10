import { UserServiceViewModel } from "@/viewmodels.g";

const userService = new UserServiceViewModel();

userService.getLoggedInUser.setConcurrency("cancel");
userService.getLoggedInUser();

declare module "@vue/runtime-core" {
  interface ComponentCustomProperties {
    readonly $userName: string;
    readonly $userId: string;
    readonly $userEmail: string;
  }
}

document.addEventListener("visibilitychange", () => {
  if (!document.hidden) userService.getLoggedInUser();
},
  false
);

export const globalProperties = {
  get $userName(): string {
    return userService.getLoggedInUser.result?.name ?? "";
  },
  get $userId(): string {
    return userService.getLoggedInUser.result?.applicationUserId ?? "";
  },
  get $userEmail(): string {
    return userService.getLoggedInUser.result?.email ?? "";
  }
}

export default userService