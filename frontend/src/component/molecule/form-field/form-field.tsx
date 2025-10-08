import { Label } from "../../atom/label";
import { FormFieldProps } from "./form-field.props";

export function FormField({ label, htmlFor, children }: FormFieldProps) {
  return (
    <div className="space-y-2">
      <Label htmlFor={htmlFor}>{label}</Label>
      {children}
    </div>
  );
}
