import { render, screen, fireEvent } from "@testing-library/react";
import { describe, test, expect, vi } from "vitest";
import { ItemTypesForm } from "./ItemTypesForm";

describe("ItemTypesForm", () => {
    test("submits form with name and unit", () => {
        const handleCreate = vi.fn();
        render(<ItemTypesForm onCreate={handleCreate} />);

        fireEvent.change(screen.getByPlaceholderText(/Name/i), {
            target: { value: "Box", name: "name" },
        });
        fireEvent.change(screen.getByPlaceholderText(/Unit/i), {
            target: { value: "pcs", name: "unit" },
        });

        fireEvent.click(screen.getByText(/Add/i));

        expect(handleCreate).toHaveBeenCalledWith({ name: "Box", unit: "pcs" });
    });

    test("requires both fields", () => {
        const handleCreate = vi.fn();
        render(<ItemTypesForm onCreate={handleCreate} />);

        fireEvent.click(screen.getByText(/Add/i));

        expect(handleCreate).not.toHaveBeenCalled();
    });
});
