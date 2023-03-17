<script lang="ts">
    import MapView from '@arcgis/core/views/MapView';
    import Map from '@arcgis/core/Map';
    import GraphicsLayer from '@arcgis/core/layers/GraphicsLayer';
    import '@arcgis/core/assets/esri/themes/light/main.css';
    import type { IRegion } from 'src/Types';
    import { MapStore } from 'src/Components';

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
    	//binds div element to map so it will render in this div
    	view.container = containerRef;

    	//Registers mapView to app`s state management system
    	MapStore.setMapView(view);

    	//Registers default zoom to app`s state management system
    	MapStore.updateZoom(view.zoom);

    	addLayersToArcgis();

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
				if (!response.results.length) return;
				const viewHit = response.results.filter((result: any) => {
    					return result.graphic.layer === mapyLayer || result.graphic.layer === igrekLayer;
    				})[0];
    				const graphicHit = viewHit as __esri.GraphicHit;
    				if (!graphicHit || !graphicHit.graphic) return;

    				const region = graphicHit.graphic.attributes.region as IRegion | undefined;

    				//Handle chosen region
    				MapStore.choseRegion(region);
    		});
    	});
    };
</script>

<div class="h-screen" use:createMap />
