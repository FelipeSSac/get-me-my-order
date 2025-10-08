"use client";

import type React from "react";
import { Card, CardContent } from "../../atom/card";
import { Input } from "../../atom/input";
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
import { useCreateProduct } from "../../../lib/hook/use-create-product";
import { CreateProductFormProps } from "./create-product-form.props";
import { FormEvent } from "react";

export function CreateProductForm({
  onSuccess,
  onCancel,
}: CreateProductFormProps) {
  const { loading, formData, handleChange, handleSubmit } = useCreateProduct();

  const onSubmit = (e: FormEvent) => {
    e.preventDefault();
    handleSubmit(onSuccess);
  };

  return (
    <Card>
      <FormHeader
        title="Create New Product"
        description="Add a new product to the catalog"
        onBack={onCancel}
      />
      <CardContent>
        <form onSubmit={onSubmit} className="space-y-6">
          <FormField label="Product Name" htmlFor="name">
            <Input
              id="name"
              placeholder="Enter product name"
              value={formData.name}
              onChange={(e) => handleChange("name", e.target.value)}
              required
              disabled={loading}
            />
          </FormField>

          <div className="grid gap-4 sm:grid-cols-2">
            <FormField label="Value" htmlFor="value">
              <Input
                id="value"
                type="number"
                step="0.01"
                min="0"
                placeholder="0.00"
                value={formData.value}
                onChange={(e) => handleChange("value", e.target.value)}
                required
                disabled={loading}
              />
            </FormField>

            <FormField label="Currency" htmlFor="currency">
              <Select
                value={formData.currency}
                onValueChange={(value) => handleChange("currency", value)}
                disabled={loading}
              >
                <SelectTrigger id="currency">
                  <SelectValue placeholder="Select currency" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="BRL">BRL (R$)</SelectItem>
                  <SelectItem value="USD">USD ($)</SelectItem>
                  <SelectItem value="EUR">EUR (€)</SelectItem>
                </SelectContent>
              </Select>
            </FormField>
          </div>

          <FormActions
            submitLabel="Create Product"
            loadingLabel="Creating Product..."
            isLoading={loading}
            onCancel={onCancel}
          />
        </form>
      </CardContent>
    </Card>
  );
}
