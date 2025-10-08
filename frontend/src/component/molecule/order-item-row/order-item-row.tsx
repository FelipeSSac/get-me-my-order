import { Trash2 } from "lucide-react";
import { Button } from "../../atom/button";
import { Input } from "../../atom/input";
import { Label } from "../../atom/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../atom/select";
import { formatCurrency } from "../../../lib/util/format-currency";
import { OrderItemRowProps } from "./order-item-row.props";

export function OrderItemRow({
  item,
  index,
  products,
  isLoading,
  onUpdateItem,
  onRemoveItem,
}: OrderItemRowProps) {
  return (
    <div className="rounded-lg border border-border bg-card p-4">
      <div className="grid gap-4 md:grid-cols-[2fr_1fr_1fr_auto]">
        <div className="space-y-2">
          <Label htmlFor={`product-${index}`}>Product</Label>
          <Select
            value={item.productId}
            onValueChange={(value) => onUpdateItem(index, "productId", value)}
            disabled={isLoading}
          >
            <SelectTrigger id={`product-${index}`}>
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              {products.map((product) => (
                <SelectItem key={product.id} value={product.id}>
                  {product.name} - {formatCurrency(product.price, product.currency)}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="space-y-2">
          <Label htmlFor={`quantity-${index}`}>Quantity</Label>
          <Input
            id={`quantity-${index}`}
            type="number"
            min="1"
            value={item.quantity}
            onChange={(e) => onUpdateItem(index, "quantity", e.target.value)}
            disabled={isLoading}
          />
        </div>

        <div className="space-y-2">
          <Label>Subtotal</Label>
          <div className="flex h-10 items-center rounded-md border border-border bg-muted px-3 text-sm font-medium">
            {formatCurrency(item.unitPriceAmount * item.quantity, item.unitPriceCurrency)}
          </div>
        </div>

        <div className="flex items-end">
          <Button
            type="button"
            onClick={() => onRemoveItem(index)}
            variant="ghost"
            size="icon"
            className="text-destructive hover:bg-destructive/10 hover:text-destructive"
            disabled={isLoading}
          >
            <Trash2 className="h-4 w-4" />
          </Button>
        </div>
      </div>
    </div>
  );
}
