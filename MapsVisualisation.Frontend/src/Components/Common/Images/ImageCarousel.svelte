<script lang="ts">
    import type { IMap } from 'src/Types';
    import { slide } from 'svelte/transition';

    export let maps: IMap[];
    export let selectedIndex = 0;

    const onChangePhoto = (index: number) => {
    	selectedIndex = index;
    };

</script>

{#if maps.length !== 0}
<div class="w-full select-none">
    {#key selectedIndex}
        <div transition:slide={{duration: 300}}>
            {#if maps[selectedIndex] && maps[selectedIndex].thumbnail}
                <a href={maps[selectedIndex].imageUrl} target="_blank" rel="noreferrer" class="w-fit">
                    <img class="h-48 w-fit mx-auto hover:brightness-105 cursor-pointer" src={maps[selectedIndex].thumbnail} alt="" />
                </a>
                <div class="flex justify-center my-2">
                    <div class="mx-2">Year: {maps[selectedIndex].publishYear}</div>
                    <div class="mx-2">Dpi: {maps[selectedIndex].dpi}</div> 
                </div>
            {/if}
        </div> 
    {/key}
    <div class="flex my-4">
        {#if maps.length > 1}
            <div class="flex flex-wrap mx-auto">
                {#each maps as map, index}
                    {#if map.thumbnail}
                    <div class={maps[selectedIndex].id === map.id ? 'brightness-100' : 'brightness-[.65] cursor-pointer hover:brightness-[.80]'}>
                        <div 
                            class="min-w-1"
                            on:click={() => onChangePhoto(index)}
                            on:keypress={() => onChangePhoto(index)}
                        >
                            <img id={map.id} class="h-[64px] mx-2" src={map.thumbnail} alt="" />
                            <div class="flex mx-2">
                                <div class="my-2 text-xs mx-auto">{map.publishYear}</div> 
                            </div>
                        </div> 
                    </div>              
                    {/if}
                {/each}
            </div>
        {/if}
    </div>
</div>       
{/if}