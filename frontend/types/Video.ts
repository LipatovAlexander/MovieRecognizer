import VideoFrame from './VideoFrame';

interface Video {
  title: string;
  author: string;
  duration: string;
  video_frames?: VideoFrame[];
}

export default Video;
