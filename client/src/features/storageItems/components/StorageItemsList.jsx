import React from 'react';

export function StorageItemsList({ storageItems = [], onDelete }) {
    if (storageItems.length === 0) {
        return <p>Loading...</p>;
    }

    return (
        <ul className="space-y-3">
            {storageItems.map(i => (
                <li key={i.id}>
                    <span className="font-semibold">{i.amount}</span>
                    <span className="text-sm text-gray-500 ml-2">[{i.itemType.unit}]</span>
                    <span className="font-bold ml-2">{i.itemType.name}</span>
                    
                    <button className="btn btn-error btn-sm" onClick={() => onDelete(i.id)}>Delete</button>
                </li>
            ))}
        </ul>
    );
}
