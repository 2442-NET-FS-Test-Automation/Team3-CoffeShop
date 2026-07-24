import { api } from "./client";
import type { InventoryItem} from "../types";

// Here lives the catalog data call to the api.

export interface CreateInventoryBody {
    sku: string;
    name: string;
    price: number;
    currentStock: number;
}

export async function getInventory(): Promise<InventoryItem[]> {

    const response = await api.get<InventoryItem[]>("/api/Inventory");
    return response.data; 
}

export async function getInventoryItem(sku: string): Promise<InventoryItem> {
    const response = await api.get<InventoryItem>(`/api/Inventory/${sku}`);
    return response.data;
}


export async function createBook(body:CreateInventoryBody): Promise<InventoryItem> {
    const response = await api.post<InventoryItem>("/api/Inventory", body);
    return response.data;
}

export async function deleteBook(sku: string): Promise<void> {
    await api.delete(`/api/Inventory/${sku}`);
}