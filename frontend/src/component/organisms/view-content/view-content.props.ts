export type View = "orders" | "customers" | "products";
export type SubView = "list" | "create" | "detail";

export interface ViewContentProps {
  currentView: View;
  subView: SubView;
  selectedId: string | null;
  refreshTrigger: number;
  onCreated: () => void;
  onBack: () => void;
  onSelect: (id: string) => void;
}
