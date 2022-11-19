<script lang="ts">
    import { beforeUpdate } from 'svelte';
    import type { IMap, IRegion } from 'src/Types';
    import { MapStore, ImageCarousel } from 'src/Components';
    import IoIosArrowBack from 'svelte-icons/io/IoIosArrowBack.svelte';
    import IoIosArrowForward from 'svelte-icons/io/IoIosArrowForward.svelte';
    import IoIosClose from 'svelte-icons/io/IoIosClose.svelte';
    import { fly } from 'svelte/transition';

    let currentRegionId: string = "";
    let maps: IMap[];
    let mapIndex = 0;
    $: chosenRegion = $MapStore.chosenRegion as IRegion;
    $: sideMenuVisible = $MapStore.sideMenuVisible as boolean;

    //Resets side menu state when users: closes, hides, chooses new region
    beforeUpdate(() => {
        const region = $MapStore.chosenRegion;

        if (region && currentRegionId !== region.id) {
            mapIndex = 0;
            currentRegionId = region.id;
        }


        if (region && region.maps && region.maps.length !== 0) {
            maps = region.maps;
        }else{
            maps = [];
        }
    });
</script>

{#if chosenRegion !== undefined}
    {#key sideMenuVisible}
    {#if !sideMenuVisible}
        <div 
            class="absolute top-[46%] z-30 bg-neutral-900 rounded-r-lg h-[8%] cursor-pointer text-white-100 flex"                        
            on:click={() => {MapStore.handleMenuVisibility()}} 
            on:keypress={() => {MapStore.handleMenuVisibility()}}
            transition:fly={{x: -300, duration: 300}}
        >
            <div class="h-4 my-auto pr-2">
                <IoIosArrowForward />
            </div>
        </div>
    {/if}
    <div 
        class={sideMenuVisible ? "fixed h-full w-[25%] z-40" : "fixed h-full w-[25%] -left-[25%] z-40"}
        transition:fly={{x: -300, duration: 300}}
    >
        <div class="flex h-full">
            <div class="bg-black-800/95 h-full w-full pt-16 text-white-100 shadow-2xl px-4 overflow-y-auto pb-8 scrollbar">
                <div class="flex my-4">
                    <div class="my-auto font-semibold">
                        {chosenRegion.regionIdentity}
                    </div>
                    <div class="my-auto font-semibold mx-4">
                        {chosenRegion.regionName1}
                    </div>
                    <div 
                        class="my-auto h-8 w-8 ml-auto cursor-pointer text-white-700 hover:text-white-100"
                        on:click={() => {MapStore.choseRegion(undefined)}}
                        on:keypress={() => {MapStore.choseRegion(undefined)}}
                    >
                        <IoIosClose />
                    </div>
                </div>
                {#if maps.length !== 0}
                    <ImageCarousel maps={maps} bind:selectedIndex={mapIndex} />
                {/if}
                {#if chosenRegion.regionName2}
                    <div class="mt-4 mx-auto">Other names:</div>
                    <div class="ml-4">{chosenRegion.regionName2}</div>
                    <div class="ml-4">{chosenRegion.regionName3}</div>
                {/if}
                {#if chosenRegion.otherSources && chosenRegion.otherSources.length > 0}
                    <div class="mt-4">
                        Other sources:
                        {#each chosenRegion.otherSources as source }
                        <a href={source.url} target="_blank" rel="noreferrer" class="mt-4 hover:font-semibold">
                            <div class="flex cursor-pointer ml-4 w-fit">
                                <div>{source.name}:</div>
                                <div class="ml-2">{source.year}</div>
                            </div>
                        </a>
                    {/each}
                    </div>
                {/if}
            </div>
            <div class="flex h-full z-0 text-white-100">
                <div 
                    class="flex my-auto mx-auto bg-neutral-900 rounded-r-lg h-14 cursor-pointer" 
                    on:click={() => {MapStore.handleMenuVisibility()}} 
                    on:keypress={() => {MapStore.handleMenuVisibility()}}
                >
                    {#if sideMenuVisible}
                        <div class="my-auto pr-2 h-4">
                            <IoIosArrowBack />
                        </div>           
                    {/if}
                </div>
            </div>
        </div>
    </div>   
    {/key}
{/if}
