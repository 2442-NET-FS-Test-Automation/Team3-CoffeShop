import { api } from "./client";

export async function login( username: string, password: string): Promise<string> {
    const response = await api.post<{token: string}>("/auth/login", { username, password });
    return response.data.token;
}

export async function Register(username:string, password: string, email: string, name: string) {

    const response = await api.post("/auth/register", { username, password, email, name });
    return response.data;

}