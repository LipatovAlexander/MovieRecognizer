'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';
import { URL } from 'url';

export default async function CreateRecognition(_: any, formData: FormData) {
  const videoUrl = formData.get('videoUrl');

  const url = new URL(`/recognition?videoUrl=${videoUrl}`, process.env.API_URL);
  const response = await fetch(url, {
    method: 'POST',
  });

  var apiResponse = (await response.json()) as ApiResponse<MovieRecognition>;

  return apiResponse;
}
