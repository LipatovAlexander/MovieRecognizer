import RecognizedTitle from './RecognizedTitle';

interface VideoFrame {
  timestamp: string;
  fileUrl: string;
  processed: boolean;
  recognized_titles?: RecognizedTitle[];
}

export default VideoFrame;
