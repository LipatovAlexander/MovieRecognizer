'use server';

import ApiResponse from '@/types/ApiResponse';
import TopRecognizedTitle from '@/types/TopRecognizedTitle';

export default async function GetTopRecognizedMovies(limit: number) {
  const url = new URL(`/recognition/top?limit=${limit}`, process.env.API_URL);
  const headers = new Headers();
  headers.set('x-api-key', process.env.API_KEY ?? 'Unknown');
  const response = await fetch(url, { headers });

  return (await response.json()) as ApiResponse<TopRecognizedTitle[]>;
}
