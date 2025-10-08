import { NavigationTab } from "@/src/component/molecule/navigation-tabs";
import { Package, Users, ShoppingCart } from "lucide-react";

export function getNavItems(): NavigationTab[] {
  return [
    { id: "orders", label: "Orders", icon: ShoppingCart },
    { id: "customers", label: "Customers", icon: Users },
    { id: "products", label: "Products", icon: Package },
  ];
}
