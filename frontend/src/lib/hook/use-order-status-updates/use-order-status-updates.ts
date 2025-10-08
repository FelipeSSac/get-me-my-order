import { useEffect, useCallback } from "react";
import { signalRService } from "../../service/signalr-service";
import { OrderStatusNotification } from "../../type/order-status-notification";

interface UseOrderStatusUpdatesOptions {
  orderId?: string;
  onStatusUpdate: (update: OrderStatusNotification) => void;
  enabled?: boolean;
}

export function useOrderStatusUpdates({
  orderId,
  onStatusUpdate,
  enabled = true,
}: UseOrderStatusUpdatesOptions) {
  const handleStatusUpdate = useCallback(
    (notification: OrderStatusNotification) => {
      onStatusUpdate(notification);
    },
    [onStatusUpdate]
  );

  useEffect(() => {
    // Only run on client side
    if (typeof window === "undefined" || !enabled) {
      return;
    }

    let isSubscribed = false;
    let isMounted = true;

    const setupConnection = async () => {
      try {
        await signalRService.connect();

        if (!isMounted) return;

        if (orderId) {
          // Subscribe to specific order
          await signalRService.subscribeToOrder(orderId, handleStatusUpdate);
          isSubscribed = true;
        } else {
          // Listen to all order status changes
          signalRService.onOrderStatusChanged(handleStatusUpdate);
        }
      } catch (error) {
        console.error("[useOrderStatusUpdates] Setup failed:", error);
      }
    };

    setupConnection();

    return () => {
      isMounted = false;
      const cleanup = async () => {
        if (orderId && isSubscribed) {
          await signalRService.unsubscribeFromOrder(orderId, handleStatusUpdate);
        } else {
          signalRService.offOrderStatusChanged(handleStatusUpdate);
        }
      };

      cleanup();
    };
  }, [orderId, handleStatusUpdate, enabled]);
}
