<script lang="ts">
    import MapView from '@arcgis/core/views/MapView';
    import Map from '@arcgis/core/Map';
    import EsriConfig from '@arcgis/core/config';
    import Graphic from '@arcgis/core/Graphic';
    import GraphicsLayer from '@arcgis/core/layers/GraphicsLayer';
    import Polygon from '@arcgis/core/geometry/Polygon';
    import TextSymbol from '@arcgis/core/symbols/TextSymbol';
    import '@arcgis/core/assets/esri/themes/light/main.css';
    import type { IRegion } from 'src/Types';
    import { RegionType } from 'src/Types';
    import { MapStore, regionWithMaps, regionWithNoMaps } from 'src/Components';

    export let regions: IRegion[];

    const mapyLayer = new GraphicsLayer();
    const mapyTextLayer = new GraphicsLayer();
    const mapySmTextLayer = new GraphicsLayer();
    const mapyXsTextLayer = new GraphicsLayer();
    const igrekLayer = new GraphicsLayer();
    const igrekTextLayer = new GraphicsLayer();
    const igrekSmTextLater = new GraphicsLayer();
    const igrekXsTextLayer = new GraphicsLayer();

    const map = new Map({ basemap: 'osm' });
    const view = new MapView({
    	map: map,
    	zoom: 5,
    	center: [21.017532, 52.237049], // longitude, latitude
    });

    const addLayersToArcgis = () => {
    	map.add(mapyLayer);
    	map.add(igrekLayer);
    	map.add(mapyTextLayer);
    	map.add(igrekTextLayer);
    	map.add(mapySmTextLayer);
    	map.add(igrekSmTextLater);
    	map.add(mapyXsTextLayer);
    	map.add(igrekXsTextLayer);
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

    const hideUnusedLayers = () => {
    	mapyTextLayer.visible = false;
    	mapySmTextLayer.visible = false;
    	mapyXsTextLayer.visible = false;
    	igrekLayer.visible = false;
    	igrekTextLayer.visible = false;
    	igrekSmTextLater.visible = false;
    	igrekXsTextLayer.visible = false;
    };

    const addLayersToAppStateManagement = () => {
    	//Making sure app`s state management system is clean
    	MapStore.reset();

    	MapStore.addLayer(mapyLayer);
		MapStore.addLayer(mapyTextLayer);
		MapStore.addLayer(mapySmTextLayer);
		MapStore.addLayer(mapyXsTextLayer);
    	MapStore.addLayer(igrekLayer);
    	MapStore.addLayer(igrekTextLayer);
    	MapStore.addLayer(igrekSmTextLater);
    	MapStore.addLayer(igrekXsTextLayer);
    };

    const createMap = (containerRef: HTMLDivElement) => {
    	//EsriConfig.apiKey = 'AAPK06c14e32e9604c74baaa71b27da9f966o3lN5PcCGm9bdqXw1cYRns1hBIHDpNbr80YtVXMojeMOQ_lC4HMNQ04VLUBMzQBs';

    	//binds div element to map so it will render in this div
    	view.container = containerRef;

    	//Registers mapView to app`s state management system
    	MapStore.setMapView(view);

    	//Registers default zoom to app`s state management system
    	MapStore.updateZoom(view.zoom);

    	addLayersToArcgis();

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
    			mapyTextLayer.add(textGraphic);
    			mapySmTextLayer.add(smTextGraphic);
    			mapyXsTextLayer.add(xsTextGraphic);
    			mapyLayer.add(regionGraphic);
    		} else if (region.type === RegionType.IgrekAmzp) {
    			igrekTextLayer.add(textGraphic);
    			igrekSmTextLater.add(smTextGraphic);
    			igrekXsTextLayer.add(xsTextGraphic);
    			igrekLayer.add(regionGraphic);
    		}
    	});

    	//By default all added layers are visible which looks chaotic
    	hideUnusedLayers();

    	//Registers layers to app`s state management system
    	addLayersToAppStateManagement();

    	//Executes when zoom changes
    	view.watch('zoom', (zoom: number) => {
    		MapStore.updateZoom(zoom);
    	});

    	//executes when clicked on map
    	view.on('click', (event: any) => {
    		const screenPoint = {
    			x: event.x,
    			y: event.y
    		};
            
    		//finds graphic element  
    		// eslint-disable-next-line @typescript-eslint/no-floating-promises
    		view.hitTest(screenPoint).then((response) => {
    			if (response.results.length) {
    				const viewHit = response.results.filter((result: any) => {
    					return result.graphic.layer === mapyLayer || result.graphic.layer === igrekLayer;
    				})[0];
    				const graphicHit = viewHit as __esri.GraphicHit;
    				if (!graphicHit || !graphicHit.graphic) return;

    				const region = graphicHit.graphic.attributes.region as IRegion | undefined;

    				//Handle chosen region
    				MapStore.choseRegion(region);
    			}
    		});
    	});
    };
</script>

{#if regions.length !== 0}
    <div class="h-screen" use:createMap />
{/if}
{#if regions.length === 0}
    <div>Loading</div>
{/if}
