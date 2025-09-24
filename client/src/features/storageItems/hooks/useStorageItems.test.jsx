import {renderHook, act} from "@testing-library/react";
import {useStorageItems} from "./useStorageItems";
import * as api from "../../../api/storageItems";

vi.mock("../../../api/storageItems", () => ({
    getStorageItems: vi.fn(() => Promise.resolve({data: []})),
    createStorageItem: vi.fn(() => Promise.resolve({status: 201})),
    patchStorageItem: vi.fn(() => Promise.resolve({status: 201})),
    deleteStorageItem: vi.fn(() => Promise.resolve({status: 204})),
}));

describe("useStorageItems", () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("loads items on mount", async () => {
        api.getStorageItems.mockResolvedValueOnce({
            data: [{id: 1, amount: 5, itemType: {name: "Flour", unit: "g"}}],
        });

        const {result} = renderHook(() => useStorageItems());

        await act(async () => {
        });

        expect(result.current.items).toEqual([
            {id: 1, amount: 5, itemType: {name: "Flour", unit: "g"}},
        ]);
    });

    it("creates an item and reloads on 201", async () => {
        api.createStorageItem.mockResolvedValueOnce({status: 201});
        api.getStorageItems
            .mockResolvedValueOnce({data: []})
            .mockResolvedValueOnce({
                data: [{id: 2, amount: 10, itemType: {name: "Sugar", unit: "g"}}],
            });

        const {result} = renderHook(() => useStorageItems());

        await act(async () => {
            await result.current.create({amount: 10, itemTypeId: 1});
        });

        expect(api.createStorageItem).toHaveBeenCalledWith({amount: 10, itemTypeId: 1});
        expect(api.getStorageItems).toHaveBeenCalled();
        expect(result.current.items).toEqual([
            {id: 2, amount: 10, itemType: {name: "Sugar", unit: "g"}},
        ]);
    });

    it("patches an item and reloads on 201", async () => {
        api.patchStorageItem.mockResolvedValueOnce({status: 201});
        api.getStorageItems
            .mockResolvedValueOnce({data: []})
            .mockResolvedValueOnce({
                data: [{id: 3, amount: 20, itemType: {name: "Rice", unit: "kg"}}],
            });

        const {result} = renderHook(() => useStorageItems());

        await act(async () => {
            await result.current.patch({id: 3, amount: 20});
        });

        expect(api.patchStorageItem).toHaveBeenCalledWith({id: 3, amount: 20});
        expect(result.current.items).toEqual([
            {id: 3, amount: 20, itemType: {name: "Rice", unit: "kg"}},
        ]);
    });

    it("removes an item and reloads on 204", async () => {
        api.deleteStorageItem.mockResolvedValueOnce({status: 204});
        api.getStorageItems.mockResolvedValueOnce({data: []});

        const {result} = renderHook(() => useStorageItems());

        await act(async () => {
            await result.current.remove(1);
        });

        expect(api.deleteStorageItem).toHaveBeenCalledWith(1);
        expect(result.current.items).toEqual([]);
    });
});
