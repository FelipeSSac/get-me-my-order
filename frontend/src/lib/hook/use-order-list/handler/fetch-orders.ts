import type { Order } from "../../../type/order";
import { orderApi } from "../../../api/order.api";

export async function fetchOrders(
  page: number,
  pageSize: number,
  status: string,
  setTotalPages: React.Dispatch<React.SetStateAction<number>>,
  setOrders: React.Dispatch<React.SetStateAction<Order[]>>,
  setLoading: React.Dispatch<React.SetStateAction<boolean>>,
  setError: React.Dispatch<React.SetStateAction<string | null>>
) {
  try {
    setLoading(true);
    setError(null);

    const result = await orderApi.getAll({
      page,
      pageSize,
      status,
    });

    setOrders(result.items || []);
    setTotalPages(result.totalPages || 0);
  } catch (err) {
    setError("Failed to load orders");
    console.error(err);
    setOrders([]);
  } finally {
    setLoading(false);
  }
}
