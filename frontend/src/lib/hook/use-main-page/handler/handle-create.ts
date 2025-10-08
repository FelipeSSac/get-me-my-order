import { SubView } from "@/src/component/organisms/view-content";
import { Dispatch, SetStateAction } from "react";

export function createHandleCreate(
  setSubView: Dispatch<SetStateAction<SubView>>,
  setSelectedId: Dispatch<SetStateAction<string | null>>
) {
  return () => {
    setSubView("create");
    setSelectedId(null);
  };
}
