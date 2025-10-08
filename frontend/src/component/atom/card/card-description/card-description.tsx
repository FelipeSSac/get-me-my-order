import { cx } from "class-variance-authority";
import { CardDescriptionProps } from "./card-description.props";

export function CardDescription({ className, ...props }: CardDescriptionProps) {
  return (
    <div
      data-slot="card-description"
      className={cx("text-muted-foreground text-sm", className)}
      {...props}
    />
  );
}
