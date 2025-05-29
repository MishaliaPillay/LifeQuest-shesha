import React, { useState, useEffect } from "react";
import {
  CameraOutlined,
  DownloadOutlined,
  InboxOutlined,
  SoundOutlined,
  EyeOutlined,
} from "@ant-design/icons";
import {
  ComponentFactoryArguments,
  ConfigurableFormItem,
  IToolboxComponent,
  validateConfigurableComponentSettings,
} from "@shesha-io/reactjs";
import {
  Button,
  Typography,
  Divider,
  Space,
  Card,
  message,
  Upload,
  Spin,
  Input,
} from "antd";
import {
  INutritionAnalyzerProps,
  AnalysisResult,
} from "../../designer-components/food/types";
import { nutritionAnalyzerSettings } from "../../designer-components/food/settingsForm";
import {
  speakText,
  downloadReportAsPDF,
  validateImageFile,
} from "../../../../../utils/pdf";
import { analyzeFoodImage } from "../../../../../utils/gemini-service";
const { Title, Text, Paragraph } = Typography;
const { Dragger } = Upload;
const { TextArea } = Input;

export const NutritionAnalyzerUI: React.FC<INutritionAnalyzerProps> = (
  props
) => {
  const {
    title = "Nutrition Image Analyzer",
    enablePdfDownload = true,
    enableTextToSpeech = true,
    showImagePreview = true,
    showStructuredData = true,
    maxFileSize = 10,
    placeholder = "Provide additional context about the food image (e.g., meal type, dietary restrictions)",
    acceptedFileTypes = ".jpg,.jpeg,.png,.webp",
    analysisPrompt = "",
    customInstructions = "",
  } = props;

  const [selectedImage, setSelectedImage] = useState<File | null>(null);
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [prompt, setPrompt] = useState<string>("");
  const [analysis, setAnalysis] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [structuredData, setStructuredData] = useState<
    AnalysisResult["structured"] | null
  >(null);

  const handleImageChange = (info: any) => {
    const { status } = info.file;

    if (status === "done") {
      const file = info.file.originFileObj as File;

      if (validateImageFile(file, maxFileSize)) {
        setSelectedImage(file);

        if (showImagePreview) {
          const reader = new FileReader();
          reader.onload = () => setImagePreview(reader.result as string);
          reader.readAsDataURL(file);
        }

        message.success(`${info.file.name} uploaded successfully`);
      }
    } else if (status === "error") {
      message.error(`${info.file.name} upload failed`);
    }
  };

  const handleAnalyze = async () => {
    if (!selectedImage) {
      message.warning("Please upload an image first");
      return;
    }

    setIsLoading(true);
    setAnalysis("");
    setStructuredData(null);

    try {
      const combinedPrompt = [analysisPrompt, prompt, customInstructions]
        .filter(Boolean)
        .join(" ");

      const result = await analyzeFoodImage(selectedImage, combinedPrompt);

      setAnalysis(result.fullText);
      if (showStructuredData) setStructuredData(result.structured);

      message.success("Analysis completed successfully");
    } catch (error) {
      message.error(error instanceof Error ? error.message : "Analysis failed");
    } finally {
      setIsLoading(false);
    }
  };

  const uploadProps = {
    name: "file",
    multiple: false,
    accept: acceptedFileTypes || "image/*",
    onChange: handleImageChange,
    beforeUpload: (file: File) =>
      validateImageFile(file, maxFileSize) || Upload.LIST_IGNORE,
    showUploadList: false,
  };

  return (
    <ConfigurableFormItem model={props}>
      {(value, onChange) => {
        useEffect(() => {
          if (analysis && structuredData && typeof onChange === "function") {
            onChange({
              analysis,
              structuredData,
              imageFile: selectedImage?.name,
              timestamp: new Date().toISOString(),
            });
          }
          // We intentionally omit onChange in deps to avoid infinite loops
          // eslint-disable-next-line react-hooks/exhaustive-deps
        }, [analysis, structuredData, selectedImage]);

        return (
          <div
            style={{
              padding: "16px",
              border: "1px solid #d9d9d9",
              borderRadius: "6px",
            }}
          >
            {title && (
              <Title
                level={3}
                style={{ marginBottom: 16, textAlign: "center" }}
              >
                {title}
              </Title>
            )}

            <Divider />

            <Dragger {...uploadProps} style={{ marginBottom: 16 }}>
              <p className="ant-upload-drag-icon">
                <InboxOutlined />
              </p>
              <p className="ant-upload-text">Click or drag file to upload</p>
              <p className="ant-upload-hint">
                Support for single image upload only (Max: {maxFileSize}MB)
              </p>
            </Dragger>

            <TextArea
              value={prompt}
              onChange={(e) => setPrompt(e.target.value)}
              placeholder={placeholder}
              rows={3}
              style={{ marginBottom: 16 }}
            />

            <Button
              type="primary"
              onClick={handleAnalyze}
              disabled={isLoading || !selectedImage}
              size="large"
              block
              style={{ marginBottom: 16 }}
            >
              {isLoading ? <Spin size="small" /> : "Analyze Image"}
            </Button>

            {showImagePreview && imagePreview && (
              <Card
                title={
                  <>
                    <EyeOutlined /> Image Preview
                  </>
                }
                style={{ marginBottom: 16 }}
                bodyStyle={{ textAlign: "center" }}
              >
                <img
                  src={imagePreview}
                  alt="Preview"
                  style={{
                    maxWidth: "100%",
                    maxHeight: 300,
                    objectFit: "contain",
                    borderRadius: 4,
                  }}
                />
              </Card>
            )}

            {analysis && (
              <Card title="Analysis Results" style={{ marginBottom: 16 }}>
                <Paragraph style={{ whiteSpace: "pre-wrap" }}>
                  {analysis}
                </Paragraph>

                <Divider />

                <Text type="secondary" style={{ fontSize: 12 }}>
                  <strong>Disclaimer:</strong> This analysis is for
                  informational purposes only and does not constitute
                  professional nutritional advice.
                </Text>

                <div style={{ marginTop: 16 }}>
                  <Space>
                    {enablePdfDownload && (
                      <Button
                        type="primary"
                        icon={<DownloadOutlined />}
                        onClick={() =>
                          downloadReportAsPDF(
                            analysis,
                            imagePreview,
                            undefined,
                            title
                          )
                        }
                      >
                        Download PDF
                      </Button>
                    )}

                    {enableTextToSpeech && (
                      <Button
                        icon={<SoundOutlined />}
                        onClick={() => speakText(analysis)}
                      >
                        Read Aloud
                      </Button>
                    )}
                  </Space>
                </div>
              </Card>
            )}

            {showStructuredData && structuredData && (
              <Card title="Summary" size="small">
                <Space direction="vertical" style={{ width: "100%" }}>
                  <Text>
                    <strong>Total Calories:</strong>{" "}
                    {structuredData.totalCalories}
                  </Text>
                  <Text>
                    <strong>Health Score:</strong> {structuredData.healthScore}
                    /10
                  </Text>
                  <Text>
                    <strong>Food Items:</strong>{" "}
                    {structuredData.foodItems.join(", ")}
                  </Text>
                </Space>
              </Card>
            )}
          </div>
        );
      }}
    </ConfigurableFormItem>
  );
};

const NutritionAnalyzerComponent: IToolboxComponent<INutritionAnalyzerProps> = {
  type: "nutritionAnalyzer",
  name: "Nutrition Analyzer",
  icon: <CameraOutlined />,

  Factory: ({ model }: ComponentFactoryArguments<INutritionAnalyzerProps>) => (
    <NutritionAnalyzerUI {...model} />
  ),

  initModel: (model) => ({
    ...model,
    title: "Nutrition Image Analyzer",
    enablePdfDownload: true,
    enableTextToSpeech: true,
    showImagePreview: true,
    showStructuredData: true,
    maxFileSize: 10,
    placeholder:
      "Provide additional context about the food image (e.g., meal type, dietary restrictions)",
    acceptedFileTypes: ".jpg,.jpeg,.png,.webp",
    analysisPrompt:
      "Analyze this food image for nutritional content, calories, and health recommendations.",
    customInstructions: "",
  }),

  settingsFormMarkup: nutritionAnalyzerSettings,
  validateSettings: (model) =>
    validateConfigurableComponentSettings(nutritionAnalyzerSettings, model),

  isInput: false,
};

export default NutritionAnalyzerComponent;
