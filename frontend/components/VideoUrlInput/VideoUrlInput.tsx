'use client';

import useIsMobile from '@/helpers/useIsMobile';
import { Button, Group, TextInput } from '@mantine/core';
import { useForm, zodResolver } from '@mantine/form';
import { z } from 'zod';
import classes from './VideoUrlInput.module.css';

const schema = z.object({
  videoUrl: z.string().url(),
});

export default function VideoUrlInput({ onSubmit }: { onSubmit: (videoUrl: string) => void }) {
  const form = useForm({
    initialValues: {
      videoUrl: '',
    },
    validate: zodResolver(schema),
  });
  const isMobile = useIsMobile();

  const size = isMobile ? 'sm' : 'xl';

  return (
    <form
      style={{ width: '100%', maxWidth: 1000 }}
      onSubmit={form.onSubmit((values) => onSubmit(values.videoUrl))}
    >
      <Group>
        <TextInput
          style={{ flex: 1 }}
          size={size}
          placeholder="Enter YouTube video URL"
          classNames={classes}
          {...form.getInputProps('videoUrl')}
        />
        <Button size={size} type="submit">
          Recognize
        </Button>
      </Group>
    </form>
  );
}
