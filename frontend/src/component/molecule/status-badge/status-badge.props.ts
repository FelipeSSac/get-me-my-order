import { OrderStatus } from "../../../lib/type/order-status";

export interface StatusBadgeProps {
  status: OrderStatus;
  size?: "sm" | "lg";
}
