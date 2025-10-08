"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../atom/card";
import { Button } from "../../atom/button";
import {
  ArrowLeft,
  RefreshCw,
  User,
  Calendar,
  DollarSign,
  Package,
} from "lucide-react";
import { formatDate } from "../../../lib/util/format-date";
import { StatusBadge } from "../../molecule/status-badge";
import { formatCurrency } from "../../../lib/util/format-currency";
import { useOrderDetail } from "../../../lib/hook/use-order-detail";
import { OrderDetailProps } from "./order-detail.props";

export function OrderDetail({ orderId, onBack }: OrderDetailProps) {
  const { order, loading, fetchOrder } = useOrderDetail(orderId);

  if (loading || !order) {
    return (
      <Card>
        <CardContent className="py-12">
          <div className="flex items-center justify-center gap-2 text-muted-foreground">
            <RefreshCw className="h-4 w-4 animate-spin" />
            Loading order details...
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <Button onClick={onBack} variant="ghost" className="gap-2">
          <ArrowLeft className="h-4 w-4" />
          Back to Orders
        </Button>
        <Button
          onClick={fetchOrder}
          variant="outline"
          size="sm"
          className="gap-2 bg-transparent"
        >
          <RefreshCw className="h-4 w-4" />
          Refresh
        </Button>
      </div>

      <Card>
        <CardHeader>
          <div className="flex items-start justify-between">
            <div>
              <CardTitle className="text-2xl">Order #{order.id}</CardTitle>
              <CardDescription className="mt-2">
                Created {formatDate(order.createdAt)}
              </CardDescription>
            </div>
            <StatusBadge status={order.status} size="lg" />
          </div>
        </CardHeader>
        <CardContent className="space-y-6">
          <div className="grid gap-6 md:grid-cols-2">
            <div className="flex items-start gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
                <User className="h-5 w-5 text-primary" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Customer</p>
                <p className="text-lg font-medium text-foreground">
                  {order.client.fisrtName} {order.client.lastName}
                </p>
              </div>
            </div>

            <div className="flex items-start gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
                <Calendar className="h-5 w-5 text-primary" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Created At</p>
                <p className="text-lg font-medium text-foreground">
                  {formatDate(order.createdAt)}
                </p>
              </div>
            </div>
          </div>

          <div className="space-y-3">
            <h3 className="text-sm font-medium text-foreground">Order Items</h3>
            <div className="rounded-lg border border-border">
              <table className="w-full">
                <thead>
                  <tr className="border-b border-border bg-muted/30 text-left text-sm text-muted-foreground">
                    <th className="p-3 font-medium">Product</th>
                    <th className="p-3 font-medium text-right">Unit Price</th>
                    <th className="p-3 font-medium text-right">Quantity</th>
                    <th className="p-3 font-medium text-right">Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {order.products.map((item, index: number) => (
                    <tr
                      key={index}
                      className="border-b border-border/50 last:border-0"
                    >
                      <td className="p-3">
                        <div className="flex items-center gap-2">
                          <Package className="h-4 w-4 text-muted-foreground" />
                          <span className="text-sm text-foreground">
                            {item.productName}
                          </span>
                        </div>
                      </td>
                      <td className="p-3 text-right text-sm text-muted-foreground">
                        {formatCurrency(item.unitPriceAmount)}
                      </td>
                      <td className="p-3 text-right text-sm text-foreground">
                        {item.quantity}x
                      </td>
                      <td className="p-3 text-right text-sm font-medium text-foreground">
                        {formatCurrency(item.unitPriceAmount * item.quantity)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>

          <div className="rounded-lg border border-border bg-muted/30 p-4">
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
                  <DollarSign className="h-5 w-5 text-primary" />
                </div>
                <span className="text-lg font-medium text-foreground">
                  Total Value
                </span>
              </div>
              <span className="text-2xl font-bold text-primary">
                {formatCurrency(order.totalAmount)}
              </span>
            </div>
          </div>

          <div className="rounded-lg border border-border bg-muted/30 p-4">
            <h3 className="mb-2 text-sm font-medium text-foreground">
              Status Flow
            </h3>
            <div className="flex items-center gap-2">
              <StatusBadge status="Pending" />
              <div className="h-px flex-1 bg-border" />
              <StatusBadge status="Processing" />
              <div className="h-px flex-1 bg-border" />
              <StatusBadge status="Done" />
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
