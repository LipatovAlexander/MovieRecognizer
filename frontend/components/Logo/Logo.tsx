import { Text } from '@mantine/core';
import classes from './Logo.module.css';
import Link from 'next/link';

export default function Logo() {
  return (
    <Text
      className={classes.logo}
      variant="gradient"
      component={Link}
      gradient={{ from: 'pink', to: 'yellow' }}
      href="/"
    >
      Movie Recognizer
    </Text>
  );
}
