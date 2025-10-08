import { apiClient } from "./client";
import type { Order } from "../type/order";
import { PaginatedEntity } from "../type/paginated-entity";
import { CreateOrderRequest } from "./interface/create-order-request";
import { GetOrdersParams } from "./interface/get-orders-params";

export const orderApi = {
  create: (data: CreateOrderRequest) => apiClient.post<void>("/orders", data),

  getAll: async (params?: GetOrdersParams) => {
    const queryParams = new URLSearchParams();
    if (params?.page) queryParams.set("page", params.page.toString());
    if (params?.pageSize)
      queryParams.set("pageSize", params.pageSize.toString());

    if (params?.status && params.status !== "all")
      queryParams.set("status", params.status);

    const query = queryParams.toString();
    const result = await apiClient.get<PaginatedEntity<Order>>(
      `/orders${query ? `?${query}` : ""}`
    );

    return result;
  },

  getById: (id: string) => apiClient.get<Order>(`/orders/${id}`),
};
