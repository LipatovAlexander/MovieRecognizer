import { Title, Text } from '@mantine/core';
import classes from './Welcome.module.css';

export function Welcome() {
  return (
    <>
      <Title className={classes.title} ta="center">
        Welcome to{' '}
        <Text inherit variant="gradient" component="span" gradient={{ from: 'pink', to: 'yellow' }}>
          Movie Recognizer
        </Text>
      </Title>
      <Text c="dimmed" ta="center" size="xl" maw={650} mx="auto" mt="lg">
        Discover movies from clips in a snap! Just paste a YouTube link, and we'll tell you the
        film. Easy and fast movie identification is just a click away.
      </Text>
    </>
  );
}
