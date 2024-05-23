import RecognizedTitle from './RecognizedTitle';
import Video from './Video';

interface MovieRecognition {
  id: string;
  user_id: string;
  video_url: string;
  created_at: string;
  status: MovieRecognitionStatus;
  failure_message?: string;
  video?: Video;
  recognized_movie?: RecognizedTitle;
  recognized_correctly?: boolean;
}

export type MovieRecognitionStatus = 'Created' | 'InProgress' | 'Failed' | 'Succeeded' | 'Invalid';

export default MovieRecognition;
