import type { Order } from "../../../type/order";
import { orderApi } from "../../../api/order.api";

export async function fetchOrder(
  orderId: string,
  setOrder: React.Dispatch<React.SetStateAction<Order | null>>,
  setLoading: React.Dispatch<React.SetStateAction<boolean>>
) {
  try {
    setLoading(true);
    const order = await orderApi.getById(orderId);
    setOrder(order);
  } catch (error) {
    console.error("Failed to load order:", error);
    setOrder(null);
  } finally {
    setLoading(false);
  }
}
