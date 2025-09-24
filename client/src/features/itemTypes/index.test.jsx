import { vi } from "vitest";
import { render, screen, fireEvent } from "@testing-library/react";
import { ItemTypesFeature } from "./index.jsx";
import * as hookModule from "./hooks/useItemTypes";

vi.mock("./hooks/useItemTypes");

describe("ItemTypesFeature", () => {
    beforeEach(() => {
        hookModule.useItemTypes.mockReturnValue({
            items: [{ id: "1", name: "Flour", unit: "g" }],
            create: vi.fn(),
            remove: vi.fn(),
        });
    });
    
    it("renders heading, form, and list", () => {
        render(<ItemTypesFeature />);

        expect(
            screen.getByRole("heading", { name: /item types/i })
        ).toBeInTheDocument();

        expect(screen.getByPlaceholderText("Name")).toBeInTheDocument();
        expect(screen.getByPlaceholderText("Unit")).toBeInTheDocument();

        expect(screen.getByText(/Flour/i)).toBeInTheDocument();
        expect(screen.getByText(/g/i)).toBeInTheDocument();
    });

    it("calls remove when delete button clicked", async () => {
        render(<ItemTypesFeature />);
        fireEvent.click(screen.getByText(/delete/i));
        expect(hookModule.useItemTypes().remove).toHaveBeenCalledWith("1");
    });

    it("calls create when form submitted", () => {
        render(<ItemTypesFeature />);

        fireEvent.change(screen.getByPlaceholderText("Name"), {
            target: { value: "Salt" },
        });
        fireEvent.change(screen.getByPlaceholderText("Unit"), {
            target: { value: "g" },
        });

        fireEvent.click(screen.getByRole("button", { name: /add/i }));

        expect(hookModule.useItemTypes().create).toHaveBeenCalledWith({
            name: "Salt",
            unit: "g",
        });
    });
});
