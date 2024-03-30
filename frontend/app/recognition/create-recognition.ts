'use server';

import { randomUUID } from 'crypto';

export default async function CreateRecognition(videoUrl: string) {
  return randomUUID() as string;
}
