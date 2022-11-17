import SimpleFillSymbol from '@arcgis/core/symbols/SimpleFillSymbol';

export const regionWithMaps = new SimpleFillSymbol({
	color: [0, 0, 0, 0.1],
	outline: {
		color: [144, 238, 144],
		width: 0.5
	}
});

export const regionWithNoMaps = new SimpleFillSymbol({
	color: [0, 0, 0, 0.05],
	outline: {
		color: [150, 150, 150],
		width: 0.5
	}
});

export const selectedRegion = new SimpleFillSymbol({
	color: [0, 0, 0, 0.05],
	outline: {
		color: [255, 0, 0],
		width: 1
	}
});