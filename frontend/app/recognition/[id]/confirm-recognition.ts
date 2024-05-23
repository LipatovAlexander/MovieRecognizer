'use server';

import { cookies } from 'next/headers';
import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';

export default async function ConfirmRecognition(id: string, recognizedCorrectly: boolean) {
  const cookie = cookies();
  const userId = cookie.get('user_id');

  if (!userId) {
    throw new Error('user_id cookie not set');
  }

  const url = new URL(`/recognition/confirm`, process.env.API_URL);
  const headers = new Headers();
  headers.set('Content-Type', 'application/json');
  headers.set('x-api-key', process.env.API_KEY ?? 'Unknown');
  const response = await fetch(url, {
    method: 'POST',
    headers,
    body: JSON.stringify({
      userId: userId.value,
      movieRecognitionId: id,
      recognizedCorrectly,
    }),
  });

  return (await response.json()) as ApiResponse<MovieRecognition>;
}
