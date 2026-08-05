import { api } from "./client";

// Here lives the catalog data call to the api.

export interface InventoryItem {

    productId: number;
    sku: string;
    name: string;
    stock: number;
    price: number;
}


export interface CreateInventoryBody {
    sku: string;
    name: string;
    price: number;
    stock: number;
}

export async function getInventory(): Promise<InventoryItem[]> {

    const response = await api.get<InventoryItem[]>("/api/inventory");
    return response.data; 
}

export async function getInventoryItem(sku: string): Promise<InventoryItem> {
    const response = await api.get<InventoryItem>(`/api/inventory/${sku}`);
    return response.data;
}


export async function createDrink(body:CreateInventoryBody): Promise<InventoryItem> {
    const response = await api.post<InventoryItem>("/api/inventory", body);
    return response.data;
}

export async function deleteDrink(sku: string): Promise<void> {
    await api.delete(`/api/inventory/${sku}`);
}

export async function editDrink(item: {sku: string; name: string; price: number; stock: number }) {

    await api.put(`/api/inventory/edit`, item);
    
}
