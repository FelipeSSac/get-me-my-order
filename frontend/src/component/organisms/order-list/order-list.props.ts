export interface OrderListProps {
  onOrderSelect: (orderId: string) => void;
  refreshTrigger?: number;
}
