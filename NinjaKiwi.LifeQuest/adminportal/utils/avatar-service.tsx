import { GoogleGenerativeAI } from "@google/generative-ai";

const apiKey = "AIzaSyDLdNq_SPRv8p9iXVcGKo7gEUZDELclcwg";
const genAI = new GoogleGenerativeAI(apiKey || "");

async function getImageData(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => {
      if (typeof reader.result === "string") {
        resolve(reader.result.split(",")[1]); // get base64 data without prefix
      } else {
        reject("Invalid file data");
      }
    };
    reader.onerror = reject;
    reader.readAsDataURL(file);
  });
}

export const analyzePersonImage = async (imageFile: File): Promise<string> => {
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
      You are an AI assistant that analyzes a person's photo to create a detailed description for generating a cute chibi-style 3D character avatar.

      Please describe the person's appearance with these points:
      - Approximate age and gender
      - Hairstyle and hair color
      - Clothing style and colors
      - Facial features (eye shape and color, nose, mouth)
      - Expression or mood shown
      - Distinctive features or accessories (glasses, freckles, earrings, hats, scars, etc.)
      
      Make the description suitable for creating a chibi 3D avatar:
      - Emphasize cute and simplified features typical of chibi style
      - Mention colors clearly
      - Keep it concise but specific enough to capture the person's unique look

      Do NOT include assumptions beyond what is clearly visible in the photo.

      Return ONLY the descriptive text to be used as a character design prompt without extra commentary.
    `;

    const result = await model.generateContent({
      contents: [
        {
          role: "user",
          parts: [{ text: promptText }, imagePart],
        },
      ],
    });

    const description = result.response.text().trim();
    return description; // ✅ Just the string
  } catch (error: unknown) {
    console.error("Error analyzing image:", error);
    const errorMessage =
      error instanceof Error ? error.message : "Unknown error";
    throw new Error(`Failed to analyze image: ${errorMessage}`);
  }
};
