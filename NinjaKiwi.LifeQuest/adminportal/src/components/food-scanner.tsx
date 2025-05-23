"use client";
import { useState } from "react";
import { analyzeFoodImage } from "../../utils/gemini-service";

import jsPDF from "jspdf";
import {
  Button,
  Typography,
  Divider,
  Space,
  Card,
  message,
  Upload,
  Spin,
} from "antd";
import {
  DownloadOutlined,
  InboxOutlined,
  SoundOutlined,
} from "@ant-design/icons";
import styles from "./HealthAnalysisComponent.module.css";
const { Title, Text, Paragraph } = Typography;
const { Dragger } = Upload;

// Reading the text using SpeechSynthesis API
const speakText = (text) => {
  const utterance = new SpeechSynthesisUtterance(text);
  window.speechSynthesis.speak(utterance);
};

// Generating and downloading the analysis report as PDF
const downloadReportAsPDF = (
  content,
  imageData,
  fileName = "AI_Food_Report.pdf"
) => {
  const doc = new jsPDF();

  // Add title
  doc.setFontSize(16);
  doc.setFont("helvetica", "bold");
  doc.text("Nutritional Image Analysis Report", 10, 20);

  // Add date
  doc.setFontSize(10);
  doc.setFont("helvetica", "normal");
  doc.text(`Generated on: ${new Date().toLocaleString()}`, 10, 30);

  // Add divider
  doc.line(10, 35, 200, 35);

  // Embed image if available
  if (imageData) {
    doc.addImage(imageData, "JPEG", 10, 40, 60, 60); // x, y, width, height
  }

  // Add content below the image
  const textStartY = imageData ? 110 : 45;

  doc.setFontSize(12);
  doc.setFont("helvetica", "normal");
  const lines = doc.splitTextToSize(content, 180);
  doc.text(lines, 10, textStartY);

  // Add disclaimer at the bottom
  doc.setFontSize(10);
  doc.setFont("helvetica", "italic");
  doc.text(
    "Disclaimer: This analysis is for informational purposes only.",
    10,
    doc.internal.pageSize.height - 20
  );

  doc.save(fileName);
};

export default function HealthAnalysisComponent() {
  const [selectedImage, setSelectedImage] = useState(null);
  const [imagePreview, setImagePreview] = useState(null);
  const [prompt, setPrompt] = useState("");
  const [analysis, setAnalysis] = useState("");

  const [isLoading, setIsLoading] = useState(false);
  const [structuredData, setStructuredData] = useState<{
    totalCalories: number;
    healthScore: number;
    foodItems: string[];
  } | null>(null);

  const handleImageChange = (info) => {
    const { status } = info.file;

    if (status === "done") {
      const file = info.file.originFileObj;
      setSelectedImage(file);

      // Creating a preview
      const reader = new FileReader();
      reader.onload = () => setImagePreview(reader.result);
      reader.readAsDataURL(file);
      message.success(`${info.file.name} file uploaded successfully.`);
    } else if (status === "error") {
      message.error(`${info.file.name} file upload failed.`);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    setAnalysis("");
    setStructuredData(null); // Reset structured data

    if (!selectedImage) {
      return;
    }

    setIsLoading(true);
    try {
      const result = await analyzeFoodImage(selectedImage, prompt);
      setAnalysis(result.fullText); // this is a string
      setStructuredData(result.structured);
      setIsLoading(false);
    } catch (err) {
      const errorMessage =
        err instanceof Error ? err.message : "An unknown error occurred";
      message.error(errorMessage);
      setIsLoading(false);
    }
  };

  const uploadProps = {
    name: "file",
    multiple: false,
    accept: "image/*",
    onChange: handleImageChange,
    beforeUpload: (file) => {
      const isImage = file.type.startsWith("image/");
      if (!isImage) {
        message.error(`${file.name} is not an image file`);
      }
      return isImage || Upload.LIST_IGNORE;
    },
  };

  return (
    <div className={styles.container}>
      <Title level={2} className={styles.title}>
        Nutrition Image Analyzer
      </Title>
      <Divider />
      <Dragger {...uploadProps} className={styles.upload}>
        <p className="ant-upload-drag-icon">
          <InboxOutlined />
        </p>
        <p className="ant-upload-text">
          Click or drag file to this area to upload
        </p>
        <p className="ant-upload-hint">
          Support for a single image upload only
        </p>
      </Dragger>

      <div className={styles.form}>
        {/* ... */}
        <textarea
          value={prompt}
          onChange={(e) => setPrompt(e.target.value)}
          className={styles.textarea}
          placeholder="Provide additional context about the food image (e.g., meal type, dietary restrictions)"
        />

        <Button
          type="primary"
          onClick={handleSubmit}
          disabled={isLoading || !selectedImage}
          className={styles.button}
          size="large"
        >
          {isLoading ? <Spin size="small" /> : "Analyze Image"}
        </Button>
      </div>

      {/* image preview */}
      {imagePreview && (
        <div>
          <Title level={5} className={styles.previewContainer}>
            Image Preview
          </Title>
          <Card
            style={{ marginBottom: "20px" }}
            bodyStyle={{ display: "flex", justifyContent: "center" }}
          >
            <img
              src={imagePreview}
              alt="Preview"
              className={styles.previewImage}
            />
          </Card>
        </div>
      )}

      {/* analysis */}
      {analysis && (
        <Card className={styles.analysisContainer}>
          <Title level={3} className={styles.analysisTitle}>
            Nutritional Analysis Results
          </Title>
          <Divider />

          <div className={styles.analysisContent}>
            <Paragraph className={styles.analysisText}>{analysis}</Paragraph>
          </div>

          <Divider />

          <div className={styles.disclaimer}>
            <Text type="secondary" italic className={styles.disclaimerText}>
              <strong>Disclaimer:</strong> This analysis is for informational
              purposes only and does not constitute professional nutritional
              advice.
            </Text>
          </div>

          <Space className={styles.buttonContainer} size="middle">
            <Button
              type="primary"
              icon={<DownloadOutlined />}
              onClick={() => downloadReportAsPDF(analysis, imagePreview)}
              className={styles.button}
            >
              Download PDF
            </Button>
            <Button
              type="default"
              icon={<SoundOutlined />}
              onClick={() => speakText(analysis)}
              className={styles.button}
            >
              Text to Speech
            </Button>
          </Space>
        </Card>
      )}

      {/* structured data */}
      {structuredData && (
        <Card className={styles.analysisContainer}>
          <Title level={4}>AI Summary</Title>
          <Paragraph>
            <strong>Total Calories:</strong> {structuredData.totalCalories}
          </Paragraph>
          <Paragraph>
            <strong>Health Score:</strong> {structuredData.healthScore}/10
          </Paragraph>
          <Paragraph>
            <strong>Food Items:</strong> {structuredData.foodItems.join(", ")}
          </Paragraph>
        </Card>
      )}
    </div>
  );
}
