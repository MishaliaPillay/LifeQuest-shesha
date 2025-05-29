import { DesignerToolbarSettings } from "@shesha-io/reactjs";
import { nanoid } from "nanoid";

export const nutritionAnalyzerSettings = new DesignerToolbarSettings()
  .addSectionSeparator({
    id: nanoid(),
    propertyName: "displaySection",
    parentId: "root",
    label: "Display Settings",
  })
  .addContextPropertyAutocomplete({
    id: nanoid(),
    propertyName: "propertyName",
    parentId: "root",
    label: "Property name",
    validate: { required: true },
  })
  .addTextField({
    id: nanoid(),
    propertyName: "title",
    parentId: "root",
    label: "Component Title",
    description: "Title displayed at the top of the component",
  })
  .addCheckbox({
    id: nanoid(),
    propertyName: "hideLabel",
    parentId: "root",
    label: "Hide Label",
  })
  .addSectionSeparator({
    id: nanoid(),
    propertyName: "featuresSection",
    parentId: "root",
    label: "Feature Settings",
  })
  .addCheckbox({
    id: nanoid(),
    propertyName: "enablePdfDownload",
    parentId: "root",
    label: "Enable PDF Download",
    description: "Allow users to download analysis as PDF",
  })
  .addCheckbox({
    id: nanoid(),
    propertyName: "enableTextToSpeech",
    parentId: "root",
    label: "Enable Text to Speech",
    description: "Allow users to hear the analysis",
  })
  .addCheckbox({
    id: nanoid(),
    propertyName: "showImagePreview",
    parentId: "root",
    label: "Show Image Preview",
    description: "Display uploaded image preview",
  })
  .addCheckbox({
    id: nanoid(),
    propertyName: "showStructuredData",
    parentId: "root",
    label: "Show Structured Data",
    description: "Display structured analysis summary",
  })
  .addSectionSeparator({
    id: nanoid(),
    propertyName: "uploadSection",
    parentId: "root",
    label: "Upload Settings",
  })
  .addNumberField({
    id: nanoid(),
    propertyName: "maxFileSize",
    parentId: "root",
    label: "Max File Size (MB)",
    description: "Enter a value between 1 and 100 MB",
  })
  .addTextField({
    id: nanoid(),
    propertyName: "acceptedFileTypes",
    parentId: "root",
    label: "Accepted File Types",
    description: "Comma-separated list of file types (e.g., .jpg,.png,.jpeg)",
  })
  .addSectionSeparator({
    id: nanoid(),
    propertyName: "aiSection",
    parentId: "root",
    label: "AI Analysis Settings",
  })
  .addTextArea({
    id: nanoid(),
    propertyName: "placeholder",
    parentId: "root",
    label: "Input Placeholder",
    description: "Placeholder text for the context input field",
  })
  .addTextArea({
    id: nanoid(),
    propertyName: "analysisPrompt",
    parentId: "root",
    label: "Default Analysis Prompt",
    description: "Default prompt sent to AI for analysis",
  })
  .addTextArea({
    id: nanoid(),
    propertyName: "customInstructions",
    parentId: "root",
    label: "Custom Instructions",
    description: "Additional instructions for the AI analysis",
  })
  .toJson();
