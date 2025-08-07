import React, {useEffect, useState} from 'react';
import {getItems, createItem, deleteItem} from './api/itemTypes';

function App() {
    const [items, setItems] = useState([]);
    const [newItem, setNewItem] = useState({ name: "", unit: "" });
    
    useEffect(() => {
        getItems().then(res => setItems(res.data));
    }, []);

    const handleAdd = async (e) => {
        e.preventDefault();
        if (!newItem.name || !newItem.unit) 
            return;
        
        const res = await createItem(newItem);
        console.log(res);
        if (res.status === 201) 
        {
            const get = await getItems();
            setItems(get.data);
        } else {
            alert("Failed to add item");
        }

    };

    const handleDelete = async (id) => {
        const res = await deleteItem(id);
        if (res.status === 204) {
            setItems(items.filter((i) => i.id !== id));
        } else {
            alert("Failed to delete item");
        }
    };

    return (
        <div>
            <h1>Items</h1>
            <form onSubmit={handleAdd} style={{marginBottom: "1rem"}}>
                <input
                    type="text"
                    placeholder="Name"
                    value={newItem.name}
                    onChange={(e) => setNewItem({...newItem, name: e.target.value})}
                    required
                />
                <input
                    type="text"
                    placeholder="Unit"
                    value={newItem.unit}
                    onChange={(e) => setNewItem({...newItem, unit: e.target.value})}
                    required
                />
                <button type="submit">Create</button>
            </form>
            <ul>
                {items.map(item => (
                    <li key={item.id}>
                        name: {item.name} - unit: {item.unit}
                        <button onClick={() => handleDelete(item.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default App;
