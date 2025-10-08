import { apiClient } from "./client";
import type { Product } from "../type/product";
import { PaginatedEntity } from "../type/paginated-entity";
import { CreateProductRequest } from "./interface/create-product-request";
import { GetProductParams } from "./interface/get-product-params";

export const productApi = {
  create: (data: CreateProductRequest) =>
    apiClient.post<void>("/products", data),

  getAll: async (params?: GetProductParams) => {
    const queryParams = new URLSearchParams();
    if (params?.page) queryParams.set("page", params.page.toString());
    if (params?.pageSize)
      queryParams.set("pageSize", params.pageSize.toString());

    const query = queryParams.toString();
    const result = await apiClient.get<PaginatedEntity<Product>>(
      `/products${query ? `?${query}` : ""}`
    );

    return result;
  },
};
