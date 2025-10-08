import { ProductFormData } from "@/src/lib/type/product-form-data";
import { productApi } from "../../../api/product.api";

export function createHandleSubmit(
  formData: ProductFormData,
  setLoading: React.Dispatch<React.SetStateAction<boolean>>
) {
  return async (onSuccess: () => void) => {
    setLoading(true);

    try {
      await productApi.create({
        name: formData.name,
        value: parseFloat(formData.value),
        currency: formData.currency,
      });
      onSuccess();
    } catch (error) {
      console.error("Failed to create product:", error);
    } finally {
      setLoading(false);
    }
  };
}
