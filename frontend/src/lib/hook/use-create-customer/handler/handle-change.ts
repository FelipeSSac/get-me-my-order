import { CustomerFormData } from "@/src/lib/type/customer-form-data";
import { Dispatch, SetStateAction } from "react";

export function createHandleChange(
  setFormData: Dispatch<SetStateAction<CustomerFormData>>
) {
  return (field: keyof CustomerFormData, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };
}
