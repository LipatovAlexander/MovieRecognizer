import GetTopRecognizedMovies from '@/api/get-top-recognized-movies';
import {
  Card,
  Center,
  Image,
  CardSection,
  Grid,
  GridCol,
  Highlight,
  Badge,
  Anchor,
  Title,
  Stack,
} from '@mantine/core';
import TopRecognizedTitle from '@/types/TopRecognizedTitle';
import classes from './TopRecognizedMovies.module.css';

function TopRecognizedMovieCard({
  topRecognizedTitle,
  size,
  position,
}: {
  topRecognizedTitle: TopRecognizedTitle;
  size: number;
  position: number;
}) {
  return (
    <Card>
      <CardSection pos="relative">
        <Image src={topRecognizedTitle.recognized_title.thumbnail} height={size} />
        <Badge pos="absolute" top={0} left={0} radius={0} size="xl">
          Top {position}
        </Badge>
      </CardSection>
      <Anchor href={topRecognizedTitle.recognized_title.link} c="inherit" mt={10} w="fit-content">
        {topRecognizedTitle.recognized_title.title}
      </Anchor>
      <Highlight
        highlight={topRecognizedTitle.count.toString()}
        highlightStyles={{
          backgroundImage:
            'linear-gradient(45deg, var(--mantine-color-cyan-5), var(--mantine-color-indigo-5))',
          fontWeight: 700,
          WebkitBackgroundClip: 'text',
          WebkitTextFillColor: 'transparent',
          fontSize: '18px',
        }}
        c="dimmed"
      >{`Recognized ${topRecognizedTitle.count} times`}</Highlight>
    </Card>
  );
}

export default async function TopRecognizedMovies() {
  const topRecognizedMovies = await GetTopRecognizedMovies(3);

  if (!topRecognizedMovies.ok) {
    return <></>;
  }

  const top1Card = (
    <TopRecognizedMovieCard
      topRecognizedTitle={topRecognizedMovies.value[0]}
      size={250}
      position={1}
    />
  );
  const top2Card = (
    <TopRecognizedMovieCard
      topRecognizedTitle={topRecognizedMovies.value[1]}
      size={220}
      position={2}
    />
  );
  const top3Card = (
    <TopRecognizedMovieCard
      topRecognizedTitle={topRecognizedMovies.value[2]}
      size={190}
      position={3}
    />
  );

  return (
    <Center mt={50}>
      <Stack>
        <Title ta="center" mb={20} className={classes.title}>
          Most popular films
        </Title>
        <Grid align="center" justify="center" gutter={50}>
          <GridCol span="content">{top1Card}</GridCol>
          <GridCol span="content">{top2Card}</GridCol>
          <GridCol span="content">{top3Card}</GridCol>
        </Grid>
      </Stack>
    </Center>
  );
}
