import { Slot } from "@radix-ui/react-slot";
import { cx } from "class-variance-authority";
import { buttonVariants } from "./button.variant";
import { ButtonProps } from "./button.props";

export function Button({
  className,
  variant,
  size,
  asChild = false,
  ...props
}: ButtonProps) {
  const Comp = asChild ? Slot : "button";

  return (
    <Comp
      data-slot="button"
      className={cx(buttonVariants({ variant, size, className }))}
      {...props}
    />
  );
}
