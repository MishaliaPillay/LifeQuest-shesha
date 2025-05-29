import { IConfigurableFormComponent } from '@shesha-io/reactjs';

export interface IPersonAnalyzerProps extends IConfigurableFormComponent {
  title: string;
  enablePdfDownload: boolean;
  enableTextToSpeech: boolean;
  showImagePreview: boolean;
  showStructuredData: boolean;
  maxFileSize: number; // in MB
  placeholder: string;
  acceptedFileTypes: string;
  analysisPrompt: string;
  customInstructions: string;
  playerId: string; // <--- Add playerId to the interface
}

export interface StructuredAnalysisData {
  totalCalories: number;
  healthScore: number;
  foodItems: string[];
}

export interface AnalysisResult {
  fullText: string;
  structured: StructuredAnalysisData;
}