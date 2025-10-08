import { useEffect, useState } from "react";
import type { Customer } from "../../type/customer";
import { fetchCustomers } from "./handler/fetch-customers";

export function useCustomerList(refreshTrigger?: number) {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 6;

  const handleFetchCustomers = () => {
    fetchCustomers(
      currentPage,
      pageSize,
      setTotalPages,
      setCustomers,
      setLoading
    );
  };

  useEffect(() => {
    handleFetchCustomers();
  }, [currentPage, refreshTrigger]);

  return {
    customers,
    loading,
    currentPage,
    setCurrentPage,
    totalPages,
    fetchCustomers: handleFetchCustomers,
  };
}
