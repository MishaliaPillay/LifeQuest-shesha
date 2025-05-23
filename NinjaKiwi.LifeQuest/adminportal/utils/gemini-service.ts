import { GoogleGenerativeAI } from "@google/generative-ai";

// Initialize the API with your API key
const apiKey = "AIzaSyDLdNq_SPRv8p9iXVcGKo7gEUZDELclcwg";
const genAI = new GoogleGenerativeAI(apiKey || "");

// Get image as base64 data

export const getImageData = async (file: File): Promise<string> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onloadend = () => {
      // Extract base64 data part from the data URL
      const dataUrl = reader.result as string;
      const base64Data = dataUrl.split(",")[1];
      resolve(base64Data);
    };
    reader.onerror = reject;
    reader.readAsDataURL(file);
  });
};

// Analyze image using Gemini API
export const analyzeFoodImage = async (
  imageFile: File,
  contextPrompt: string
): Promise<{
  fullText: string;
  structured: {
    totalCalories: number;
    healthScore: number;
    foodItems: string[];
  } | null;
}> => {
  try {
    const model = genAI.getGenerativeModel({ model: "gemini-1.5-flash" });

    const imageData = await getImageData(imageFile);
    const imagePart = {
      inlineData: {
        data: imageData,
        mimeType: imageFile.type,
      },
    };

    const promptText = `
      You are a nutritional AI assistant.
      Analyze this image of food and provide:
      - A list of identified food items
      - Estimated calories per item and total
      - Macronutrient breakdown (if possible)
      - A health score from 1 to 10 based on general nutritional guidelines
      - Any health insights (e.g., "High in sugar", "Good protein source", etc.)
      - Recommendations for improvement if the meal is unbalanced

      Context: ${contextPrompt}

      Important:
      - Don't guess food if unclear
      - Be honest about uncertainty

      
    `;

    const result = await model.generateContent({
      contents: [
        {
          role: "user",
          parts: [{ text: promptText }, imagePart],
        },
      ],
    });

    const fullText = result.response.text();

    // Try to extract the JSON block from the result
    const jsonMatch = fullText.match(/\{[\s\S]*?\}/);
    let structured = null;

    if (jsonMatch) {
      try {
        structured = JSON.parse(jsonMatch[0]);
      } catch (jsonErr) {
        console.warn("Failed to parse structured JSON:", jsonErr);
      }
    }

    return { fullText, structured };
  } catch (error: unknown) {
    console.error("Error analyzing image:", error);

    const errorMessage =
      error instanceof Error ? error.message : "Unknown error";

    throw new Error(`Failed to analyze image: ${errorMessage}`);
  }
};
