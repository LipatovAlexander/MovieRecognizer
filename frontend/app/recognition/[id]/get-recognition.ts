'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';

export default async function GetRecognition(id: string) {
  const url = new URL(`/recognition/${id}`, process.env.API_URL);
  const headers = new Headers();
  headers.set('x-api-key', process.env.API_KEY ?? 'Unknown');
  const response = await fetch(url, { headers });

  return (await response.json()) as ApiResponse<MovieRecognition>;
}
