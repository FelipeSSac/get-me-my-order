import {
  LogLevel,
  HubConnection,
  HubConnectionState,
  HubConnectionBuilder,
} from "@microsoft/signalr";
import { OrderStatusNotification } from "../type/order-status-notification";

const SIGNALR_HUB_URL =
  process.env.NEXT_PUBLIC_SIGNALR_URL || "http://localhost:5001/hubs/orders";

class SignalRService {
  private connection: HubConnection | null = null;
  private readonly hubUrl = SIGNALR_HUB_URL;

  async connect(): Promise<void> {
    if (this.connection?.state === HubConnectionState.Connected) {
      return;
    }

    this.connection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        withCredentials: true,
        skipNegotiation: false,
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          return Math.min(
            1000 * Math.pow(2, retryContext.previousRetryCount),
            30000
          );
        },
      })
      .configureLogging(LogLevel.Warning)
      .build();

    try {
      await this.connection.start();
    } catch (error) {
      console.error("[SignalR] Connection failed:", error);
      throw error;
    }
  }

  async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
    }
  }

  async subscribeToOrder(
    orderId: string,
    callback: (notification: OrderStatusNotification) => void
  ): Promise<void> {
    if (!this.connection) {
      throw new Error("SignalR connection not established");
    }

    await this.connection.invoke("SubscribeToOrder", orderId);

    this.connection.on("OrderStatusChanged", callback);
  }

  async unsubscribeFromOrder(
    orderId: string,
    callback: (notification: OrderStatusNotification) => void
  ): Promise<void> {
    if (!this.connection) {
      return;
    }

    await this.connection.invoke("UnsubscribeFromOrder", orderId);

    this.connection.off("OrderStatusChanged", callback);
  }

  onOrderStatusChanged(
    callback: (notification: OrderStatusNotification) => void
  ): void {
    if (!this.connection) {
      throw new Error("SignalR connection not established");
    }

    this.connection.on("OrderStatusChanged", callback);
  }

  offOrderStatusChanged(
    callback: (notification: OrderStatusNotification) => void
  ): void {
    if (this.connection) {
      this.connection.off("OrderStatusChanged", callback);
    }
  }

  getConnectionState(): HubConnectionState | null {
    return this.connection?.state ?? null;
  }
}

export const signalRService = new SignalRService();
