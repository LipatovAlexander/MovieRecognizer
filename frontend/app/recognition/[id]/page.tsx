'use client';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';
import { useEffect, useState } from 'react';
import GetRecognition from './get-recognition';
import { Alert, Button, Group, Loader, Stack, Image, Text } from '@mantine/core';
import { IconInfoCircle } from '@tabler/icons-react';
import Link from 'next/link';
import { Carousel } from '@mantine/carousel';

export default function RecognitionPage({ params }: { params: { id: string } }) {
  const [data, setData] = useState<ApiResponse<MovieRecognition>>();

  useEffect(() => {
    const fetchData = async () => {
      const recognition = await GetRecognition(params.id);
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
    return <Loader />;
  }

  if (!data.ok || data.value.status === 'Failed' || data.value.status === 'Invalid') {
    const error = !data.ok ? data.code : data.value.failure_message ?? 'Unknown error';

    return (
      <Stack>
        <Alert variant="outline" color="red" icon={<IconInfoCircle />}>
          Error: {error}
        </Alert>
        <Button maw={100} mx="auto" component={Link} href="/recognition">
          Try again
        </Button>
      </Stack>
    );
  }

  if (data.value.status !== 'Succeeded') {
    return <Loader />;
  }

  const movie = data.value.recognized_movie;
  const movieBlock = !!movie && (
    <Group gap="lg">
      <Image src={movie.thumbnail} radius="xs" h="400" w="auto" fit="contain" />
      <Stack maw="580">
        <Text fw={900} size="xl">
          {movie.title}
        </Text>
        <Text c="dimmed">{movie.subtitle}</Text>
        <Text>{movie.description}</Text>
        <Text c="dimmed">
          <Link href={movie.link}>{movie.source}</Link>
        </Text>
      </Stack>
    </Group>
  );

  const video = data.value.video;
  const videoBlock = !!video && (
    <Text>
      <Link href={data.value.video_url}>{video.title}</Link>
    </Text>
  );

  const frames = data.value.video?.video_frames;
  const framesBlock = !!frames && (
    <Carousel
      maw={1000}
      height={200}
      slideSize="20%"
      slideGap="md"
      align="center"
      withIndicators
      loop
      slidesToScroll={5}
      nextControlProps={{
        style: { backgroundColor: 'black', opacity: 0.5, border: 'none', color: 'white' },
      }}
      previousControlProps={{
        style: { backgroundColor: 'black', opacity: 0.5, border: 'none', color: 'white' },
      }}
    >
      {frames.map((frame) => (
        <Carousel.Slide key={frame.fileUrl}>
          <Image fit="contain" src={frame.fileUrl} />
        </Carousel.Slide>
      ))}
    </Carousel>
  );

  return (
    <Stack>
      {movieBlock}
      {framesBlock}
      {videoBlock}
    </Stack>
  );
}
