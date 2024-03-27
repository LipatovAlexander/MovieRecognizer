import { Button, Group } from '@mantine/core';
import Link from 'next/link';

export function CallToAction() {
  return (
    <Group justify="center" mt="xl">
      <Button size="xl" component={Link} href="recognition">
        Find Your Movie
      </Button>
    </Group>
  );
}
