
export interface LoginRequest {
    
    username: string;
    password: string;


}

export interface LoginResponse {

    token: string;

}

export interface AuthUser {

    name: string;
    role: string;

}

export const UserRole = {

    Manager: "Manager",
    Barista: "Barista",
    Customer: "Customer"
} as const;

export type UserRole = typeof UserRole [keyof typeof UserRole];
export type Sku = string;

export interface InventoryItem{
    sku: Sku;
    name: string;
    price: number;
    currentStock: number;
}

export type FetchState = "idle" | "loading" | "loaded" | "failed"

export const HttpStatus = {
    Ok : 200,
    Created : 201,
    NoContent : 204,
    BadRequest : 400,
    Unauthorized : 401,
    Forbidden : 403,
    NotFound : 404
} as const;
export type HttpStatus = typeof HttpStatus[keyof typeof HttpStatus];

export const SortDirection = {
    Ascending : "asc",
    Descending : "desc"
} as const;
export type SortDirection = typeof SortDirection[keyof typeof SortDirection];
