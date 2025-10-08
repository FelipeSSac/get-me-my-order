import { SubView, View } from "@/src/component/organisms/view-content";
import { Dispatch, SetStateAction } from "react";

export function createHandleViewChange(
  setCurrentView: Dispatch<SetStateAction<View>>,
  setSubView: Dispatch<SetStateAction<SubView>>,
  setSelectedId: Dispatch<SetStateAction<string | null>>
) {
  return (view: string) => {
    setCurrentView(view as View);
    setSubView("list");
    setSelectedId(null);
  };
}
