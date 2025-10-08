import type { LucideIcon } from "lucide-react";

export interface NavigationTab {
  id: string;
  label: string;
  icon: LucideIcon;
}

export interface NavigationTabsProps {
  tabs: NavigationTab[];
  activeTab: string;
  onTabChange: (tabId: string) => void;
}
