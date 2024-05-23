import Link from 'next/link';
import { Button, Group } from '@mantine/core';

export default function RecognitionHistoryButton() {
  return (
    <Group justify="center" mt="md">
      <Button variant="outline" maw={200} component={Link} href="/recognition/history">
        Recognition history
      </Button>
    </Group>
  );
}
