/** @type {import('tailwindcss').Config} */
module.exports = {
	mode: 'jit',
	theme: {
		extend: {      
			spacing:{
				'1px': '1px',
				'10%': '10%',
				'95%': '95%',
				'1%' : '1%',
				'18': '4.5rem',
				'-3/12': '-23.3%',
			},
			minWidth: {
				'1': '1rem',
			},
			animation: {
				'spin-slow': 'spin 5s linear infinite',
			},
			colors: {
				transparent: 'transparent',
				current: 'currentColor',
				black: {
					100: '#F3F6FB',
					200: '#999999',
					300: '#666666',
					400: '#333333',
					500: '#222222',
					600: '#111111',
					700: '#000000',
					800: '#000000',
					900: '#000000'
				},
				white: {
					100: '#ffffff',
					200: '#ffffff',
					300: '#ffffff',
					400: '#ffffff',
					500: '#ffffff',
					600: '#cccccc',
					700: '#999999',
					800: '#666666',
					900: '#333333'
				},
				red: {
					100: '#f8d3d7',
					200: '#f2a6af',
					300: '#eb7a87',
					400: '#e54d5f',
					500: '#de2137',
					600: '#b21a2c',
					700: '#851421',
					800: '#590d16',
					900: '#2c070b'
				},
				green: {
					100: '#d4edda',
					200: '#a9dcb5',
					300: '#7eca8f',
					400: '#53b96a',
					500: '#28a745',
					600: '#208637',
					700: '#186429',
					800: '#10431c',
					900: '#08210e'
				},
				blue: {
					100: '#ccccf5',
					200: '#9999eb',
					300: '#6666e1',
					400: '#3333d7',
					500: '#0000cd',
					600: '#0000a4',
					700: '#00007b',
					800: '#000052',
					900: '#000029'
				},
			},
		},
	},
	content: ['./index.html','./src/**/*.{svelte,js,ts}'],
	plugins: [],
};
  