//name the dropdown

import { IToolboxComponentGroup } from "@shesha-io/reactjs";
import ImageScanAvatar from "../components/avatar";
import ImageScan from "../components/food";
export const formDesignerComponents: IToolboxComponentGroup[] = [
  {
    name: "Gemini",
    components: [ImageScan, ImageScanAvatar],
    visible: true,
  },
];
