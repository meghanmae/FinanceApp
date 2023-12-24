import { createApp } from "vue";
import { createVuetify } from "vuetify";
import { createCoalesceVuetify } from "coalesce-vue-vuetify3";
import { aliases, fa } from "vuetify/iconsets/fa";
import { AxiosClient as CoalesceAxiosClient } from "coalesce-vue";

import userService, {
  globalProperties as userServiceProps,
} from "@/services/UserService";

import App from "./App.vue";
import router from "./router";

// Import global CSS and Fonts:
import "typeface-roboto";
import "@fortawesome/fontawesome-free/css/all.css";
import "coalesce-vue-vuetify3/styles.css";
import "@/site.scss";
import "vuetify/styles";

import $metadata from "@/metadata.g";
// viewmodels.g has side effects - it populates the global lookup on ViewModel and ListViewModel.
// This global lookup allows the admin page components to function.
import "@/viewmodels.g";

// SETUP: vuetify
const inputDefaults = { density: "compact", variant: "outlined" };
const vuetify = createVuetify({
  icons: {
    defaultSet: "fa",
    aliases,
    sets: { fa },
  },
  defaults: {
    VTextField: inputDefaults,
    VTextarea: inputDefaults,
    VSelect: inputDefaults,
    VCombobox: inputDefaults,
    VAutocomplete: inputDefaults,
    VField: inputDefaults,
    VInput: inputDefaults,
    VSwitch: { color: "primary" }, // https://github.com/vuetifyjs/vuetify/issues/16486
  },
  theme: {
    defaultTheme: "dark",
    themes: {
      light: {
        colors: {
          primary: "#3F51B5",
          error: "#F44336",
        },
      },
      dark: {
        colors: {
          primary: "#3F51B5",
          error: "#F44336",
        },
      },
    },
  },
});

// SETUP: coalesce-vue
CoalesceAxiosClient.defaults.baseURL = "/api";
CoalesceAxiosClient.defaults.withCredentials = true;

// SETUP: coalesce-vue-vuetify
const coalesceVuetify = createCoalesceVuetify({
  metadata: $metadata,
});

const app = createApp(App);
Object.defineProperties(
  app.config.globalProperties,
  Object.getOwnPropertyDescriptors(userServiceProps)
);

app.use(router);
router.beforeEach(async (to: any, from: any, next: any) => {
  if (from.path !== to.path) {
    await userService.getLoggedInUser();
  }
  next();
});

app.use(vuetify);
app.use(coalesceVuetify);
app.mount("#app");
