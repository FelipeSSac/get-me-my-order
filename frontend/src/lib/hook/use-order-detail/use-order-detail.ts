import { useEffect, useState, useCallback } from "react";
import type { Order } from "../../type/order";
import { fetchOrder } from "./handler/fetch-order";
import { useOrderStatusUpdates } from "../use-order-status-updates";
import { OrderStatusNotification } from "../../type/order-status-notification";

export function useOrderDetail(orderId: string) {
  const [order, setOrder] = useState<Order | null>(null);
  const [loading, setLoading] = useState(true);

  const handleFetchOrder = () => {
    fetchOrder(orderId, setOrder, setLoading);
  };

  // Handle real-time order status updates via SignalR for this specific order
  const handleStatusUpdate = useCallback((update: OrderStatusNotification) => {
    setOrder((prevOrder) => {
      if (prevOrder && prevOrder.id === update.orderId) {
        return { ...prevOrder, status: update.status, updatedAt: update.timestamp };
      }
      return prevOrder;
    });
  }, []);

  useOrderStatusUpdates({
    orderId,
    onStatusUpdate: handleStatusUpdate,
    enabled: true,
  });

  useEffect(() => {
    handleFetchOrder();
  }, [orderId]);

  return {
    order,
    loading,
    fetchOrder: handleFetchOrder,
  };
}
