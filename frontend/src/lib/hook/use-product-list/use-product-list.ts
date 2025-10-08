import { useEffect, useState } from "react";
import type { Product } from "../../type/product";
import { fetchProducts } from "./handler/fetch-products";

export function useProductList(refreshTrigger?: number) {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 6;

  const handleFetchProducts = () => {
    fetchProducts(
      currentPage,
      pageSize,
      setTotalPages,
      setProducts,
      setLoading
    );
  };

  useEffect(() => {
    handleFetchProducts();
  }, [currentPage, refreshTrigger]);

  return {
    products,
    loading,
    currentPage,
    setCurrentPage,
    totalPages,
    fetchProducts: handleFetchProducts,
  };
}
