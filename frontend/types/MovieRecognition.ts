interface MovieRecognition {
  id: string;
  video_url: string;
  created_at: string;
  status: MovieRecognitionStatus;
}

type MovieRecognitionStatus = 'Created' | 'InProgress' | 'Failed' | 'Succeeded' | 'Invalid';

export default MovieRecognition;
