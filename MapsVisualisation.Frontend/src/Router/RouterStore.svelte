<script lang="ts" context="module">
    import { writable } from 'svelte/store';
    import type { ComponentType } from 'svelte';

    export interface IRoute {
        path: string;
        component: ComponentType;
    }

    export interface IRouter {
        routes: IRoute[];
        currentPath: string;
    }

    const createRouter = () => {
        const { subscribe, update } = writable<IRouter>({routes: [], currentPath: '/'});

        const addRoute = (router: IRouter, path: string, component: ComponentType): IRouter => {
            if (router.routes.find(route => route.path === path)) return router;
            router.routes.push({path, component});
            return router;
        };

        const changePath = (router: IRouter, path: string): IRouter => {
            if (path === router.currentPath) return router;

            if (!router.routes.map(route => route.path).includes(path)){
                console.error(`Path "${path}" is not registered in Router component !`);
                return router;
            }
            window.history.pushState("", "", path);
            return {...router, currentPath: path};
        };

        return {
            subscribe,
            addRoute: (path: string, component: ComponentType) => update(router => addRoute(router, path, component)),
            changePath: (path: string) => update(router => changePath(router, path)),
        };
    };

    export const routerStore = createRouter();
</script>
