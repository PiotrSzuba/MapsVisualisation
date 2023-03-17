<script lang="ts">
  import { onMount, afterUpdate } from 'svelte';
  import { GetAllRegions } from 'src/Services';
  import { Map, MapStore } from 'src/Components';

  const asyncFun = async (): Promise<void> => {
    const data = await GetAllRegions();
    if (!data) return;
    MapStore.setRegions(data);
  }

  afterUpdate(async () => {
    setTimeout(async () => {
      asyncFun();
    }, 1);
  });

</script>

<Map />
