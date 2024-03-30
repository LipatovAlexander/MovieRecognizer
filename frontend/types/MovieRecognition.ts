interface MovieRecognition {
  id: string;
  video_url: string;
  created_at: string;
  status: MovieRecognitionStatus;
  failure_message?: string;
  video?: Video;
  recognized_movie?: RecognizedTitle;
}

type MovieRecognitionStatus = 'Created' | 'InProgress' | 'Failed' | 'Succeeded' | 'Invalid';

export default MovieRecognition;
