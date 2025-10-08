import { cx } from "class-variance-authority";
import { CardProps } from "./card.props";

export function Card({ className, ...props }: CardProps) {
  return (
    <div
      data-slot="card"
      className={cx(
        "bg-card text-card-foreground flex flex-col gap-6 rounded-xl border py-6 shadow-sm",
        className
      )}
      {...props}
    />
  );
}
