import { CustomerFormData } from "@/src/lib/type/customer-form-data";
import { customerApi } from "../../../api/customer.api";

export function createHandleSubmit(
  formData: CustomerFormData,
  setLoading: React.Dispatch<React.SetStateAction<boolean>>
) {
  return async (onSuccess: () => void) => {
    setLoading(true);

    try {
      await customerApi.create({
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
      });
      onSuccess();
    } catch (error) {
      console.error("Failed to create customer:", error);
    } finally {
      setLoading(false);
    }
  };
}
