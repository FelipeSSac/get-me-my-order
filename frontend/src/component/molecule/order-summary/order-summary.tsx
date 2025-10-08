import { formatCurrency } from "../../../lib/util/format-currency";
import { OrderSummaryProps } from "./order-summary.props";

export function OrderSummary({ totalValue, currency }: OrderSummaryProps) {
  return (
    <div className="rounded-lg border border-border bg-muted/30 p-4">
      <div className="flex items-center justify-between">
        <span className="text-lg font-medium text-foreground">Total Value</span>
        <span className="text-2xl font-bold text-primary">
          {formatCurrency(totalValue, currency)}
        </span>
      </div>
    </div>
  );
}
