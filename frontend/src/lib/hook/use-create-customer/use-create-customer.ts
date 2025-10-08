import { useState } from "react";
import { createHandleChange } from "./handler/handle-change";
import { createHandleSubmit } from "./handler/handle-submit";
import { CustomerFormData } from "../../type/customer-form-data";

export function useCreateCustomer() {
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState<CustomerFormData>({
    firstName: "",
    lastName: "",
    email: "",
  });

  const handleChange = createHandleChange(setFormData);
  const handleSubmit = createHandleSubmit(formData, setLoading);

  return {
    loading,
    formData,
    handleChange,
    handleSubmit,
  };
}
