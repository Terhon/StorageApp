import axios from 'axios';

const API = axios.create({ baseURL: 'https://localhost:44327/api' });

export const getItemTypes = () => API.get('/itemType');
export const createItemTypes = (item) => API.post('/itemType', item);
export const deleteItemTypes = (id) => API.delete(`/itemType/${id}`);
