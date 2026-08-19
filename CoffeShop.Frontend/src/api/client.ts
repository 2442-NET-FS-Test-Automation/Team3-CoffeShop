import axios from "axios";
import { getToken } from "../auth/storage";

export const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL ??  "http://localhost:5011" //change this url to the server
})

api.interceptors.request.use((config) => {
    const token = getToken();
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
});