import { cx } from "class-variance-authority";
import { CardContentProps } from "./card-content.props";

export function CardContent({ className, ...props }: CardContentProps) {
  return (
    <div
      data-slot="card-content"
      className={cx("px-6", className)}
      {...props}
    />
  );
}
