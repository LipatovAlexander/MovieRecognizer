'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';
import { URL } from 'url';
import { cookies } from 'next/headers';

export default async function CreateRecognition(_: any, formData: FormData) {
  const videoUrl = formData.get('videoUrl');

  const url = new URL('/recognition', process.env.API_URL);
  const headers = new Headers();
  headers.set('Content-Type', 'application/json');
  headers.set('x-api-key', process.env.API_KEY ?? 'Unknown');

  const userId = cookies().get('user_id')?.value;

  if (!userId) {
    throw new Error('user_id cookie required');
  }

  const response = await fetch(url, {
    method: 'POST',
    headers: headers,
    body: JSON.stringify({
      userId,
      videoUrl,
    }),
  });

  if (!response.ok) {
    throw new Error(
      `Response was not successful. Status: ${response.status}. Text: ${await response.text()}`
    );
  }

  return (await response.json()) as ApiResponse<MovieRecognition>;
}
