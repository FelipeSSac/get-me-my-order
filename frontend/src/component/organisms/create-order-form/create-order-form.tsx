"use client";

import type React from "react";
import { Card, CardContent } from "../../atom/card";
import { Button } from "../../atom/button";
import { Label } from "../../atom/label";
import { Plus } from "lucide-react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../atom/select";
import { FormHeader } from "../../molecule/form-header";
import { FormField } from "../../molecule/form-field";
import { FormActions } from "../../molecule/form-actions";
import { OrderItemRow } from "../../molecule/order-item-row";
import { OrderSummary } from "../../molecule/order-summary";
import { useCreateOrder } from "../../../lib/hook/use-create-order";
import { CreateOrderFormProps } from "./create-order-form.props";

export function CreateOrderForm({ onSuccess, onCancel }: CreateOrderFormProps) {
  const {
    loading,
    customers,
    products,
    selectedCustomerId,
    orderItems,
    totalValue,
    currency,
    setSelectedCustomerId,
    handleAddItem,
    handleUpdateItem,
    handleRemoveItem,
    handleSubmit,
  } = useCreateOrder();

  const onSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    handleSubmit(onSuccess);
  };

  return (
    <Card>
      <FormHeader
        title="Create New Order"
        description="Select a customer and add products to create an order. The order will be sent to processing flow."
        onBack={onCancel}
      />
      <CardContent>
        <form onSubmit={onSubmit} className="space-y-6">
          <FormField label="Customer" htmlFor="customer">
            <Select
              value={selectedCustomerId}
              onValueChange={setSelectedCustomerId}
              disabled={loading}
            >
              <SelectTrigger id="customer">
                <SelectValue placeholder="Select a customer" />
              </SelectTrigger>
              <SelectContent>
                {customers.map((customer) => (
                  <SelectItem key={customer.id} value={customer.id}>
                    {customer.firstName} {customer.lastName} - {customer.email}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </FormField>

          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <Label>Order Items</Label>
              <Button
                type="button"
                onClick={handleAddItem}
                variant="outline"
                size="sm"
                className="gap-2 bg-transparent"
              >
                <Plus className="h-4 w-4" />
                Add Item
              </Button>
            </div>

            {orderItems.length === 0 ? (
              <div className="rounded-lg border border-dashed border-border bg-muted/30 p-8 text-center">
                <p className="text-sm text-muted-foreground">
                  No items added yet. Click &quot;Add Item&quot; to get started.
                </p>
              </div>
            ) : (
              <div className="space-y-3">
                {orderItems.map((item, index) => (
                  <OrderItemRow
                    key={index}
                    item={item}
                    index={index}
                    products={products}
                    isLoading={loading}
                    onUpdateItem={handleUpdateItem}
                    onRemoveItem={handleRemoveItem}
                  />
                ))}
              </div>
            )}
          </div>

          {orderItems.length > 0 && <OrderSummary totalValue={totalValue} currency={currency} />}

          <FormActions
            submitLabel="Create Order"
            loadingLabel="Creating Order..."
            isLoading={loading}
            isDisabled={!selectedCustomerId || orderItems.length === 0}
            onCancel={onCancel}
          />
        </form>
      </CardContent>
    </Card>
  );
}
