"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../atom/card";
import { Button } from "../../atom/button";
import { RefreshCw, Package } from "lucide-react";
import { formatCurrency } from "../../../lib/util/format-currency";
import { useProductList } from "../../../lib/hook/use-product-list";
import { ProductListProps } from "./product-list.props";
import { PaginationFooter } from "../../molecule/pagination-footer";
import { formatDate } from "@/src/lib/util/format-date";

export function ProductList({ refreshTrigger }: ProductListProps) {
  const {
    currentPage,
    setCurrentPage,
    totalPages,
    products,
    loading,
    fetchProducts,
  } = useProductList(refreshTrigger);

  if (loading) {
    return (
      <Card>
        <CardContent className="py-12">
          <div className="flex items-center justify-center gap-2 text-muted-foreground">
            <RefreshCw className="h-4 w-4 animate-spin" />
            Loading products...
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center justify-between">
          <div>
            <CardTitle>Products</CardTitle>
            <CardDescription>Manage product catalog</CardDescription>
          </div>
          <Button
            onClick={fetchProducts}
            variant="outline"
            size="sm"
            className="gap-2 bg-transparent"
          >
            <RefreshCw className="h-4 w-4" />
            Refresh
          </Button>
        </div>
      </CardHeader>
      <CardContent>
        {products.length === 0 ? (
          <div className="py-12 text-center text-muted-foreground">
            No products found. Create your first product to get started.
          </div>
        ) : (
          <>
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              {products.map((product) => (
                <div
                  key={product.id}
                  className="rounded-lg border border-border bg-card p-4 transition-colors hover:bg-accent/50"
                >
                  <div className="mb-3 flex items-start justify-between">
                    <div className="flex-1">
                      <h3 className="font-semibold text-foreground">
                        {product.name}
                      </h3>
                      <p className="text-xs text-muted-foreground">
                        ID: #{product.id}
                      </p>
                    </div>
                    <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
                      <Package className="h-5 w-5 text-primary" />
                    </div>
                  </div>
                  <div className="flex items-center justify-between pt-3 border-t border-border">
                    <span className="text-lg font-semibold text-primary">
                      {formatCurrency(product.price, product.currency)}
                    </span>
                    <span className="text-xs text-muted-foreground">
                      {formatDate(product.createdAt)}
                    </span>
                  </div>
                </div>
              ))}
            </div>
            {totalPages > 1 && (
              <PaginationFooter
                currentPage={currentPage}
                setCurrentPage={setCurrentPage}
                totalPages={totalPages}
              />
            )}
          </>
        )}
      </CardContent>
    </Card>
  );
}
