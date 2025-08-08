import React from "react";
import {ItemTypesList} from "./components/ItemTypesList";
import {ItemTypesForm} from "./components/ItemTypesForm";
import {useItemTypes} from "./hooks/useItemTypes";

export function ItemTypesFeature()
{
    const { items, create, remove} = useItemTypes();
    
    return (
        <div>
            <h2>Item Types</h2>
            <ItemTypesForm onCreate={create} />
            <ItemTypesList itemsTypes={items} onDelete={remove} />
        </div>
    )
}