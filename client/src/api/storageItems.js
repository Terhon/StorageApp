import axios from 'axios';

const API = axios.create({ baseURL: 'https://localhost:44327/api' });

export const getStorageItems = () => API.get('/StorageItem');
export const getStorageItem = (id) => API.get(`/StorageItem/${id}`);
export const createStorageItem = (item) => API.post('/StorageItem', item);
export const patchStorageItem = (item) => API.patch('/StorageItem', item);
export const deleteStorageItem = (id) => API.delete(`/StorageItem/${id}`);
