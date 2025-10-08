import { OrderItem } from "./order-item";
import { OrderStatus } from "./order-status";

export type Client = {
  id: string;
  fisrtName: string;
  lastName: string;
  email: string;
  createdAt: string;
  updatedAt: string;
};

export type Order = {
  id: string;
  client: Client;
  products: OrderItem[];
  totalAmount: number;
  totalCurrency: string;
  status: OrderStatus;
  createdAt: string;
  updatedAt: string;
};
