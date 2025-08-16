import React from "react";
import {ItemTypesList} from "./components/ItemTypesList";
import {ItemTypesForm} from "./components/ItemTypesForm";
import {useItemTypes} from "./hooks/useItemTypes";

export function ItemTypesFeature()
{
    const { items, create, remove} = useItemTypes();
    
    return (
        <div className="max-w-lg mx-auto mt-10 p-6 bg-base-200 rounded-xl shadow-lg">
            <h1 className="text-2xl font-bold mb-4 text-center">Item Types</h1>
            <ItemTypesForm onCreate={create}/>
            <ItemTypesList itemsTypes={items} onDelete={remove}/>
        </div>
    )
}