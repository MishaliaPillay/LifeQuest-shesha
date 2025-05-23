import jsPDF from 'jspdf';
import { message } from 'antd';

export const speakText = (text: string): void => {
  if ('speechSynthesis' in window) {
    const utterance = new SpeechSynthesisUtterance(text);
    utterance.rate = 0.8;
    utterance.pitch = 1;
    utterance.volume = 0.8;
    window.speechSynthesis.speak(utterance);
  } else {
    message.warning('Text-to-speech is not supported in your browser');
  }
};

export const downloadReportAsPDF = (
  content: string,
  imageData: string | null,
  fileName: string = 'Nutrition_Analysis_Report.pdf',
  title: string = 'Nutritional Analysis Report'
): void => {
  try {
    const doc = new jsPDF();
    
    // Add title
    doc.setFontSize(16);
    doc.setFont('helvetica', 'bold');
    doc.text(title, 10, 20);
    
    // Add date
    doc.setFontSize(10);
    doc.setFont('helvetica', 'normal');
    doc.text(`Generated: ${new Date().toLocaleString()}`, 10, 30);
    
    // Add divider
    doc.line(10, 35, 200, 35);
    
    // Embed image if available
    let textStartY = 45;
    if (imageData) {
      try {
        doc.addImage(imageData, 'JPEG', 10, 40, 60, 60);
        textStartY = 110;
      } catch (error) {
        console.warn('Could not add image to PDF:', error);
      }
    }
    
    // Add content
    doc.setFontSize(12);
    doc.setFont('helvetica', 'normal');
    const lines = doc.splitTextToSize(content, 180);
    doc.text(lines, 10, textStartY);
    
    // Add disclaimer
    doc.setFontSize(10);
    doc.setFont('helvetica', 'italic');
    doc.text(
      'Disclaimer: This analysis is for informational purposes only.',
      10,
      doc.internal.pageSize.height - 20
    );
    
    doc.save(fileName);
    message.success('PDF downloaded successfully');
  } catch (error) {
    console.error('Error generating PDF:', error);
    message.error('Failed to generate PDF');
  }
};

export const validateImageFile = (file: File, maxSizeMB: number = 10): boolean => {
  const isImage = file.type.startsWith('image/');
  const isValidSize = file.size / 1024 / 1024 < maxSizeMB;
  
  if (!isImage) {
    message.error(`${file.name} is not an image file`);
    return false;
  }
  
  if (!isValidSize) {
    message.error(`File size must be less than ${maxSizeMB}MB`);
    return false;
  }
  
  return true;
};
