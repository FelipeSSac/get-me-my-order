"use client";

import type React from "react";
import { Card, CardContent } from "../../atom/card";
import { Input } from "../../atom/input";
import { FormHeader } from "../../molecule/form-header";
import { FormField } from "../../molecule/form-field";
import { FormActions } from "../../molecule/form-actions";
import { useCreateCustomer } from "../../../lib/hook/use-create-customer";
import { CreateCustomerFormProps } from "./create-customer-form.props";
import { FormEvent } from "react";

export function CreateCustomerForm({
  onSuccess,
  onCancel,
}: CreateCustomerFormProps) {
  const { loading, formData, handleChange, handleSubmit } = useCreateCustomer();

  const onSubmit = (e: FormEvent) => {
    e.preventDefault();
    handleSubmit(onSuccess);
  };

  return (
    <Card>
      <FormHeader
        title="Create New Customer"
        description="Add a new customer to the system"
        onBack={onCancel}
      />
      <CardContent>
        <form onSubmit={onSubmit} className="space-y-6">
          <FormField label="First Name" htmlFor="firstName">
            <Input
              id="firstName"
              placeholder="Enter first name"
              value={formData.firstName}
              onChange={(e) => handleChange("firstName", e.target.value)}
              required
              disabled={loading}
            />
          </FormField>

          <FormField label="Last Name" htmlFor="lastName">
            <Input
              id="lastName"
              placeholder="Enter last name"
              value={formData.lastName}
              onChange={(e) => handleChange("lastName", e.target.value)}
              required
              disabled={loading}
            />
          </FormField>

          <FormField label="Email" htmlFor="email">
            <Input
              id="email"
              type="email"
              placeholder="customer@email.com"
              value={formData.email}
              onChange={(e) => handleChange("email", e.target.value)}
              required
              disabled={loading}
            />
          </FormField>

          <FormActions
            submitLabel="Create Customer"
            loadingLabel="Creating Customer..."
            isLoading={loading}
            onCancel={onCancel}
          />
        </form>
      </CardContent>
    </Card>
  );
}
