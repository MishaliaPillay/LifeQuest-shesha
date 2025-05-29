import React from "react";
import {
  IConfigurableFormComponent,
  IToolboxComponent,
  validateConfigurableComponentSettings,
} from "@shesha-io/reactjs";
import { PersonAnalyzerUI } from "../../components/avatar";
import { personAnalyzerSettings } from "./settingsForm";
import { BookOutlined } from "@ant-design/icons";

interface IImageScanAvatar extends IConfigurableFormComponent {}

const ImageScanComponent: IToolboxComponent<IImageScanAvatar> = {
  type: "ImageScan",
  isInput: false,
  isOutput: true,
  name: "ImageScan",
  icon: <BookOutlined />,

  Factory: ({ model }) => {
    return <PersonAnalyzerUI title={""} enablePdfDownload={false} enableTextToSpeech={false} showImagePreview={false} showStructuredData={false} maxFileSize={0} placeholder={""} acceptedFileTypes={""} analysisPrompt={""} customInstructions={""} {...model} />;
  },

  settingsFormMarkup: () => personAnalyzerSettings,
  validateSettings: (model) =>
    validateConfigurableComponentSettings(personAnalyzerSettings, model),
};

export default ImageScanComponent;
