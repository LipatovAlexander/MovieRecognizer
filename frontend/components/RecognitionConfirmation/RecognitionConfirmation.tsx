import MovieRecognition from '@/types/MovieRecognition';
import { Button, Center, Group, Stack, Text } from '@mantine/core';
import { useCallback, useState } from 'react';
import ConfirmRecognition from '@/app/recognition/[id]/confirm-recognition';

export default function RecognitionConfirmation({
  movieRecognition,
}: {
  movieRecognition: MovieRecognition;
}) {
  const [recognizedCorrectly, setRecognizedCorrectly] = useState<boolean | null>(null);
  const confirm = useCallback(async (recognizedCorrectly: boolean) => {
    await ConfirmRecognition(movieRecognition.id, recognizedCorrectly);
    setRecognizedCorrectly(recognizedCorrectly);
  }, []);

  if (recognizedCorrectly !== null) {
    return (
      <Center>
        <Text size="xl">Thank you for feedback!</Text>
      </Center>
    );
  }

  return (
    <Center>
      <Stack>
        <Text size="xl">Was it recognized correctly?</Text>
        <Group justify="center">
          <Button variant="subtle" onClick={() => confirm(true)}>
            Yes
          </Button>
          <Button variant="subtle" onClick={() => confirm(false)}>
            No
          </Button>
        </Group>
      </Stack>
    </Center>
  );
}
