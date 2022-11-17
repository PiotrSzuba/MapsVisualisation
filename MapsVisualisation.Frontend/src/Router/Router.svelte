<script lang="ts">
    import { afterUpdate } from 'svelte';
    import type { ComponentType } from 'svelte';
    import type { IRouter } from './RouterStore.svelte';
    import { routerStore } from './RouterStore.svelte';
    import EmptyComponent from './EmptyComponent.svelte';

    const renderPage = (router: IRouter): ComponentType => {
        if (router.routes.length === 0) return EmptyComponent;
        
        const route = router.routes.find(r => r.path === router.currentPath);

        if (!route) return EmptyComponent;

        return route.component;
    };

    afterUpdate(() => {
        if (window.location.pathname === $routerStore.currentPath) return;
        
        routerStore.changePath(window.location.pathname);
    });

</script>

<svelte:component this={renderPage($routerStore)} />

<slot></slot>