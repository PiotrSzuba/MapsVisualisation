<script lang="ts">
    import { routerStore } from './RouterStore.svelte';

    export let to: string;

    const onClick = () => {
        routerStore.changePath(to);
    };

</script>

<div on:click={onClick} on:keypress={onClick} class={$$props.class}>
    <slot></slot>
</div>