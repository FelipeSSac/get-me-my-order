import type { OrderItem } from "../../../lib/type/order-item";
import type { Product } from "../../../lib/type/product";

export interface OrderItemRowProps {
  item: OrderItem;
  index: number;
  products: Product[];
  isLoading: boolean;
  onUpdateItem: (
    index: number,
    field: "productId" | "quantity",
    value: string | number
  ) => void;
  onRemoveItem: (index: number) => void;
}
