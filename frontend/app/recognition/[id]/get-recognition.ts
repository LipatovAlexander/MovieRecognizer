'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';

export default async function GetRecognition(id: string) {
  const response = await fetch(`${process.env.API_URL}/recognition/${id}`);

  var apiResponse = (await response.json()) as ApiResponse<MovieRecognition>;

  return apiResponse;
}
