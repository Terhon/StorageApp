import { useState, useEffect } from "react";
import { getItemTypes } from "../../../api/itemTypes";

export default function AddStorageItemForm({ onSubmit, onCancel }) {
    const [form, setForm] = useState({
        amount: "",
        acquisitionDate: "",
        itemTypeId: ""
    });

    const [itemTypes, setItemTypes] = useState([]);

    useEffect(() => {
        getItemTypes().then(res => setItemTypes(res.data));
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(form);
        setForm({ amount: "", acquisitionDate: "", itemTypeId: "" });
    };

    return (
        <form onSubmit={handleSubmit} className="flex flex-col gap-3">
            <input
                type="number"
                placeholder="Amount"
                value={form.amount}
                onChange={(e) => setForm({ ...form, amount: e.target.value })}
                className="input input-bordered w-full"
                required
            />

            <input
                type="date"
                value={form.acquisitionDate}
                onChange={(e) => setForm({ ...form, acquisitionDate: e.target.value })}
                className="input input-bordered w-full"
                required
            />

            <select
                value={form.itemTypeId}
                onChange={(e) => setForm({ ...form, itemTypeId: e.target.value })}
                className="select select-bordered w-full"
                required
            >
                <option value="">Select Item Type</option>
                {itemTypes.map(type => (
                    <option key={type.id} value={type.id}>
                        {type.name} ({type.unit})
                    </option>
                ))}
            </select>

            <div className="flex gap-2 justify-end">
                <button type="submit" className="btn btn-primary">Confirm</button>
                <button type="button" className="btn btn-error" onClick={onCancel}>Cancel</button>
            </div>
        </form>
    );
}
