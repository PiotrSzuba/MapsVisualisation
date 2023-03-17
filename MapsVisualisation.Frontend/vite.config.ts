import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import postcss from './postcss.config.cjs';
import tsconfigPaths from 'vite-tsconfig-paths';

export default defineConfig({
	resolve: {
		alias: {
			src: '/src',
		},
	},
	plugins: [svelte(), tsconfigPaths()],
	css:{
		postcss
	},
	build: {
		outDir: '../MapsVisualisation.Api/SvelteApp'
	}
});
