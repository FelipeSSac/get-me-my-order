import { Loader2 } from "lucide-react";
import { Button } from "../../atom/button";
import { FormActionsProps } from "./form-actions.props";

export function FormActions({
  submitLabel,
  loadingLabel,
  isLoading,
  isDisabled = false,
  onCancel,
}: FormActionsProps) {
  return (
    <div className="flex gap-3 pt-4">
      <Button
        type="submit"
        disabled={isLoading || isDisabled}
        className="flex-1"
      >
        {isLoading ? (
          <>
            <Loader2 className="mr-2 h-4 w-4 animate-spin" />
            {loadingLabel}
          </>
        ) : (
          submitLabel
        )}
      </Button>
      <Button
        type="button"
        variant="outline"
        onClick={onCancel}
        disabled={isLoading}
      >
        Cancel
      </Button>
    </div>
  );
}
