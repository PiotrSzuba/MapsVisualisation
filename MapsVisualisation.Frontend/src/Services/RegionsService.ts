import axios from 'axios';
import type { IRegion } from 'src/Types';

export const GetAllRegions = async (): Promise<IRegion[] | undefined> => {
	try {
		return await axios.get('https://localhost:7178/regions')
			.then(response => response.data);
	} catch (error) {
		console.error(error);
	}
	return undefined;
};
