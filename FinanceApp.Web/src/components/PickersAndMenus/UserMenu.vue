<template>
  <v-menu>
    <template v-slot:activator="{ props }">
      <v-avatar color="white" v-bind="props" class="ma-5">
        <v-icon color="primary"> fa-solid fa-user </v-icon>
      </v-avatar>
    </template>
    <v-card>
      <v-list>
        <v-list-item>
          <template #prepend>
            <v-avatar color="white">
              <v-icon color="primary"> fa-solid fa-user </v-icon>
            </v-avatar>
          </template>
          <v-list-item-title>
            {{ $userName.split(" ")[0] }}
            <br />
            {{ $userEmail.split(" ")[0] }}
          </v-list-item-title>
        </v-list-item>

        <v-divider />

        <v-list-item @click="toggleTheme">
          <template #prepend>
            <v-icon> fa-solid fa-{{ isDarkTheme ? "sun" : "moon" }} </v-icon>
          </template>
          <v-list-item-title>
            {{ isDarkTheme ? "Light" : "Dark" }} Mode
          </v-list-item-title>
        </v-list-item>

        <v-list-item href="/MicrosoftIdentity/Account/SignOut">
          <template #prepend>
            <v-icon> fas fa-sign-out </v-icon>
          </template>
          <v-list-item-title> Sign Out </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-card>
  </v-menu>
</template>

<script setup lang="ts">
import { SYSTEM_THEME } from "@/lib/symbols";
import { useTheme } from "vuetify";

const theme = useTheme();

const isDarkTheme = computed(() => theme.global.name.value == "dark");

function toggleTheme() {
  const newTheme = isDarkTheme.value ? "light" : "dark";
  theme.global.name.value = newTheme;
  localStorage.setItem(SYSTEM_THEME, newTheme);
}
</script>
