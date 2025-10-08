import { Dispatch, SetStateAction } from "react";
import type { Customer } from "../../../type/customer";
import type { Product } from "../../../type/product";

export async function fetchData(
  setCustomers: Dispatch<SetStateAction<Customer[]>>,
  setProducts: Dispatch<SetStateAction<Product[]>>
) {
  // TODO: Replace with actual API calls
  const mockCustomers: Customer[] = [
    {
      id: "c3284b50-8617-4c1f-aeb5-b1734f01d35d",
      firstName: "João",
      lastName: "Silva",
      email: "joao.silva@gmail.com",
      createdAt: new Date(Date.now() - 86400000 * 30).toISOString(),
      updatedAt: new Date(Date.now() - 86400000 * 30).toISOString(),
    },
    {
      id: "c3284b51-8617-4c1f-aeb5-b1734f01d36e",
      firstName: "Maria",
      lastName: "Santos",
      email: "maria.santos@gmail.com",
      createdAt: new Date(Date.now() - 86400000 * 15).toISOString(),
      updatedAt: new Date(Date.now() - 86400000 * 15).toISOString(),
    },
  ];

  const mockProducts: Product[] = [
    {
      id: "501dc79c-2c26-4a83-9c83-b9588a021347",
      name: "Arroz",
      price: 4.6,
      currency: "BRL",
      createdAt: new Date(Date.now() - 86400000 * 15).toISOString(),
      updatedAt: new Date(Date.now() - 86400000 * 15).toISOString(),
    },
    {
      id: "7c97da4e-378b-4cbf-a221-1fed4d27484b",
      name: "Macarrao",
      price: 7.4,
      currency: "BRL",
      createdAt: new Date(Date.now() - 86400000 * 15).toISOString(),
      updatedAt: new Date(Date.now() - 86400000 * 15).toISOString(),
    },
    {
      id: "0199c0fa-0d2f-7469-a8b2-af7e9a086038",
      name: "Galinha Caipira",
      price: 1,
      currency: "BRL",
      createdAt: new Date(Date.now() - 86400000 * 15).toISOString(),
      updatedAt: new Date(Date.now() - 86400000 * 15).toISOString(),
    },
  ];

  setCustomers(mockCustomers);
  setProducts(mockProducts);
}
