import { useEffect, useState } from "react";
import type { Customer } from "../../type/customer";
import type { Product } from "../../type/product";
import type { OrderItem } from "../../type/order-item";
import { fetchData } from "./handler/fetch-data";
import { createHandleAddItem } from "./handler/handle-add-item";
import { createHandleUpdateItem } from "./handler/handle-update-item";
import { createHandleRemoveItem } from "./handler/handle-remove-item";
import { createHandleSubmit } from "./handler/handle-submit";

export function useCreateOrder() {
  const [loading, setLoading] = useState(false);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [selectedCustomerId, setSelectedCustomerId] = useState<string>("");
  const [orderItems, setOrderItems] = useState<OrderItem[]>([]);

  useEffect(() => {
    fetchData(setCustomers, setProducts);
  }, []);

  const handleAddItem = createHandleAddItem(products, orderItems, setOrderItems);
  const handleUpdateItem = createHandleUpdateItem(
    products,
    orderItems,
    setOrderItems
  );
  const handleRemoveItem = createHandleRemoveItem(orderItems, setOrderItems);
  const handleSubmit = createHandleSubmit(
    selectedCustomerId,
    orderItems,
    setLoading
  );

  const totalValue = orderItems.reduce(
    (sum, item) => sum + item.unitPriceAmount * item.quantity,
    0
  );

  return {
    loading,
    customers,
    products,
    selectedCustomerId,
    orderItems,
    totalValue,
    setSelectedCustomerId,
    handleAddItem,
    handleUpdateItem,
    handleRemoveItem,
    handleSubmit,
  };
}
