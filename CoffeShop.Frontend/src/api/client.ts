import axios from "axios";
import { getToken } from "../auth/storage";

export const api = axios.create({
    baseURL: "http://localhost:5137" //change this url to the server
})

api.interceptors.request.use((config) => {
    const token = getToken();
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
});