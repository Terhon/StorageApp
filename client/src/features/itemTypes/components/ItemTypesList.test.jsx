import { render, screen, fireEvent } from "@testing-library/react";
import { vi } from "vitest";
import { ItemTypesList } from "./ItemTypesList";

describe("ItemTypesList", () => {
    test("shows Loading... when no items", () => {
        render(<ItemTypesList itemsTypes={[]} onDelete={vi.fn()} />);
        expect(screen.getByText(/Loading/i)).toBeInTheDocument();
    });

    test("renders list of item types", () => {
        const items = [
            { id: 1, name: "Apples", unit: "kg" },
            { id: 2, name: "Milk", unit: "liters" },
        ];

        render(<ItemTypesList itemsTypes={items} onDelete={vi.fn()} />);

        expect(screen.getByText("Apples")).toBeInTheDocument();
        expect(screen.getByText("[kg]")).toBeInTheDocument();
        expect(screen.getByText("Milk")).toBeInTheDocument();
        expect(screen.getByText("[liters]")).toBeInTheDocument();
    });

    test("calls onDelete when delete button is clicked", () => {
        const items = [{ id: 1, name: "Apples", unit: "kg" }];
        const onDelete = vi.fn();

        render(<ItemTypesList itemsTypes={items} onDelete={onDelete} />);

        fireEvent.click(screen.getByText("Delete"));

        expect(onDelete).toHaveBeenCalledTimes(1);
        expect(onDelete).toHaveBeenCalledWith(1);
    });
});
