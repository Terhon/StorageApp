import axios from 'axios';

const API = axios.create({ baseURL: 'https://localhost:44327/api' });

export const getItems = () => API.get('/itemType');
export const createItem = (item) => API.post('/itemType', item);
export const deleteItem = (id) => API.delete(`/itemType/${id}`);
