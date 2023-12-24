/* eslint-disable @typescript-eslint/no-empty-function */
import { createCoalesceVuetify } from "coalesce-vue-vuetify3";
import { enableAutoUnmount, mount } from "@vue/test-utils";
import { ArgumentsType } from "vitest";

import { createVuetify } from "vuetify";
import $metadata from "@/metadata.g";
import router from "@/router";

// Stub ResizeObserver, as vuetify's VApp relies on it.
global.ResizeObserver ??= class {
  observe() {}
  unobserve() {}
  disconnect() {}
};

// Automatically teardown components after each test. Especially necessary
// with Vuetify, which will attach dialogs and such to document.body.
enableAutoUnmount(afterEach);

const vuetify = createVuetify({});
const coalesceVuetify = createCoalesceVuetify({
  metadata: $metadata,
});

const mountComponent = function (
  component: ArgumentsType<typeof mount>[0],
  options: ArgumentsType<typeof mount>[1]
) {
  return mount(component, {
    ...options,
    attachTo: document.body,
    global: {
      plugins: [vuetify, coalesceVuetify, router],
    },
  });
} as typeof mount;

export { nextTick } from "vue";
export { flushPromises } from "@vue/test-utils";
export { mockEndpoint } from "coalesce-vue/lib/test-utils";
export { mountComponent as mount };
