<script lang="ts" context="module">
	import { writable } from 'svelte/store';
	import type GraphicsLayer from '@arcgis/core/layers/GraphicsLayer';
	import MapView from '@arcgis/core/views/MapView';
	import type { IRegion } from 'src/Types';
	import { RegionType } from 'src/Types';
	import Point from '@arcgis/core/geometry/Point';
	import { regionWithMaps, regionWithNoMaps, selectedRegion } from 'src/Components';
    	import Graphic from '@arcgis/core/Graphic';
	import TextSymbol from '@arcgis/core/symbols/TextSymbol';
	import Polygon from '@arcgis/core/geometry/Polygon';

	enum LayerNames {
		mapyLayer,
		mapyTextLayer,
		mapySmTextLayer,
		mapyXsTextLayer,
		igrekLayer,
		igrekTextLayer,
		igrekSmTextLater,
		igrekXsTextLayer,
	}

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

	const hideAllText = (mapStore: IMapStoreData) => {
		mapStore.layers[LayerNames.mapyTextLayer].visible = false;
		mapStore.layers[LayerNames.mapySmTextLayer].visible = false;
		mapStore.layers[LayerNames.mapyXsTextLayer].visible = false;
		mapStore.layers[LayerNames.igrekTextLayer].visible = false;
		mapStore.layers[LayerNames.igrekSmTextLater].visible = false;
		mapStore.layers[LayerNames.igrekXsTextLayer].visible = false;
	};

	const hideAllLayers = (mapStore: IMapStoreData) => {
		mapStore.layers[LayerNames.mapyLayer].visible = false;
		mapStore.layers[LayerNames.igrekLayer].visible = false;
		hideAllText(mapStore);
	};

	const handleTextLayers = (mapStore: IMapStoreData) => {
		if (mapStore.mode && mapStore.layers.length === maxLayers) {
			mapStore.layers[LayerNames.igrekTextLayer].visible = mapStore.zoom >= 9;
			mapStore.layers[LayerNames.igrekSmTextLater].visible = mapStore.zoom === 8;
			mapStore.layers[LayerNames.igrekXsTextLayer].visible = mapStore.zoom === 7;
		} else if (!mapStore.mode && mapStore.layers.length === maxLayers) {
			mapStore.layers[LayerNames.mapyTextLayer].visible = mapStore.zoom >= 10;
			mapStore.layers[LayerNames.mapySmTextLayer].visible = mapStore.zoom === 9;
			mapStore.layers[LayerNames.mapyXsTextLayer].visible = mapStore.zoom === 8;
		}
	};

	const handleSelectedRegion = (mapStore: IMapStoreData, layerIndex: number, region?: IRegion) => {
		const mapLayers = mapStore.layers.filter((_value, index) => index === LayerNames.mapyLayer || index === LayerNames.igrekLayer);
		for (let i = 0; i < 2; i++) {
			for (const graphic of mapLayers[i].graphics) {
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
			mapStore.layers[LayerNames.mapyLayer].visible = true;
			mapStore.mode = false;
		} else {
			mapStore.layers[LayerNames.igrekLayer].visible = true;
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

	const getAllText = (region: IRegion) => {
		return region.regionIdentity +
                '\n' + region.regionName1 +
                '\n' + region.regionName2 +
                '\n' + region.regionName3;
	};

    const createNormalText = (region: IRegion) => {
    	return new TextSymbol({
    		text: getAllText(region),
    		yoffset: '6px',
    	});
    };

    const createSmallText = (region: IRegion) => {
    	return new TextSymbol({
    		text: getAllText(region),
    		yoffset: '6px',
    		font: {
    			size: 7,
    		}
    	});
    };

    const createExtraSmallText = (region: IRegion) => {
    	return new TextSymbol({
    		text: region.regionIdentity,
    		yoffset: '-3px',
    		font: {
    			size: 7,
    		}
    	});
    };

    const createRegionBox = (region: IRegion) => {
    	const yDiff = (region.nwLat - region.swLat) / 100;
    	const xDiff = (region.neLong - region.nwLong ) / 100;
    	return new Polygon({
    		rings: [[
    			[region.nwLong + xDiff, region.nwLat - yDiff],
    			[region.neLong - xDiff, region.neLat - yDiff],
    			[region.seLong - xDiff, region.seLat + yDiff],
    			[region.swLong + xDiff, region.swLat + yDiff],
    		]],
    	});
    };

    const createGraphic = (box: Polygon, content: __esri.SymbolProperties, attributes?: any) => {
    	return new Graphic({
    		geometry: box,
    		symbol: content,
    		attributes: attributes,
    	});
    };

	const addRegionsToLayers = (mapStore: IMapStoreData, regions: IRegion[]) => {
		regions.forEach(region => {
			const regionBoundingBox: Polygon = createRegionBox(region);

			const text: TextSymbol = createNormalText(region);

			const smText: TextSymbol  = createSmallText(region);

			const xsText: TextSymbol  = createExtraSmallText(region);

			const textGraphic: Graphic = createGraphic(regionBoundingBox, text);

			const smTextGraphic: Graphic = createGraphic(regionBoundingBox, smText);

			const xsTextGraphic: Graphic = createGraphic(regionBoundingBox, xsText);

			const regionContent = region.maps && region.maps.length !== 0 ? regionWithMaps : regionWithNoMaps;

			const regionGraphic: Graphic = createGraphic(regionBoundingBox, regionContent, { region });

			// adds graphics to layer
			if (region.type === RegionType.MapyAmzp){
				mapStore.layers[LayerNames.mapyTextLayer].add(textGraphic);
				mapStore.layers[LayerNames.mapySmTextLayer].add(smTextGraphic);
				mapStore.layers[LayerNames.mapyXsTextLayer].add(xsTextGraphic);
				mapStore.layers[LayerNames.mapyLayer].add(regionGraphic);
			} else if (region.type === RegionType.IgrekAmzp) {
				mapStore.layers[LayerNames.igrekTextLayer].add(textGraphic);
				mapStore.layers[LayerNames.igrekSmTextLater].add(smTextGraphic);
				mapStore.layers[LayerNames.igrekXsTextLayer].add(xsTextGraphic);
				mapStore.layers[LayerNames.igrekLayer].add(regionGraphic);
			}
		});

		return mapStore;
	}

	const setRegions = (mapStore: IMapStoreData, regions: IRegion[]) => {
		mapStore.regions = regions;
		console.log(regions[0]);
		return addRegionsToLayers(mapStore, regions);
	};

	const setMapView = (mapStore: IMapStoreData, mapView: MapView) => {
		mapStore.mapView = mapView;
		return mapStore;
	};

	const setSearchResultsVisible = (mapStore: IMapStoreData, visible: boolean) => {
		mapStore.searchResultsVisible = visible;
		return mapStore;
	};

	const reset = (mapStore: IMapStoreData) => {
		mapStore = defaultMapStore;
		return mapStore;
	};

  	const createMapStore = (): IMapStoreActions => {
  		const { subscribe, update } = writable<IMapStoreData>(defaultMapStore);
		
		return {
			subscribe,
			reset: () => update(mapStore => reset(mapStore)),
			addLayer: (graphicLayer: GraphicsLayer) => update(mapStore => addLayer(mapStore, graphicLayer)),
			switchLayers: () => update(mapStore => switchLayers(mapStore)),
			updateZoom: (zoom: number) => update(mapStore => handleZoomChange(mapStore, zoom)),
			choseRegion: async (region: IRegion | undefined) => update(mapStore => setChosenRegion(mapStore, region)),
			handleMenuVisibility: () => update(handleMenuVisibility),
			setRegions: (regions: IRegion[]) => update(mapStore => setRegions(mapStore, regions)),
			setMapView: (mapView: MapView) => update(mapStore => setMapView(mapStore, mapView)),
			findRegion: (region: IRegion | undefined) => update(mapStore => findRegion(mapStore, region)),
			setSearchResultsVisible: (visible: boolean) => update(mapStore => setSearchResultsVisible(mapStore, visible)),
		};
  };

  export const MapStore = createMapStore();
</script>