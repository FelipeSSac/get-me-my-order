"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../atom/card";
import { Button } from "../../atom/button";
import { Eye, RefreshCw } from "lucide-react";
import { formatCurrency } from "../../../lib/util/format-currency";
import { StatusBadge } from "../../molecule/status-badge";
import { formatDate } from "../../../lib/util/format-date";
import { useOrderList } from "../../../lib/hook/use-order-list";
import { OrderListProps } from "./order-list.props";
import { PaginationFooter } from "../../molecule/pagination-footer";

export function OrderList({ onOrderSelect, refreshTrigger }: OrderListProps) {
  const {
    orders,
    currentPage,
    setCurrentPage,
    statusFilter,
    setStatusFilter,
    totalPages,
    loading,
    error,
    fetchOrders,
  } = useOrderList(refreshTrigger);

  if (loading) {
    return (
      <Card>
        <CardContent className="py-12">
          <div className="flex items-center justify-center gap-2 text-muted-foreground">
            <RefreshCw className="h-4 w-4 animate-spin" />
            Loading orders...
          </div>
        </CardContent>
      </Card>
    );
  }

  if (error) {
    return (
      <Card>
        <CardContent className="py-12">
          <div className="text-center">
            <p className="text-destructive">{error}</p>
            <Button
              onClick={fetchOrders}
              variant="outline"
              className="mt-4 bg-transparent"
            >
              Try Again
            </Button>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center justify-between">
          <div>
            <CardTitle>Orders</CardTitle>
            <CardDescription>
              Manage and track all orders in the system
            </CardDescription>
          </div>
          <Button
            onClick={fetchOrders}
            variant="outline"
            size="sm"
            className="gap-2 bg-transparent"
          >
            <RefreshCw className="h-4 w-4" />
            Refresh
          </Button>
        </div>
        <div className="flex gap-2 pt-4">
          <Button
            onClick={() => setStatusFilter("all")}
            variant={statusFilter === "all" ? "default" : "outline"}
            size="sm"
            className={statusFilter === "all" ? "" : "bg-transparent"}
          >
            All
          </Button>
          <Button
            onClick={() => setStatusFilter("pending")}
            variant={statusFilter === "pending" ? "default" : "outline"}
            size="sm"
            className={statusFilter === "pending" ? "" : "bg-transparent"}
          >
            Pending
          </Button>
          <Button
            onClick={() => setStatusFilter("processing")}
            variant={statusFilter === "processing" ? "default" : "outline"}
            size="sm"
            className={statusFilter === "processing" ? "" : "bg-transparent"}
          >
            Processing
          </Button>
          <Button
            onClick={() => setStatusFilter("done")}
            variant={statusFilter === "done" ? "default" : "outline"}
            size="sm"
            className={statusFilter === "done" ? "" : "bg-transparent"}
          >
            Completed
          </Button>
        </div>
      </CardHeader>
      <CardContent>
        {orders.length === 0 ? (
          <div className="py-12 text-center text-muted-foreground">
            No orders found. Create your first order to get started.
          </div>
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b border-border text-left text-sm text-muted-foreground">
                    <th className="pb-3 font-medium">Order ID</th>
                    <th className="pb-3 font-medium">Customer</th>
                    <th className="pb-3 font-medium">Items</th>
                    <th className="pb-3 font-medium">Total Value</th>
                    <th className="pb-3 font-medium">Status</th>
                    <th className="pb-3 font-medium">Created</th>
                    <th className="pb-3 font-medium">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {orders.map((order) => (
                    <tr
                      key={order.id}
                      className="border-b border-border/50 transition-colors hover:bg-accent/50"
                    >
                      <td className="py-4">
                        <span className="font-mono text-sm text-foreground">
                          #{order.id}
                        </span>
                      </td>
                      <td className="py-4 text-sm text-foreground">
                        {order.client.fisrtName} {order.client.lastName}
                      </td>
                      <td className="py-4 text-sm text-muted-foreground">
                        {order.products.length} item(s)
                      </td>
                      <td className="py-4 text-sm font-medium text-foreground">
                        {formatCurrency(order.totalAmount)}
                      </td>
                      <td className="py-4">
                        <StatusBadge status={order.status} />
                      </td>
                      <td className="py-4 text-sm text-muted-foreground">
                        {formatDate(order.createdAt)}
                      </td>
                      <td className="py-4">
                        <Button
                          onClick={() => onOrderSelect(order.id)}
                          variant="ghost"
                          size="sm"
                          className="gap-2"
                        >
                          <Eye className="h-4 w-4" />
                          View
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            {totalPages > 1 && (
              <PaginationFooter
                currentPage={currentPage}
                setCurrentPage={setCurrentPage}
                totalPages={totalPages}
              />
            )}
          </>
        )}
      </CardContent>
    </Card>
  );
}
