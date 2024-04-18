'use client';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';
import { useEffect, useState } from 'react';
import GetRecognition from './get-recognition';
import { Alert, Loader, Stack, Image, Text, Box, Grid, Center } from '@mantine/core';
import { IconInfoCircle } from '@tabler/icons-react';
import Link from 'next/link';
import FramesGallery from '@/components/FramesGallery/FramesGallery';
import TryAgainButton from '@/components/TryAgainButton/TryAgainButton';
import RecognitionProcess from '@/components/RecognitionProcess/RecognitionProcess';
import useIsMobile from '@/helpers/useIsMobile';

export default function RecognitionPage({ params }: { params: { id: string } }) {
  const [data, setData] = useState<ApiResponse<MovieRecognition>>();
  const isMobile = useIsMobile();

  useEffect(() => {
    const fetchData = async () => {
      const recognition = await GetRecognition(params.id);

      if (!recognition) {
        return;
      }

      setData(recognition);

      if (
        recognition.ok &&
        (recognition.value.status === 'Created' || recognition.value.status === 'InProgress')
      ) {
        setTimeout(fetchData, 1000);
      }
    };

    fetchData();
  }, []);

  if (!data) {
    return (
      <Center>
        <Loader />
      </Center>
    );
  }

  if (!data.ok || data.value.status === 'Failed' || data.value.status === 'Invalid') {
    const error = !data.ok ? data.code : data.value.failure_message ?? 'Unknown error';

    return (
      <Stack>
        <Alert variant="outline" color="red" icon={<IconInfoCircle />}>
          Error: {error}
        </Alert>
        <TryAgainButton />
      </Stack>
    );
  }

  if (data.value.status !== 'Succeeded') {
    return <RecognitionProcess movieRecognition={data.value} />;
  }

  const movie = data.value.recognized_movie;
  const movieBlock = !!movie ? (
    <Grid gutter="xl">
      <Grid.Col span={isMobile ? 12 : 5}>
        <Image src={movie.thumbnail} radius="xs" mah="400" fit="contain" />
      </Grid.Col>
      <Grid.Col span={isMobile ? 12 : 7}>
        <Center h="100%">
          <Stack>
            <Box>
              <Text fw={900} size="xl">
                {movie.title}
              </Text>
              <Text c="dimmed">{movie.subtitle}</Text>
            </Box>
            <Text>{movie.description}</Text>
            <Text c="dimmed">
              <Link target="_blank" href={movie.link}>
                {movie.source}
              </Link>
            </Text>
          </Stack>
        </Center>
      </Grid.Col>
    </Grid>
  ) : (
    <>
      <Text fz="40px">Unfortunately we were unable to recognize the film :(</Text>
      <TryAgainButton />
    </>
  );

  const video = data.value.video;
  const videoBlock = !!video && (
    <Text size="xl">
      <Link target="_blank" href={data.value.video_url}>
        {video.title}
      </Link>
    </Text>
  );

  const frames = data.value.video?.video_frames;
  const framesBlock = !isMobile && !!movie && !!frames && <FramesGallery frames={frames} />;

  return (
    <Stack gap="xl">
      {videoBlock}
      {movieBlock}
      {framesBlock}
    </Stack>
  );
}
