import { useState } from "react";
import type { ProductFormData } from "../../type/product-form-data";
import { createHandleChange } from "./handler/handle-change";
import { createHandleSubmit } from "./handler/handle-submit";

export function useCreateProduct() {
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState<ProductFormData>({
    name: "",
    value: "",
    currency: "BRL",
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
