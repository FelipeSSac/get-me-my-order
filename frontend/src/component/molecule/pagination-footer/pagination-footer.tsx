import { ChevronLeft, ChevronRight } from "lucide-react";
import { Button } from "../../atom/button";
import { PaginationFooterProps } from "./pagination-footer.props";

export function PaginationFooter({
  totalPages,
  currentPage,
  setCurrentPage,
}: PaginationFooterProps) {
  return (
    <div className="flex items-center justify-between pt-4 border-t border-border mt-4">
      <p className="text-sm text-muted-foreground">
        Showing {currentPage} of {totalPages} page items
      </p>
      <div className="flex gap-2">
        <Button
          onClick={() => setCurrentPage((prev) => Math.max(1, prev - 1))}
          disabled={currentPage === 1}
          variant="outline"
          size="sm"
          className="gap-2 bg-transparent"
        >
          <ChevronLeft className="h-4 w-4" />
          Previous
        </Button>
        <div className="flex items-center gap-1">
          {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
            <Button
              key={page}
              onClick={() => setCurrentPage(page)}
              variant={currentPage === page ? "default" : "outline"}
              size="sm"
              className={
                currentPage === page
                  ? "min-w-[2.5rem]"
                  : "min-w-[2.5rem] bg-transparent"
              }
            >
              {page}
            </Button>
          ))}
        </div>
        <Button
          onClick={() =>
            setCurrentPage((prev) => Math.min(totalPages, prev + 1))
          }
          disabled={currentPage === totalPages}
          variant="outline"
          size="sm"
          className="gap-2 bg-transparent"
        >
          Next
          <ChevronRight className="h-4 w-4" />
        </Button>
      </div>
    </div>
  );
}
