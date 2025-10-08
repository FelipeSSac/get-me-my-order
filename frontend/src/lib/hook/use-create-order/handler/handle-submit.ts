import { orderApi } from "../../../api/order.api";
import type { OrderItem } from "../../../type/order-item";

export function createHandleSubmit(
  selectedCustomerId: string,
  orderItems: OrderItem[],
  setLoading: React.Dispatch<React.SetStateAction<boolean>>
) {
  return async (onSuccess: () => void) => {
    if (!selectedCustomerId || orderItems.length === 0) {
      return;
    }

    setLoading(true);

    try {
      await orderApi.create({
        clientId: selectedCustomerId,
        productList: orderItems.map((item) => ({
          productId: item.productId,
          quantity: item.quantity,
        })),
      });
      onSuccess();
    } catch (error) {
      console.error("Failed to create order:", error);
    } finally {
      setLoading(false);
    }
  };
}
