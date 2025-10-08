import { ArrowLeft } from "lucide-react";
import { Button } from "../../atom/button";
import { CardDescription, CardHeader, CardTitle } from "../../atom/card";
import { FormHeaderProps } from "./form-header.props";

export function FormHeader({ title, description, onBack }: FormHeaderProps) {
  return (
    <CardHeader>
      <div className="flex items-center gap-3">
        <Button onClick={onBack} variant="ghost" size="sm" className="gap-2">
          <ArrowLeft className="h-4 w-4" />
          Back
        </Button>
      </div>
      <CardTitle className="mt-4">{title}</CardTitle>
      <CardDescription>{description}</CardDescription>
    </CardHeader>
  );
}
