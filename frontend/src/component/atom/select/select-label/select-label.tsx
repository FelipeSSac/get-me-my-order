"use client";

import * as SelectPrimitive from "@radix-ui/react-select";
import { cx } from "class-variance-authority";
import { SelectLabelProps } from "./select-label.props";

export function SelectLabel({ className, ...props }: SelectLabelProps) {
  return (
    <SelectPrimitive.Label
      data-slot="select-label"
      className={cx("text-muted-foreground px-2 py-1.5 text-xs", className)}
      {...props}
    />
  );
}
