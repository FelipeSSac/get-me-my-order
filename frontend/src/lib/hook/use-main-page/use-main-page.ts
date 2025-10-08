import { useState } from "react";
import { createHandleCreate } from "./handler/handle-create";
import { createHandleCreated } from "./handler/handle-created";
import { createHandleSelect } from "./handler/handle-select";
import { createHandleBack } from "./handler/handle-back";
import { createHandleViewChange } from "./handler/handle-view-change";
import { getCreateButtonLabel } from "./handler/get-create-button-label";
import { getNavItems } from "./handler/get-nav-items";
import { SubView, View } from "@/src/component/organisms/view-content";

export function useMainPage() {
  const [currentView, setCurrentView] = useState<View>("orders");
  const [subView, setSubView] = useState<SubView>("list");
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handleCreate = createHandleCreate(setSubView, setSelectedId);
  const handleCreated = createHandleCreated(setSubView, setRefreshTrigger);
  const handleSelect = createHandleSelect(setSelectedId, setSubView);
  const handleBack = createHandleBack(setSubView, setSelectedId);
  const handleViewChange = createHandleViewChange(
    setCurrentView,
    setSubView,
    setSelectedId
  );

  return {
    currentView,
    subView,
    selectedId,
    refreshTrigger,
    navItems: getNavItems(),
    createButtonLabel: getCreateButtonLabel(currentView),
    handleCreate,
    handleCreated,
    handleSelect,
    handleBack,
    handleViewChange,
  };
}
