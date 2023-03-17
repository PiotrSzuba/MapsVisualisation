<script lang="ts">
    import { MapStore, Switch } from 'src/Components';
    import { Link } from 'src/Router';
    import type { IRegion } from 'src/Types';
    import { RegionType } from 'src/Types';
    import IoIosClose from 'svelte-icons/io/IoIosClose.svelte';

    $: mode = $MapStore.mode;
    let search = '';
    let filteredRegions: IRegion[] = [];

    const tryFilterByMapsterMesstischblatt = (regions: IRegion[], searchQuery: string): boolean  => {
    	if (!new RegExp(/(^-?\d*$)/g).test(searchQuery)) return false;

    	filteredRegions = regions
    		.filter(r => r.type === RegionType.MapyAmzp)
    		.filter((r) => r.regionIdentity
    			.substring(0, searchQuery.length)
    			.toLowerCase()
    			.includes(searchQuery))
    		.sort((a, b) => {
    			if (a.regionIdentity.length > b.regionIdentity.length) return 1;
    			if (a.regionIdentity.length < b.regionIdentity.length) return -1;
    			if (a.regionIdentity > b.regionIdentity) return 1;
    			if (a.regionIdentity < b.regionIdentity) return -1;
    			return 0;
    		})
    		.slice(0, 10);

    	return true;
    };

    const getMatchedNumber = (normalizedSearchQuery: string) => {
    	const matchedNumbers = normalizedSearchQuery.match(new RegExp(/s[0-9]+/g));
    	if (matchedNumbers && matchedNumbers.length > 0) return matchedNumbers[0];
    	return '';
    };

    const tryFilterBySlup = (regions: IRegion[], searchQuery: string): boolean  => {
    	const normalizedSearchQuery = searchQuery.replace(' ', '');
    	if (!new RegExp(/s[0-9]+/g).test(normalizedSearchQuery)) return false;

    	filteredRegions = regions
    		.filter(r => r.type === RegionType.IgrekAmzp)
    		.filter((r) => r.regionIdentity
    			.split(' ')[1]
    			.toLowerCase()
    			.includes(normalizedSearchQuery.match(new RegExp(/s[0-9]+/g)) ? getMatchedNumber(normalizedSearchQuery) : ''))
    		.sort((a, b) => {
    			if (a.regionIdentity.split(' ')[0] > b.regionIdentity.split(' ')[0]) return 1;
    			if (a.regionIdentity.split(' ')[0] < b.regionIdentity.split(' ')[0]) return -1;
    			return 0;
    		})
    		.slice(0, 10);

    	const indexOfP = normalizedSearchQuery.indexOf('p');

    	if (indexOfP === -1 && normalizedSearchQuery.length > 3) {
    		filteredRegions = [];
    		return true;
    	}

    	if (indexOfP === -1) return true;

    	const sub = normalizedSearchQuery.substring(indexOfP);

    	filteredRegions = filteredRegions                
    		.filter((r) => r.regionIdentity
    			.replace(' ', '')
    			.substring(0, sub.length)
    			.toLowerCase()
    			.includes(sub))
    		.sort((a, b) => {
    			if (a.regionIdentity > b.regionIdentity) return 1;
    			if (a.regionIdentity < b.regionIdentity) return -1;
    			return 0;
    		})
    		.slice(0, 10);

    	return true;
    };

    const tryFilterByPas = (regions: IRegion[], searchQuery: string): boolean => {
    	const normalizedSearchQuery = searchQuery.replace(' ', '');
    	if (!new RegExp(/p[0-9]+/g).test(normalizedSearchQuery)) return false;

    	filteredRegions = regions
    		.filter(r => r.type === RegionType.IgrekAmzp)
    		.filter((r) => r.regionIdentity
    			.replace(' ', '')
    			.substring(0, normalizedSearchQuery.length)
    			.toLowerCase()
    			.includes(normalizedSearchQuery))
    		.sort((a, b) => {
    			if (a.regionIdentity > b.regionIdentity) return 1;
    			if (a.regionIdentity < b.regionIdentity) return -1;
    			return 0;
    		})
    		.slice(0, 10);
    	return true;
    };

    const filterByRegionNames = (regions: IRegion[], searchQuery: string) => {
    	filteredRegions = regions
    		.filter((r) => 
    			r.regionName1
    				.substring(0, searchQuery.length)
    				.toLowerCase()
    				.includes(searchQuery.toLowerCase()) || ( r.regionName2 && 
                r.regionName2.substring(0, search.length)
                    .toLowerCase()
                    .includes(searchQuery.toLowerCase())) || ( r.regionName3 && 
                r.regionName3
                    .substring(0, searchQuery.length)
                    .toLowerCase()
                    .includes(searchQuery.toLowerCase())))
    		.slice(0, 10)
    		.sort();
    };

    const onSearch = (event: Event) => {
    	const target = event.target as HTMLInputElement; 
    	if (target === null) return;

    	search = target.value.toLowerCase();

    	const regions = $MapStore.regions;

    	if (search.length === 0) {
    		filteredRegions = [];
    		return;
    	}

    	if (tryFilterByMapsterMesstischblatt(regions, search)) return;

    	if (tryFilterBySlup(regions, search)) return;

    	if (tryFilterByPas(regions, search)) return;

    	filterByRegionNames(regions, search);
    };

    const resetSearch = () => {
    	search = '';
    	filteredRegions = [];
    };

    const handleSearchResultClick = (region: IRegion) => {
    	MapStore.setSearchResultsVisible(false);
    	MapStore.findRegion(region);
    };

</script>

<div class="fixed flex flex-nowrap w-[25%] z-50 left-4 top-4">
    <Link to="/" class="my-auto cursor-pointer select-none text-black-300 font-bold">
        Maps visualization
    </Link>
</div>
<div class="fixed flex w-[26%] z-10 left-[37%] top-2">
    <div class="flex my-auto w-[100%] mx-auto h-[40px]">
        <input 
            class={$MapStore.searchResultsVisible && search ? 'input-focused' : 'input'}
            type="text" 
            placeholder="Search for region"
            bind:value={search}
            on:input={(e) => onSearch(e)}
            on:focus={() => { MapStore.setSearchResultsVisible(true); }}
        />
        {#if search.length !== 0}
            <div 
                class="absolute my-auto h-8 w-8 ml-auto cursor-pointer z-50 text-white-700 right-2 
                top-[21%] hover:text-white-100"
                on:click={() => resetSearch()}
                on:keypress={() => resetSearch()}
            >
                <IoIosClose />
            </div> 
        {/if}
        {#if $MapStore.searchResultsVisible && filteredRegions.length !== 0 }
            <div 
                class="absolute z-40 top-[48px] bg-black-900/75 w-[100%] text-white-100 rounded-b-2xl border-t-4 border-black-900"
            >        
                {#each filteredRegions as region, index}
                    <div class={index === filteredRegions.length - 1 ? 'search-row hover:bg-black-200 rounded-b-2xl' : 'search-row hover:bg-black-200'}
                        on:click={() => handleSearchResultClick(region)}
                        on:keypress={() => handleSearchResultClick(region)}
                    >
                        <div class="mr-2">{region.regionName1}</div>
                        {#if region.regionName2 && region.regionName2 !== '...'}
                            <div class="mr-2">{region.regionName2}</div>                    
                        {/if}
                        {#if region.regionName3}
                            <div class="mr-2">{region.regionName3}</div>                    
                        {/if}
                        <div class="mr-2">{region.regionIdentity}</div>
                        {#if region.maps}
                            <div>Maps: {region.maps.length}</div>                   
                        {/if}
                    </div>
                {/each}

            </div>       
        {/if}
        {#if $MapStore.searchResultsVisible && filteredRegions.length === 0 && search.length !== 0}
        <div class="absolute z-40 top-[48px] bg-black-900/75 w-[100%] text-red-500 rounded-b-2xl border-t-4 border-black-900">
            <div class="flex select-none px-4 py-1 flex-wrap">
                No results
            </div>
        </div>
        {/if}
    </div>
</div>
<div class="fixed flex font-bold z-10 right-[1%] top-[1.1rem]">
    <div class="flex mx-auto">
        <div class="my-auto">Mapy Amzp</div>
        <div class="my-auto">
            <Switch bind:isChecked={mode} onChange={() => MapStore.switchLayers()} class="mx-5"/>
        </div>
        <div class="my-auto">Igrek Amzp</div>
    </div>
</div>

<style>
    .input {
        @apply px-4 py-6 w-full rounded-2xl transition duration-300 text-white-500 bg-black-900/75;
    }
    .input-focused {
        @apply px-4 py-6 w-full rounded-2xl transition duration-300 text-white-500 bg-black-900/75 rounded-b-none;
    }
    .search-row {
        @apply flex select-none px-4 py-1 cursor-pointer flex-wrap;
    }
</style>