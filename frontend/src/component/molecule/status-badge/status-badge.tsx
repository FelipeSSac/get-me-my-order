import { Clock, Loader2, CheckCircle2 } from "lucide-react";
import { cx } from "class-variance-authority";
import { StatusBadgeProps } from "./status-badge.props";

export function StatusBadge({ status, size = "sm" }: StatusBadgeProps) {
  const config = {
    Pending: {
      label: "Pending",
      icon: Clock,
      className: "bg-warning/10 text-warning border-warning/20",
    },
    Processing: {
      label: "Processing",
      icon: Loader2,
      className: "bg-processing/10 text-processing border-processing/20",
    },
    Done: {
      label: "Completed",
      icon: CheckCircle2,
      className: "bg-success/10 text-success border-success/20",
    },
  };

  const { label, icon: Icon, className } = config[status];
  const isProcessing = status === "Processing";

  return (
    <div
      className={cx(
        "inline-flex items-center gap-1.5 rounded-full border px-2.5 py-1 font-medium",
        size === "lg" ? "text-sm" : "text-xs",
        className
      )}
    >
      <Icon className={cx("h-3 w-3", isProcessing && "animate-spin")} />
      {label}
    </div>
  );
}
