import React, { useState } from 'react';

export function ItemTypesForm({ onCreate }) {
    const [form, setForm] = useState({ name: "", unit: "" });

    const handleChange = e => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    return (
        <form className="flex flex-col gap-3 mb-5" onSubmit={e => { e.preventDefault(); onCreate(form); }}>
            <input type="text" name="name" value={form.name} onChange={handleChange} placeholder="Name" required />
            <input type="text" name="unit" value={form.unit} onChange={handleChange} placeholder="Unit" required />
            <button type="submit" className="btn btn-primary w-full">Add</button>
        </form>
    );
}
