import { ComponentProps } from "react";
import { type VariantProps } from "class-variance-authority";
import { buttonVariants } from "./button.variant";

export type ButtonProps = ComponentProps<"button"> &
  VariantProps<typeof buttonVariants> & {
    asChild?: boolean;
  };
