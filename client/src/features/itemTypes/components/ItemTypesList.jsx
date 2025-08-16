import React from 'react';

export function ItemTypesList({ itemsTypes = [], onDelete }) {
    if (itemsTypes.length === 0) {
        return <p>Loading...</p>;
    }
    
    return (
        <ul className="space-y-3"> 
            {itemsTypes.map(i => (
                <li key={i.id}>
                    <span className="font-semibold">{i.name}</span>
                    <span className="text-sm text-gray-500 ml-2">[{i.unit}]</span>
                    <button className="btn btn-error btn-sm" onClick={() => onDelete(i.id)}>Delete</button>
                </li>
            ))}
        </ul>
    );
}
