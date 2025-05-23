import { IToolboxComponentGroup } from "@shesha-io/reactjs";
import AiAgentComponent from "./AiAgent";

export const formDesignerComponents: IToolboxComponentGroup[] = [
  {
    name: "Phoenix",
    components: [AiAgentComponent],
    visible: true,
  },
];
