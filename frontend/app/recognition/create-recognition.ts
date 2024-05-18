'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';
import { URL } from 'url';

export default async function CreateRecognition(_: any, formData: FormData) {
  const videoUrl = formData.get('videoUrl');

  const url = new URL(`/recognition?videoUrl=${videoUrl}`, process.env.API_URL);
  const headers = new Headers();
  headers.set('x-api-key', process.env.API_KEY ?? 'Unknown');

  const response = await fetch(url, {
    method: 'POST',
    headers: headers,
  });

  return (await response.json()) as ApiResponse<MovieRecognition>;
}
