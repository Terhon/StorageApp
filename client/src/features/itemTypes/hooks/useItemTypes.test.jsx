import { renderHook, act, waitFor } from "@testing-library/react";
import { describe, test, expect, vi, beforeEach } from "vitest";
import { useItemTypes } from "./useItemTypes";

vi.mock("../../../api/itemTypes", () => ({
    getItemTypes: vi.fn(),
    createItemTypes: vi.fn(),
    deleteItemTypes: vi.fn(),
}));

import * as api from "../../../api/itemTypes";

describe("useItemTypes", () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    test("loads initial items", async () => {
        api.getItemTypes.mockResolvedValueOnce({ data: [{ id: 1, name: "Box", unit: "pcs" }] });

        const { result } = renderHook(() => useItemTypes());

        await waitFor(() => {
            expect(result.current.items).toEqual([{ id: 1, name: "Box", unit: "pcs" }]);
        });
    });

    test("creates item and reloads", async () => {
        api.getItemTypes.mockResolvedValueOnce({ data: [] });
        api.createItemTypes.mockResolvedValueOnce({ status: 201 });
        api.getItemTypes.mockResolvedValueOnce({ data: [{ id: 2, name: "Bottle", unit: "ml" }] });

        const { result } = renderHook(() => useItemTypes());

        await waitFor(() => expect(result.current.items).toEqual([]));

        await act(async () => {
            await result.current.create({ name: "Bottle", unit: "ml" });
        });

        await waitFor(() => {
            expect(result.current.items).toEqual([{ id: 2, name: "Bottle", unit: "ml" }]);
        });
    });

    test("removes item and reloads", async () => {
        api.getItemTypes.mockResolvedValueOnce({ data: [{ id: 1, name: "Box", unit: "pcs" }] });
        api.deleteItemTypes.mockResolvedValueOnce({ status: 204 });
        api.getItemTypes.mockResolvedValueOnce({ data: [] });

        const { result } = renderHook(() => useItemTypes());

        await waitFor(() => expect(result.current.items).toEqual([{ id: 1, name: "Box", unit: "pcs" }]));

        await act(async () => {
            await result.current.remove(1);
        });

        await waitFor(() => {
            expect(result.current.items).toEqual([]);
        });
    });
});
