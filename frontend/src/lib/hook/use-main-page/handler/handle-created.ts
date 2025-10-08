import { SubView } from "@/src/component/organisms/view-content";
import { Dispatch, SetStateAction } from "react";

export function createHandleCreated(
  setSubView: Dispatch<SetStateAction<SubView>>,
  setRefreshTrigger: Dispatch<SetStateAction<number>>
) {
  return () => {
    setSubView("list");
    setRefreshTrigger((prev) => prev + 1);
  };
}
