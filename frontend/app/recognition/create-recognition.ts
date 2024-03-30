'use server';

import ApiResponse from '@/types/ApiResponse';
import MovieRecognition from '@/types/MovieRecognition';

export default async function CreateRecognition(_: any, formData: FormData) {
  const videoUrl = formData.get('videoUrl');

  const response = await fetch(`${process.env.API_URL}/recognition?videoUrl=${videoUrl}`, {
    method: 'POST',
  });

  var apiResponse = (await response.json()) as ApiResponse<MovieRecognition>;

  return apiResponse;
}
