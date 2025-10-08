"use client";

import * as SelectPrimitive from "@radix-ui/react-select";
import { SelectValueProps } from "./select-value.props";

export function SelectValue({ ...props }: SelectValueProps) {
  return <SelectPrimitive.Value data-slot="select-value" {...props} />;
}
