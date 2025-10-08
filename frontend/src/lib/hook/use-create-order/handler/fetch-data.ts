import { Dispatch, SetStateAction } from "react";
import type { Customer } from "../../../type/customer";
import type { Product } from "../../../type/product";
import { customerApi } from "../../../api/customer.api";
import { productApi } from "../../../api/product.api";

export async function fetchData(
  setCustomers: Dispatch<SetStateAction<Customer[]>>,
  setProducts: Dispatch<SetStateAction<Product[]>>
) {
  try {
    // Fetch customers and products from API
    const [customersResponse, productsResponse] = await Promise.all([
      customerApi.getAll(),
      productApi.getAll(),
    ]);

    setCustomers(customersResponse.items);
    setProducts(productsResponse.items);
  } catch (error) {
    console.error("Failed to fetch data:", error);
    setCustomers([]);
    setProducts([]);
  }
}
