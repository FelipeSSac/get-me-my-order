import type { Product } from "../../../type/product";
import type { OrderItem } from "../../../type/order-item";
import { Dispatch, SetStateAction } from "react";

export function createHandleAddItem(
  products: Product[],
  orderItems: OrderItem[],
  setOrderItems: Dispatch<SetStateAction<OrderItem[]>>
) {
  return () => {
    if (products.length === 0) return;

    const firstProduct = products[0];
    const newItem: OrderItem = {
      productId: firstProduct.id,
      productName: firstProduct.name,
      quantity: 1,
      unitPriceAmount: firstProduct.price,
      unitPriceCurrency: firstProduct.currency,
    };
    setOrderItems([...orderItems, newItem]);
  };
}
