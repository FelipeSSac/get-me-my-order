import { Dispatch, SetStateAction } from "react";
import type { ProductFormData } from "../../../type/product-form-data";

export function createHandleChange(
  setFormData: Dispatch<SetStateAction<ProductFormData>>
) {
  return (field: keyof ProductFormData, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };
}
