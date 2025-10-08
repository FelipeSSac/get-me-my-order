import { Plus, Package } from "lucide-react";
import { Button } from "../../atom/button";
import { PageHeaderProps } from "./page-header.props";

export function PageHeader({
  title,
  subtitle,
  showCreateButton,
  createButtonLabel,
  onCreateClick,
}: PageHeaderProps) {
  return (
    <header className="border-b border-border bg-card">
      <div className="container mx-auto px-4 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
              <Package className="h-5 w-5 text-primary" />
            </div>
            <div>
              <h1 className="text-xl font-semibold text-foreground">{title}</h1>
              <p className="text-sm text-muted-foreground">{subtitle}</p>
            </div>
          </div>
          {showCreateButton && (
            <Button onClick={onCreateClick} className="gap-2">
              <Plus className="h-4 w-4" />
              {createButtonLabel}
            </Button>
          )}
        </div>
      </div>
    </header>
  );
}
