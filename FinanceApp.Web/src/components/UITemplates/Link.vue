<template>
    <v-hover>
        <template v-slot:default="{ isHovering, props }">
            <router-link v-bind="props" :to="to" :class="isHovering ? textHoverColor : textColor" class="text-truncate">
                {{ text }}
            </router-link>
        </template>
    </v-hover>
</template>
  
<script setup lang="ts">
import { useTheme } from "vuetify";

const props = withDefaults(
    defineProps<{
        text: string;
        to: string;
        color?: string;
    }>(),
    {
        color: "primary",
    }
);

const theme = useTheme();

const textColor = computed(() => {
    return theme.global.current.value.dark
        ? "text-" + props.color + "-lighten-2"
        : "text-" + props.color + "-darken-2";
});

const textHoverColor = computed(() => "text-" + props.color);
</script>