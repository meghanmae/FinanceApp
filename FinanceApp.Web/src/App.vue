<template>
  <v-app id="vue-app">
    <v-app-bar color="primary">
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title>
        <router-link to="/" style="color: inherit">
          <v-icon class="mr-2 mb-3"> fa-solid fa-piggy-bank </v-icon>
          Penny Pincher
        </router-link>
      </v-toolbar-title>
      <UserMenu />
    </v-app-bar>
    <v-navigation-drawer v-model="drawer">
      <v-list>
        <v-list-item link to="/">
          <template #prepend>
            <v-icon>fas fa-home</v-icon>
          </template>
          <v-list-item-title> Dashboard </v-list-item-title>
        </v-list-item>

        <v-list-item link to="/admin">
          <template #prepend>
            <v-icon>fas fa-cogs</v-icon>
          </template>
          <v-list-item-title>Admin Pages</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <!-- https://stackoverflow.com/questions/52847979/what-is-router-view-key-route-fullpath -->
      <router-view v-slot="{ Component, route }">
        <transition name="router-transition" mode="out-in" appear>
          <component :is="Component" :key="route.path" />
        </transition>
      </router-view>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
const drawer = ref<boolean | null>(null);
</script>

<style lang="scss">
.router-transition-enter-active,
.router-transition-leave-active {
  transition: 0.1s ease-out;
}

.router-transition-enter-from,
.router-transition-leave-to {
  opacity: 0;
}
</style>
