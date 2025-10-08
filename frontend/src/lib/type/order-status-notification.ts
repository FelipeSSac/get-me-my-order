import { OrderStatus } from "./order-status";

export interface OrderStatusNotification {
  orderId: string;
  status: OrderStatus;
  timestamp: string;
  data?: {
    totalValue?: number;
    currency?: string;
    updatedAt?: string;
  };
}
