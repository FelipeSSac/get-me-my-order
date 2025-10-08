import { SubView } from "@/src/component/organisms/view-content";
import { Dispatch, SetStateAction } from "react";

export function createHandleBack(
  setSubView: Dispatch<SetStateAction<SubView>>,
  setSelectedId: Dispatch<SetStateAction<string | null>>
) {
  return () => {
    setSubView("list");
    setSelectedId(null);
  };
}
