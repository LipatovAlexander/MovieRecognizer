'use client';

import useUserId from '@/helpers/useUserId';
import { useEffect, useState } from 'react';
import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';
import GetRecognitionHistory from '@/app/recognition/get-recognition-history';
import { Box, Center, Loader, Stack, Table, Text, Title } from '@mantine/core';
import { CallToAction } from '@/components/CallToAction/CallToAction';
import Link from 'next/link';
import truncate from '@/helpers/truncate';
import { IconLink } from '@tabler/icons-react';

export default function RecognitionHistoryPage() {
  const userId = useUserId();
  const [loading, setLoading] = useState(true);
  const [recognitionHistory, setRecognitionHistory] = useState<ApiResponse<MovieRecognition[]>>();

  useEffect(() => {
    setLoading(true);
    const fetchRecognitionHistory = async () => {
      const response = await GetRecognitionHistory(userId);
      setRecognitionHistory(response);
      setLoading(false);
    };

    fetchRecognitionHistory();
  }, [userId]);

  if (loading) {
    return (
      <Center>
        <Loader />
      </Center>
    );
  }

  if (!recognitionHistory?.ok || recognitionHistory.value.length === 0) {
    return (
      <Stack>
        <Center>
          <Text fz="40px">Nothing to show here</Text>
        </Center>
        <CallToAction />
      </Stack>
    );
  }

  const rows = recognitionHistory.value.map((movieRecognition) => {
    const createdAtDate = new Date(movieRecognition.created_at);
    const createdAtString =
      ('0' + createdAtDate.getDate()).slice(-2) +
      '-' +
      ('0' + (createdAtDate.getMonth() + 1)).slice(-2) +
      '-' +
      createdAtDate.getFullYear() +
      ' ' +
      ('0' + createdAtDate.getHours()).slice(-2) +
      ':' +
      ('0' + createdAtDate.getMinutes()).slice(-2);

    const displayedVideoTitle = truncate(
      movieRecognition.video?.title ?? movieRecognition.video_url,
      50
    );

    const displayedRecognizedMovieText = truncate(
      movieRecognition.recognized_movie?.title ?? '',
      50
    );
    const displayedRecognizedMovie = !!movieRecognition.recognized_movie ? (
      <Link href={movieRecognition.recognized_movie.link}>{displayedRecognizedMovieText}</Link>
    ) : (
      displayedRecognizedMovieText
    );

    const displayedRecognitionConfirmation =
      movieRecognition.recognized_correctly === null ||
      movieRecognition.recognized_correctly === undefined
        ? ''
        : movieRecognition.recognized_correctly
          ? 'Yes'
          : 'No';

    return (
      <Table.Tr key={movieRecognition.id}>
        <Table.Td>
          <Link href={movieRecognition.video_url}>{displayedVideoTitle}</Link>
        </Table.Td>
        <Table.Td>{displayedRecognizedMovie}</Table.Td>
        <Table.Td>{displayedRecognitionConfirmation}</Table.Td>
        <Table.Td>
          <Link href={`/recognition/${movieRecognition.id}`}>
            <IconLink />
          </Link>
        </Table.Td>
      </Table.Tr>
    );
  });

  return (
    <Box mih="calc(100vh - 100px)">
      <Center mb="xl">
        <Title>Recognition history</Title>
      </Center>
      <Table mt="xl">
        <Table.Thead>
          <Table.Th>Source video</Table.Th>
          <Table.Th>Recognized movie</Table.Th>
          <Table.Th>Recognized correctly</Table.Th>
          <Table.Th>Details</Table.Th>
        </Table.Thead>
        <Table.Tbody>{rows}</Table.Tbody>
      </Table>
    </Box>
  );
}
