'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';

export default async function GetRecognition(id: string) {
  const url = new URL(`/recognition/${id}`, process.env.API_URL);
  const response = await fetch(url);

  var apiResponse = (await response.json()) as ApiResponse<MovieRecognition>;

  return apiResponse;
}
