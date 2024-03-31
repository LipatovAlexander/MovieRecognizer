import { Button } from '@mantine/core';
import Link from 'next/link';

export default function TryAgainButton() {
  return (
    <Button mx="auto" size="xl" component={Link} href="/recognition">
      Try again
    </Button>
  );
}
