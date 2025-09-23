import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import AddStorageItemForm from "./AddStorageItemForm";
import { vi } from "vitest";
import { getItemTypes } from "../../../api/itemTypes";

vi.mock("../../../api/itemTypes", () => ({
    getItemTypes: vi.fn(),
}));

describe("AddStorageItemForm", () => {
    const mockOnSubmit = vi.fn();
    const mockOnCancel = vi.fn();

    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("renders form inputs", async () => {
        getItemTypes.mockResolvedValueOnce({ data: [] });

        render(<AddStorageItemForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

        expect(screen.getByPlaceholderText("Amount")).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /confirm/i })).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /cancel/i })).toBeInTheDocument();
    });

    it("loads and displays item types", async () => {
        getItemTypes.mockResolvedValueOnce({
            data: [{ id: "1", name: "Sugar", unit: "kg" }],
        });

        render(<AddStorageItemForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

        await waitFor(() =>
            expect(screen.getByText(/Sugar \(kg\)/)).toBeInTheDocument()
        );
    });

    it("submits form with values", async () => {
        getItemTypes.mockResolvedValueOnce({
            data: [{ id: "1", name: "Flour", unit: "g" }],
        });

        render(<AddStorageItemForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

        expect(await screen.findByText(/Flour/)).toBeInTheDocument();
        
        fireEvent.change(screen.getByPlaceholderText("Amount"), {
            target: { value: "5" },
        });
        fireEvent.change(screen.getByLabelText(/acquisition date/i), {
            target: { value: "2025-09-18" },
        });
        fireEvent.change(screen.getByLabelText("ItemType"), { target: { value: "1" } });

        fireEvent.click(screen.getByRole("button", { name: /confirm/i }));

        await waitFor(() =>
            expect(mockOnSubmit).toHaveBeenCalledWith({
                amount: "5",
                acquisitionDate: "2025-09-18",
                itemTypeId: "1",
            })
        );
    });

    it("calls onCancel when Cancel button clicked", () => {
        getItemTypes.mockResolvedValueOnce({ data: [] });

        render(<AddStorageItemForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

        fireEvent.click(screen.getByRole("button", { name: /cancel/i }));
        expect(mockOnCancel).toHaveBeenCalled();
    });
});
