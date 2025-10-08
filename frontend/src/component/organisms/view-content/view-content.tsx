import { CreateOrderForm } from "../create-order-form";
import { OrderDetail } from "../order-detail";
import { CreateCustomerForm } from "../create-customer-form";
import { CustomerList } from "../customer-list";
import { ProductList } from "../product-list";
import { CreateProductForm } from "../create-product-form";
import { OrderList } from "../order-list/order-list";
import { ViewContentProps } from "./view-content.props";

export function ViewContent({
  currentView,
  subView,
  selectedId,
  refreshTrigger,
  onCreated,
  onBack,
  onSelect,
}: ViewContentProps) {
  return (
    <main className="container mx-auto px-4 py-8">
      {currentView === "orders" && (
        <>
          {subView === "create" && (
            <div className="mx-auto max-w-3xl">
              <CreateOrderForm onSuccess={onCreated} onCancel={onBack} />
            </div>
          )}
          {subView === "detail" && selectedId && (
            <OrderDetail orderId={selectedId} onBack={onBack} />
          )}
          {subView === "list" && (
            <OrderList onOrderSelect={onSelect} refreshTrigger={refreshTrigger} />
          )}
        </>
      )}

      {currentView === "customers" && (
        <>
          {subView === "create" && (
            <div className="mx-auto max-w-2xl">
              <CreateCustomerForm onSuccess={onCreated} onCancel={onBack} />
            </div>
          )}
          {subView === "list" && (
            <CustomerList refreshTrigger={refreshTrigger} />
          )}
        </>
      )}

      {currentView === "products" && (
        <>
          {subView === "create" && (
            <div className="mx-auto max-w-2xl">
              <CreateProductForm onSuccess={onCreated} onCancel={onBack} />
            </div>
          )}
          {subView === "list" && (
            <ProductList refreshTrigger={refreshTrigger} />
          )}
        </>
      )}
    </main>
  );
}
