"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../atom/card";
import { Button } from "../../atom/button";
import { RefreshCw, Mail } from "lucide-react";
import { formatDate } from "../../../lib/util/format-date";
import { useCustomerList } from "../../../lib/hook/use-customer-list";
import { CustomerListProps } from "./customer-list.props";
import { PaginationFooter } from "../../molecule/pagination-footer";

export function CustomerList({ refreshTrigger }: CustomerListProps) {
  const {
    currentPage,
    setCurrentPage,
    totalPages,
    customers,
    loading,
    fetchCustomers,
  } = useCustomerList(refreshTrigger);

  if (loading) {
    return (
      <Card>
        <CardContent className="py-12">
          <div className="flex items-center justify-center gap-2 text-muted-foreground">
            <RefreshCw className="h-4 w-4 animate-spin" />
            Loading customers...
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
            <CardTitle>Customers</CardTitle>
            <CardDescription>Manage customer information</CardDescription>
          </div>
          <Button
            onClick={fetchCustomers}
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
        {customers.length === 0 ? (
          <div className="py-12 text-center text-muted-foreground">
            No customers found. Create your first customer to get started.
          </div>
        ) : (
          <>
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              {customers.map((customer) => (
                <div
                  key={customer.id}
                  className="rounded-lg border border-border bg-card p-4 transition-colors hover:bg-accent/50"
                >
                  <div className="mb-3">
                    <h3 className="font-semibold text-foreground">{`${customer.firstName} ${customer.lastName}`}</h3>
                    <p className="text-xs text-muted-foreground">
                      ID: #{customer.id}
                    </p>
                  </div>
                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <Mail className="h-4 w-4" />
                      <span className="truncate">{customer.email}</span>
                    </div>
                  </div>
                  <div className="mt-3 pt-3 border-t border-border">
                    <p className="text-xs text-muted-foreground">
                      Created {formatDate(customer.createdAt)}
                    </p>
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
