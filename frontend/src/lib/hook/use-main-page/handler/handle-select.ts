import { SubView } from "@/src/component/organisms/view-content";
import { Dispatch, SetStateAction } from "react";

export function createHandleSelect(
  setSelectedId: Dispatch<SetStateAction<string | null>>,
  setSubView: Dispatch<SetStateAction<SubView>>
) {
  return (id: string) => {
    setSelectedId(id);
    setSubView("detail");
  };
}
