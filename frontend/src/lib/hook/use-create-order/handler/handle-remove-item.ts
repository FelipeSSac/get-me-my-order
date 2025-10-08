import { Dispatch, SetStateAction } from "react";
import type { OrderItem } from "../../../type/order-item";

export function createHandleRemoveItem(
  orderItems: OrderItem[],
  setOrderItems: Dispatch<SetStateAction<OrderItem[]>>
) {
  return (index: number) => {
    setOrderItems(orderItems.filter((_, i) => i !== index));
  };
}
