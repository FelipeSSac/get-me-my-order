"use client";

import * as SelectPrimitive from "@radix-ui/react-select";
import { SelectProps } from "./select.props";

export function Select({ ...props }: SelectProps) {
  return <SelectPrimitive.Root data-slot="select" {...props} />;
}
