import React from "react";
import {
  IConfigurableFormComponent,
  IToolboxComponent,
  validateConfigurableComponentSettings,
} from "@shesha-io/reactjs";
import { NutritionAnalyzerUI } from "../../components/food";
import { nutritionAnalyzerSettings } from "./settingsForm";
import { BookOutlined } from "@ant-design/icons";

interface IImageScan extends IConfigurableFormComponent {}

const ImageScanComponent: IToolboxComponent<IImageScan> = {
  type: "ImageScan",
  isInput: false,
  isOutput: true,
  name: "ImageScan",
  icon: <BookOutlined />,

  Factory: ({ model }) => {
    return (
      <NutritionAnalyzerUI
        title={""}
        enablePdfDownload={false}
        enableTextToSpeech={false}
        showImagePreview={false}
        showStructuredData={false}
        maxFileSize={0}
        placeholder={""}
        acceptedFileTypes={""}
        analysisPrompt={""}
        customInstructions={""}
        {...model}
      />
    );
  },

  settingsFormMarkup: () => nutritionAnalyzerSettings,
  validateSettings: (model) =>
    validateConfigurableComponentSettings(nutritionAnalyzerSettings, model),
};

export default ImageScanComponent;
