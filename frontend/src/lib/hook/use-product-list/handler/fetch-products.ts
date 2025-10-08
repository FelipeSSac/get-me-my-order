import type { Product } from "../../../type/product";
import { productApi } from "../../../api/product.api";

export async function fetchProducts(
  page: number,
  pageSize: number,
  setTotalPages: React.Dispatch<React.SetStateAction<number>>,
  setProducts: React.Dispatch<React.SetStateAction<Product[]>>,
  setLoading: React.Dispatch<React.SetStateAction<boolean>>
) {
  try {
    setLoading(true);

    const result = await productApi.getAll({
      page,
      pageSize,
    });

    setProducts(result?.items || []);
    setTotalPages(result.totalPages || 0);
  } catch (error) {
    console.error("Failed to load products:", error);
    setProducts([]);
  } finally {
    setLoading(false);
  }
}
