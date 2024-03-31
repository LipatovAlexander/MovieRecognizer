import { Button } from '@mantine/core';
import Link from 'next/link';

export default function TryAgainButton() {
  return (
    <Button maw={100} mx="auto" component={Link} href="/recognition">
      Try again
    </Button>
  );
}
