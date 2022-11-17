<script lang="ts">
    import type { ComponentType } from 'svelte';
    import { routerStore } from './RouterStore.svelte';

    export let path: string;
    export let component: ComponentType;

    routerStore.addRoute(path, component);
</script>
