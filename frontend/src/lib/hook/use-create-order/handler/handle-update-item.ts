import type { Product } from "../../../type/product";
import type { OrderItem } from "../../../type/order-item";
import { Dispatch, SetStateAction } from "react";

export function createHandleUpdateItem(
  products: Product[],
  orderItems: OrderItem[],
  setOrderItems: Dispatch<SetStateAction<OrderItem[]>>
) {
  return (
    index: number,
    field: "productId" | "quantity",
    value: string | number
  ) => {
    const newItems = [...orderItems];
    const item = newItems[index];

    if (field === "productId") {
      const product = products.find((p) => p.id === value);
      if (product) {
        item.productId = product.id;
        item.productName = product.name;
        item.unitPriceAmount = product.price;
        item.unitPriceCurrency = product.currency;
      }
    } else if (field === "quantity") {
      item.quantity = Number(value);
    }

    setOrderItems(newItems);
  };
}
