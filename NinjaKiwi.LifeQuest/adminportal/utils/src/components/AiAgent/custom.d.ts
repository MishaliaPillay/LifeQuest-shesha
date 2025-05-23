declare namespace JSX {
    interface IntrinsicElements {
        'elevenlabs-convai': React.DetailedHTMLProps<React.HTMLAttributes<HTMLElement>, HTMLElement> & {
            // Add any specific props your component needs
            // For example:
            // apiKey?: string;
            // voiceId?: string;
            "agent-id": string;
        };
    }
  }