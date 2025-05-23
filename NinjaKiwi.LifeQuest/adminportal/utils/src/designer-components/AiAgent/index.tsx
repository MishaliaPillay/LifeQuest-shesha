import React from "react";
//import AiAgent from "../../components/AiAgent";
import {
  FormMarkup,
  IConfigurableFormComponent,
  IToolboxComponent,
  validateConfigurableComponentSettings,
} from "@shesha-io/reactjs";
import AiAgent from "../../components/AiAgent";
import { getSettings } from "./settingsForm";
import { BookOutlined } from "@ant-design/icons";
import { IConfigurableActionArgumentsFormFactory } from "@shesha-io/reactjs/dist/interfaces/configurableAction";

interface IAiAgent extends IConfigurableFormComponent {}

const AiAgentComponent: IToolboxComponent<IAiAgent> = {
  type: "aiagent", //
  isInput: false,
  isOutput: true,
  name: "AiAgent", //
  icon: <BookOutlined />, //
  Factory: ({ model }) => {
    return <AiAgent />;
  },
  settingsFormMarkup: (data) => getSettings(),
  validateSettings: (model) =>
    validateConfigurableComponentSettings(getSettings(), model),
};

export default AiAgentComponent;
