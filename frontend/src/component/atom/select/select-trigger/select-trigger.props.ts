import * as SelectPrimitive from "@radix-ui/react-select";
import { ComponentProps } from "react";

export type SelectTriggerProps = ComponentProps<typeof SelectPrimitive.Trigger> & {
  size?: "sm" | "default";
};
