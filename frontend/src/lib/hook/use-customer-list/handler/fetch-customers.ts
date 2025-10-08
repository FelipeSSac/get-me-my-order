import type { Customer } from "../../../type/customer";
import { customerApi } from "../../../api/customer.api";

export async function fetchCustomers(
  page: number,
  pageSize: number,
  setTotalPages: React.Dispatch<React.SetStateAction<number>>,
  setCustomers: React.Dispatch<React.SetStateAction<Customer[]>>,
  setLoading: React.Dispatch<React.SetStateAction<boolean>>
) {
  try {
    setLoading(true);

    const result = await customerApi.getAll({
      page,
      pageSize,
    });

    setCustomers(result.items || []);
    setTotalPages(result.totalPages || 0);
  } catch (error) {
    console.error("Failed to load customers:", error);
    setCustomers([]);
  } finally {
    setLoading(false);
  }
}
