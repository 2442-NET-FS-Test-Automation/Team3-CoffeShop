
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