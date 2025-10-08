"use client";

import { PageHeader } from "../../molecule/page-header";
import { NavigationTabs } from "../../molecule/navigation-tabs";
import { ViewContent } from "../../organisms/view-content";
import { useMainPage } from "../../../lib/hook/use-main-page";

export function MainPage() {
  const {
    currentView,
    subView,
    selectedId,
    refreshTrigger,
    navItems,
    createButtonLabel,
    handleCreate,
    handleCreated,
    handleSelect,
    handleBack,
    handleViewChange,
  } = useMainPage();

  return (
    <div className="min-h-screen bg-background">
      <PageHeader
        title="Get Me My Order!"
        subtitle="Order Management System"
        showCreateButton={subView === "list"}
        createButtonLabel={createButtonLabel}
        onCreateClick={handleCreate}
      />

      <NavigationTabs
        tabs={navItems}
        activeTab={currentView}
        onTabChange={handleViewChange}
      />

      <ViewContent
        currentView={currentView}
        subView={subView}
        selectedId={selectedId}
        refreshTrigger={refreshTrigger}
        onCreated={handleCreated}
        onBack={handleBack}
        onSelect={handleSelect}
      />
    </div>
  );
}
