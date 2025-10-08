import { cx } from "class-variance-authority";
import { CardTitleProps } from "./card-title.props";

export function CardTitle({ className, ...props }: CardTitleProps) {
  return (
    <div
      data-slot="card-title"
      className={cx("leading-none font-semibold", className)}
      {...props}
    />
  );
}
