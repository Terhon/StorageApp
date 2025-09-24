import { useState, useEffect } from 'react';
import * as api from '../../../api/storageItems';

export function useStorageItems() {
    const [items, setItems] = useState([]);

    useEffect(() => {
        api.getStorageItems().then(res => setItems(res.data ?? []));
    }, []);

    const reload = () => api.getStorageItems().then(res => setItems(res.data ?? []));

    const create = async (data) => {
        const res = await api.createStorageItem(data);
        if (res.status === 201)
        {
            await reload();
        } else {
            alert("Failed to add item");
        }
    };

    const patch = async (data) => {
        const res = await api.patchStorageItem(data);
        if (res.status === 201)
        {
            await reload();
        } else {
            alert("Failed to patch item");
        }
    };
    
    const remove = async (id) => {
        const res = await api.deleteStorageItem(id);
        if (res.status === 204) {
            await reload();
        } else {
            alert("Failed to delete item");
        }
    };

    return { items, create, patch, remove };
}
