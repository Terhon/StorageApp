import axios from "axios";

export const API = axios.create({ baseURL: 'https://localhost:44327/api' });

API.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("jwt");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

API.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            console.warn("Unauthorized, redirecting to login...");
            localStorage.removeItem("jwt");
            window.location.href = "/login";
        }
        return Promise.reject(error);
    }
);
