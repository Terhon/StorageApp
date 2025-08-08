import React from 'react';

export function ItemTypesList({ itemsTypes = [], onDelete }) {
    if (itemsTypes.length === 0) {
        return <p>Loading...</p>;
    }
    
    return (
        <ul>
            {itemsTypes.map(i => (
                <li key={i.id}>
                    {i.name} [{i.unit}]
                    <button onClick={() => onDelete(i.id)}>Delete</button>
                </li>
            ))}
        </ul>
    );
}
