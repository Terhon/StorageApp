import { useState, useEffect } from 'react';
import * as api from '../../../api/itemTypes';

export function useItemTypes() {
    const [items, setItems] = useState([]);

    useEffect(() => {
        api.getItemTypes().then(res => setItems(res.data ?? [])).catch(err => console.log(err));
    }, []);

    const reload = () => api.getItemTypes().then(res => setItems(res.data ?? [])).catch(err => console.log(err));

    const create = async (data) => {
        const res = await api.createItemTypes(data).catch(err => console.log(err));
        if (res.status === 201)
        {
            reload();
        } else {
            alert("Failed to add item");
        }
    };

    const remove = async (id) => {
        const res = await api.deleteItemTypes(id).catch(err => console.log(err));
        if (res.status === 204) {
            reload();
        } else {
            alert("Failed to delete item");
        }
    };

    return { items, create, remove };
}
