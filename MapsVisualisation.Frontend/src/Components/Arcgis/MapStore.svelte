<script lang="ts" context="module">
	import { writable } from 'svelte/store';
	import type GraphicsLayer from '@arcgis/core/layers/GraphicsLayer';
	import MapView from '@arcgis/core/views/MapView';
	import type { IRegion } from 'src/Types';
	import { RegionType } from 'src/Types';
	import Point from '@arcgis/core/geometry/Point';
	import { regionWithMaps, regionWithNoMaps, selectedRegion } from 'src/Components';

	interface IMapStoreActions {
		subscribe: (this: void, run: any, invalidate?: any | undefined) => any;
		reset: () => void;
		addLayer: (graphicLayer: GraphicsLayer) => void;
		switchLayers: () => void;
		updateZoom: (zoom: number) => void;
		choseRegion: (region: IRegion | undefined) => void;
		handleMenuVisibility: () => void;
		setRegions: (regions: IRegion[]) => void;
		setMapView: (mapView: MapView) => void;
		findRegion: (region: IRegion | undefined) => void;
		setSearchResultsVisible: (visible: boolean) => void;
	}

	interface IMapStoreData {
		layers: GraphicsLayer[];
		mode: boolean;
		zoom: number;
		chosenRegion: IRegion | undefined;
		sideMenuVisible: boolean;
		regions: IRegion[];
		mapView: MapView | undefined;
		searchResultsVisible: boolean;
	}

	const maxLayers = 8;

	const defaultMapStore = { 
		layers: [], 
		mode: false, 
		zoom: 0, 
		chosenRegion: undefined,
		sideMenuVisible: false,
		regions: [],
		mapView: new MapView(),
		searchResultsVisible: false,
	};

  	const createMapStore = (): IMapStoreActions => {
  		const { subscribe, update, set } = writable<IMapStoreData>(defaultMapStore);

		const hideAllText = (mapStore: IMapStoreData) => {
			mapStore.layers[2].visible = false;
			mapStore.layers[3].visible = false;
			mapStore.layers[4].visible = false;
			mapStore.layers[5].visible = false;
			mapStore.layers[6].visible = false;
			mapStore.layers[7].visible = false;
		};

		const hideAllLayers = (mapStore: IMapStoreData) => {
			mapStore.layers[0].visible = false;
			mapStore.layers[1].visible = false;
			hideAllText(mapStore);
		};

		const handleTextLayers = (mapStore: IMapStoreData) => {
			if (mapStore.mode && mapStore.layers.length === maxLayers) {
				mapStore.layers[3].visible = mapStore.zoom >= 8;
				mapStore.layers[5].visible = mapStore.zoom === 7;
				mapStore.layers[7].visible = mapStore.zoom === 6;
			} else if (!mapStore.mode && mapStore.layers.length === maxLayers) {
				mapStore.layers[2].visible = mapStore.zoom >= 9;
				mapStore.layers[4].visible = mapStore.zoom === 8;
				mapStore.layers[6].visible = mapStore.zoom === 7;
			}
		};

		const handleSelectedRegion = (mapStore: IMapStoreData, layerIndex: number, region?: IRegion) => {
			for (let i = 0; i < 2; i++) {
				for (const graphic of mapStore.layers[i].graphics) {
					if (region && graphic.attributes.region.id === region.id && i === layerIndex) {
						graphic.symbol = selectedRegion; 
					} else if (!graphic.attributes.region.maps || graphic.attributes.region.maps.length === 0) {
						graphic.symbol = regionWithNoMaps;
					} else {
						graphic.symbol = regionWithMaps;
					}
				}
			}

			return mapStore;
		};

		const addLayer = (mapStore: IMapStoreData, newLayer: GraphicsLayer): IMapStoreData => {
			if (mapStore.layers.length === maxLayers ) return mapStore;
			mapStore.layers.push(newLayer);
			return mapStore;
		};

		const switchLayers = (mapStore: IMapStoreData): IMapStoreData => {
			if (maxLayers !== mapStore.layers.length) return mapStore;

			hideAllLayers(mapStore);

			if (mapStore.mode) { 
				mapStore.layers[0].visible = true;
				mapStore.mode = false;
			} else {
				mapStore.layers[1].visible = true;
				mapStore.mode = true;
			}

			handleTextLayers(mapStore);


			return mapStore;
		};

		const handleZoomChange = (mapStore: IMapStoreData, zoom: number): IMapStoreData  => {
			if (mapStore.layers.length === maxLayers) hideAllText(mapStore);

			mapStore.zoom = zoom;

			handleTextLayers(mapStore);

			return mapStore;
		};

		const setChosenRegion = (mapStore: IMapStoreData, region: IRegion | undefined) => {
			if (region === undefined) mapStore.sideMenuVisible = false;
			if (region !== undefined) mapStore.sideMenuVisible = true;
			if (region === undefined && mapStore.chosenRegion) mapStore = handleSelectedRegion(mapStore, 0);

			mapStore.chosenRegion = region;

			if (mapStore.mapView && region) {
				mapStore.mapView.center = new Point({
					latitude: (region.nwLat + region.swLat) / 2,
					longitude: (region.nwLong + region.neLong) / 2,
				});
				let zoom = -1;
				if (region.type === RegionType.IgrekAmzp && mapStore.zoom < 6){
					zoom = 7;
				} else if(region.type === RegionType.MapyAmzp && mapStore.zoom < 7){
					zoom = 8;
				}
				if (zoom != -1) {
					mapStore.mapView.zoom = zoom;
					handleZoomChange(mapStore, zoom);
				}

				const index = region.type === RegionType.MapyAmzp ? 0 : 1;
				mapStore = handleSelectedRegion(mapStore, index, region);
			}
			mapStore.searchResultsVisible = false;
			return mapStore;
		};

		const findRegion = (mapStore: IMapStoreData, region: IRegion | undefined) => {
			let layerChanged = false;
			if (region && mapStore.chosenRegion && region.type !== mapStore.chosenRegion.type) {
				layerChanged = true;
			}
			if (region && !mapStore.chosenRegion) {
				layerChanged = region.type === RegionType.IgrekAmzp && !mapStore.mode;
			}
			mapStore = setChosenRegion(mapStore, region);

			return layerChanged ? switchLayers(mapStore) : mapStore;
		};

		const handleMenuVisibility = (mapStore: IMapStoreData) => {
			mapStore.sideMenuVisible = !mapStore.sideMenuVisible;
			return mapStore;
		};

		const setRegions = (mapStore: IMapStoreData, regions: IRegion[]) => {
			mapStore.regions = regions;
			return mapStore;
		};

		const setMapView = (mapStore: IMapStoreData, mapView: MapView) => {
			mapStore.mapView = mapView;
			return mapStore;
		};

		const setSearchResultsVisible = (mapStore: IMapStoreData, visible: boolean) => {
			mapStore.searchResultsVisible = visible;
			return mapStore;
		};

		return {
			subscribe,
			reset: () => set(defaultMapStore),
			addLayer: (graphicLayer: GraphicsLayer) => update(mapStore => addLayer(mapStore, graphicLayer)),
			switchLayers: () => update(mapStore => switchLayers(mapStore)),
			updateZoom: (zoom: number) => update(mapStore => handleZoomChange(mapStore, zoom)),
			choseRegion: (region: IRegion | undefined) => update(mapStore => setChosenRegion(mapStore, region)),
			handleMenuVisibility: () => update(handleMenuVisibility),
			setRegions: (regions: IRegion[]) => update(mapStore => setRegions(mapStore, regions)),
			setMapView: (mapView: MapView) => update(mapStore => setMapView(mapStore, mapView)),
			findRegion: (region: IRegion | undefined) => update(mapStore => findRegion(mapStore, region)),
			setSearchResultsVisible: (visible: boolean) => update(mapStore => setSearchResultsVisible(mapStore, visible)),
		};
  };

  export const MapStore = createMapStore();
</script>