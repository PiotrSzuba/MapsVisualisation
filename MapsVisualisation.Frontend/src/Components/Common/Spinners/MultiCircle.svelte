<script lang="ts">
    export let outerColor = '#FF3E00';
    export let middleColor = '#40B3FF';
    export let innerColor = '#676778';
</script>

<div style="border-color: transparent; border-right-color: {outerColor}; animation: spin 1.5s linear infinite;" class="h-full w-full rounded-full border-[3px] p-1">
    <div style="border-color: transparent; border-right-color: {middleColor}; animation: spin 5s linear infinite;" class="h-full w-full rounded-full border-[3px] p-1">
        <div style="border-color: transparent; border-right-color: {innerColor}; animation: spin 10s linear infinite;" class="h-full w-full rounded-full border-[3px]">
            <span class="hidden">Loading...</span>
        </div>
    </div>
</div>