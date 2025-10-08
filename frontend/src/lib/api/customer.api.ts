import { apiClient } from "./client";
import type { Customer } from "../type/customer";
import { PaginatedEntity } from "../type/paginated-entity";
import { CreateClientRequest } from "./interface/create-client-request";
import { GetCustomerParams } from "./interface/get-customer-params";

export const customerApi = {
  create: (data: CreateClientRequest) => apiClient.post<void>("/clients", data),

  getAll: async (params?: GetCustomerParams) => {
    const queryParams = new URLSearchParams();
    if (params?.page) queryParams.set("page", params.page.toString());
    if (params?.pageSize)
      queryParams.set("pageSize", params.pageSize.toString());

    const query = queryParams.toString();
    const result = await apiClient.get<PaginatedEntity<Customer>>(
      `/clients${query ? `?${query}` : ""}`
    );

    return result;
  },
};
