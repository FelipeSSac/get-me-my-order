import { View } from "@/src/component/organisms/view-content";

export function getCreateButtonLabel(currentView: View): string {
  switch (currentView) {
    case "orders":
      return "New Order";
    case "customers":
      return "New Customer";
    case "products":
      return "New Product";
  }
}
