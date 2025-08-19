import React, {useState} from "react";
import {StorageItemsList} from "./components/StorageItemsList.jsx";
import {useStorageItems} from "./hooks/useStorageItems.jsx";
import AddStorageItemForm from "./components/AddStorageItemForm.jsx";
import Modal from "./components/Modal.jsx";

export function StorageItemsFeature() {
    const {items, create, patch, remove} = useStorageItems();
    const [showModal, setShowModal] = useState(false);
    
    return (
        <div className="max-w-lg mx-auto mt-10 p-6 bg-base-200 rounded-xl shadow-lg">
            <h1 className="text-2xl font-bold mb-4 text-center">Storage Items</h1>

            <button className="btn btn-primary mb-4" onClick={() => setShowModal(true)}>
                Add Storage Item
            </button>

            <StorageItemsList storageItems={items} onDelete={remove}/>

            <Modal
                title="Add New Storage Item"
                isOpen={showModal}
                onClose={() => setShowModal(false)}
            >
                <AddStorageItemForm
                    onSubmit={async (item) => {await create(item); setShowModal(false);}}
                    onCancel={() => setShowModal(false)}
                />
            </Modal>
        </div>
    )
}