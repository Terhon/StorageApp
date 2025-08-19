import React from 'react';

export function StorageItemsList({storageItems = [], onDelete}) {
    if (storageItems.length === 0) {
        return <p>Loading...</p>;
    }

    return (
        <ul className="space-y-3">
            {storageItems.map(item => (
                <li className="flex justify-between items-center p-3 bg-base-200 rounded-lg shadow" key={item.id}>
                    <span className="font-semibold text-lg">{item.itemType.name}</span>
                     <span className="flex flex-col">
                        <span>
                          <span className="badge badge-outline mr-2">
                            {item.amount} {item.itemType.unit}
                          </span>
                          <span className="text-sm text-gray-500">
                            Acquired: {new Date(item.acquisitionDate).toLocaleDateString()}
                          </span>
                        </span>
                      </span>
                    <button className="btn btn-error btn-sm" onClick={() => onDelete(item.id)}>Delete</button>
                </li>
            ))}
        </ul>
    );
}
