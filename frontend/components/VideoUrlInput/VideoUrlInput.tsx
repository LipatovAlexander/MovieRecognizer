'use client';

import useIsMobile from '@/helpers/useIsMobile';
import { Button, Group, TextInput } from '@mantine/core';

export default function VideoUrlInput() {
  const isMobile = useIsMobile();

  const size = isMobile ? 'sm' : 'xl';

  return (
    <Group w="100%" maw={1000}>
      <TextInput style={{ flex: 1 }} size={size} placeholder="Enter YouTube video URL" />
      <Button size={size}>Recognize</Button>
    </Group>
  );
}
