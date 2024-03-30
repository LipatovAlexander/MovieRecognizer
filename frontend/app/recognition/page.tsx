'use client';

import { useFormState } from 'react-dom';
import CreateRecognition from './create-recognition';
import VideoUrlForm from '@/components/VideoUrlForm/VideoUrlForm';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { Alert, Stack } from '@mantine/core';
import { IconInfoCircle } from '@tabler/icons-react';

export default function RecognitionPage() {
  const [state, formAction] = useFormState(CreateRecognition, undefined);
  const [error, setError] = useState<string>();
  const router = useRouter();

  useEffect(() => {
    if (!state) {
      return;
    }

    if (state.ok) {
      setError(undefined);
      const recognitionId = state.value.id;
      router.push(`/recognition/${recognitionId}`);
    } else {
      setError(state.code);
    }
  }, [state]);

  return (
    <Stack w="100%" maw={1000}>
      {!!error && (
        <Alert variant="outline" color="red" icon={<IconInfoCircle />}>
          Error: {error}
        </Alert>
      )}
      <form action={formAction}>
        <VideoUrlForm />
      </form>
    </Stack>
  );
}
