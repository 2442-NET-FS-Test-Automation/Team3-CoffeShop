import { api } from "./client";

export type CreateOrderLineDto = {
    productId: number;
    quantity: number;
};

export type CreateOrderDto = {
    lines: CreateOrderLineDto[];
};

export type OrderLineDto = {
    orderLineId: number;
    productId: number;
    productName?: string;
    quantity: number;
    unitPrice: number;
    subtotal: number;
};

export type OrderDto = {
    orderId: number;
    userId: number;
    cashierName?: string;
    total: number;
    lines: OrderLineDto[];
};

export async function createOrder(dto: CreateOrderDto): Promise<OrderDto> {
    const response = await api.post<OrderDto>("/api/orders", dto);
    return response.data;
}
