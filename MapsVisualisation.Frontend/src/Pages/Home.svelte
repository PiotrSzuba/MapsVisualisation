<script lang="ts">
  import { onMount } from 'svelte';
  import { GetAllRegions } from 'src/Services';
  import { Map, Circle, MapStore } from 'src/Components';

  onMount(async () => {
    const data = await GetAllRegions();
    if (!data) return;
    MapStore.setRegions(data);
  });

</script>

{#if $MapStore.regions.length !== 0}
  <Map regions={$MapStore.regions} />
{/if}
{#if $MapStore.regions.length === 0}
  <div class="flex h-screen">
    <div class="mx-auto my-auto h-16 w-16">
      <Circle />
    </div>
  </div>
{/if}


