import React from "react";
import {StorageItemsList} from "./components/StorageItemsList.jsx";
import {useStorageItems} from "./hooks/useStorageItems.jsx";

export function StorageItemsFeature()
{
    const { items, create, patch, remove} = useStorageItems();

    return (
        <div className="max-w-lg mx-auto mt-10 p-6 bg-base-200 rounded-xl shadow-lg">
            <h1 className="text-2xl font-bold mb-4 text-center">Storage Items</h1>
            {/*<ItemTypesForm onCreate={create}/>*/}
            <StorageItemsList storageItems={items} onDelete={remove}/>
        </div>
    )
}