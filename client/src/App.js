import React, { useEffect, useState } from 'react';
import { getItems } from './api/itemTypes';

function App() {
  const [items, setItems] = useState([]);

  useEffect(() => {
    getItems().then(res => setItems(res.data));
  }, []);

    const handleDelete = async (id) => {
        const res = await fetch(`${API_BASE}/${id}`, { method: "DELETE" });
        if (res.ok) {
            setItems(items.filter((i) => i.id !== id));
        } else {
            alert("Failed to delete item");
        }
    };
  
  return (
      <div>
        <h1>Items</h1>
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
