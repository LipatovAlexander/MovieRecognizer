'use client';

import VideoUrlInput from '@/components/VideoUrlInput/VideoUrlInput';
import CreateRecognition from './create-recognition';
import { useCallback } from 'react';
import { useRouter } from 'next/navigation';

export default function RecognitionPage() {
  const router = useRouter();
  const onSubmit = useCallback(async (videoUrl: string) => {
    const recognitionId = await CreateRecognition(videoUrl);
    router.push(`recognition/${recognitionId}`);
  }, []);

  return (
    <>
      <VideoUrlInput onSubmit={onSubmit} />
    </>
  );
}
