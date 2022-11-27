import type { AxiosResponse } from 'axios';
import { apiService } from 'src/Services';
import type { IRegion } from 'src/Types';

export const GetAllRegions = async (): Promise<IRegion[] | undefined> => {
	return await apiService.get('/regions')
		.then(response => response.data)
		.catch((error: AxiosResponse) => Promise.reject(error));
};
