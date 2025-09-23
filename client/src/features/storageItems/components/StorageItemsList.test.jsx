import { render, screen, fireEvent } from "@testing-library/react";
import { StorageItemsList } from "./StorageItemsList";
import { vi } from "vitest";

describe("StorageItemsList", () => {
    const mockOnDelete = vi.fn();

    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("renders loading text when no items", () => {
        render(<StorageItemsList storageItems={[]} onDelete={mockOnDelete} />);
        expect(screen.getByText("Loading...")).toBeInTheDocument();
    });

    it("renders storage items", () => {
        const items = [
            {
                id: "1",
                amount: 3,
                acquisitionDate: "2025-09-01",
                itemType: { name: "Rice", unit: "kg" },
            },
        ];

        render(<StorageItemsList storageItems={items} onDelete={mockOnDelete} />);

        expect(screen.getByText("Rice")).toBeInTheDocument();
        expect(screen.getByText(/3 kg/)).toBeInTheDocument();
        expect(screen.getByText(/Acquired:/)).toBeInTheDocument();
    });

    it("calls onDelete when delete button is clicked", () => {
        const items = [
            {
                id: "2",
                amount: 10,
                acquisitionDate: "2025-09-10",
                itemType: { name: "Salt", unit: "g" },
            },
        ];

        render(<StorageItemsList storageItems={items} onDelete={mockOnDelete} />);
        fireEvent.click(screen.getByRole("button", { name: /delete/i }));

        expect(mockOnDelete).toHaveBeenCalledWith("2");
    });
});
