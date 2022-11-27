import axios from 'axios';
import type { AxiosInstance } from 'axios';

//get from env
const baseUrl = 'https://localhost:7178'; 

const axiosInstance: AxiosInstance = axios.create({
	baseURL: `${baseUrl}`,
});

export const apiService = axiosInstance;
