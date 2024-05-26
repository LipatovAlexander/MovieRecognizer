import { CallToAction } from '@/components/CallToAction/CallToAction';
import { Welcome } from '@/components/Welcome/Welcome';
import RecognitionStatistics from '@/components/RecognitionStatistics/RecognitionStatistics';
import { Anchor, Box, Center } from '@mantine/core';
import TopRecognizedMovies from '@/components/TopRecognizedMovies/TopRecognizedMovies';

export default function HomePage() {
  return (
    <Box px={10}>
      <Box mih="100vh" id="main" pt={200}>
        <Welcome />
        <CallToAction />
        <Center>
          <Anchor href="#statistics" underline="never" fz={24} mt="lg" c="inherit">
            Statistics ↓
          </Anchor>
        </Center>
      </Box>
      <Box mih="100vh" id="statistics" pt={100}>
        <RecognitionStatistics />
        <TopRecognizedMovies />
      </Box>
    </Box>
  );
}
