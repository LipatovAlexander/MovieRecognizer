import MovieRecognition from '@/types/MovieRecognition';
import { Loader, Stack, Text } from '@mantine/core';

function LoaderLayout({ children }: { children: any }) {
  return (
    <Stack>
      <Loader mx="auto" />
      <Text mx="auto">{children}</Text>
    </Stack>
  );
}

export default function RecognitionProcess({
  movieRecognition,
}: {
  movieRecognition: MovieRecognition;
}) {
  const video = movieRecognition.video;

  if (!video) {
    return <LoaderLayout>Scraping YouTube video...</LoaderLayout>;
  }

  const videoFrames = video.video_frames;

  if (!videoFrames || videoFrames.length === 0) {
    return <LoaderLayout>Downloading the video...</LoaderLayout>;
  }

  const totalFrames = videoFrames.length;
  const processedFrames = videoFrames.filter((x) => x.processed).length;

  if (processedFrames === totalFrames) {
    return <LoaderLayout>Aggregate the results...</LoaderLayout>;
  }

  return (
    <LoaderLayout>
      Process video frames ({processedFrames} / {totalFrames})...
    </LoaderLayout>
  );
}
