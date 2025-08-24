import {API} from "./common.jsx";

export const getItemTypes = () => API.get('/itemType');
export const createItemTypes = (item) => API.post('/itemType', item);
export const deleteItemTypes = (id) => API.delete(`/itemType/${id}`);
