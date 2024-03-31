import MovieRecognition from '@/types/MovieRecognition';
import { Loader, Stack, Text } from '@mantine/core';

export default function RecognitionProcess({
  movieRecognition,
}: {
  movieRecognition: MovieRecognition;
}) {
  const video = movieRecognition.video;

  if (!video) {
    return (
      <Stack>
        <Loader mx="auto" />
        <Text>Scraping YouTube video...</Text>
      </Stack>
    );
  }

  const videoFrames = video.video_frames;

  if (!videoFrames || videoFrames.length === 0) {
    return (
      <Stack>
        <Loader mx="auto" />
        <Text>Downloading the video...</Text>
      </Stack>
    );
  }

  const totalFrames = videoFrames.length;
  const processedFrames = videoFrames.filter((x) => x.processed).length;

  return (
    <Stack>
      <Loader mx="auto" />
      <Text>
        Process video frames ({processedFrames} / {totalFrames})...
      </Text>
    </Stack>
  );
}
