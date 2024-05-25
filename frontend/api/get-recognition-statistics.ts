'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognitionStatistics from '@/types/MovieRecognitionStatistics';

export default async function GetRecognitionStatistics() {
  const url = new URL('/recognition/statistics', process.env.API_URL);
  const headers = new Headers();
  headers.set('x-api-key', process.env.API_KEY ?? 'Unknown');
  const response = await fetch(url, { headers });

  return (await response.json()) as ApiResponse<MovieRecognitionStatistics>;
}
