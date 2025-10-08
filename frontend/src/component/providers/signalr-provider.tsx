"use client";

import { useEffect } from "react";
import { signalRService } from "../../lib/service/signalr-service";

export function SignalRProvider({ children }: { children: React.ReactNode }) {
  useEffect(() => {
    const initConnection = async () => {
      try {
        await signalRService.connect();
      } catch (error) {
        console.warn(
          `[SignalRProvider] Initial connection failed, will retry: ${JSON.stringify(
            error
          )}`
        );
      }
    };

    initConnection();

    return () => {
      signalRService.disconnect();
    };
  }, []);

  return <>{children}</>;
}
