import { CreateOrderProductRequest } from "./create-order-product-request";

export interface CreateOrderRequest {
  productList: CreateOrderProductRequest[];
  clientId: string;
}
