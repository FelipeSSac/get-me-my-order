import { useEffect, useState, useCallback, useRef } from "react";
import type { Order } from "../../type/order";
import { fetchOrders } from "./handler/fetch-orders";
import { useOrderStatusUpdates } from "../use-order-status-updates";
import { OrderStatusNotification } from "../../type/order-status-notification";

export function useOrderList(refreshTrigger?: number) {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [statusFilter, setStatusFilter] = useState("all");
  const [error, setError] = useState<string | null>(null);
  const pageSize = 5;

  // Keep track of current order IDs for the SignalR handler
  const currentOrderIdsRef = useRef<Set<string>>(new Set());

  // Update the ref whenever orders change
  useEffect(() => {
    currentOrderIdsRef.current = new Set(orders.map(o => o.id));
  }, [orders]);

  const handleFetchOrders = () => {
    fetchOrders(
      currentPage,
      pageSize,
      statusFilter,
      setTotalPages,
      setOrders,
      setLoading,
      setError
    );
  };

  // Handle real-time order status updates via SignalR
  const handleStatusUpdate = useCallback((update: OrderStatusNotification) => {
    setOrders((prevOrders) => {
      const orderFound = prevOrders.find(o => o.id === update.orderId);

      if (!orderFound) {
        return prevOrders;
      }

      return prevOrders.map((order) =>
        order.id === update.orderId
          ? {
              ...order,
              status: update.status,
              updatedAt: update.timestamp
            }
          : order
      );
    });
  }, []);

  useOrderStatusUpdates({
    onStatusUpdate: handleStatusUpdate,
    enabled: true,
  });

  useEffect(() => {
    handleFetchOrders();
  }, [currentPage, statusFilter, refreshTrigger]);

  return {
    currentPage,
    setCurrentPage,
    statusFilter,
    setStatusFilter,
    totalPages,
    orders,
    loading,
    error,
    fetchOrders: handleFetchOrders,
  };
}
