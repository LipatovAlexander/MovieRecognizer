import { CallToAction } from '@/components/CallToAction/CallToAction';
import { Welcome } from '@/components/Welcome/Welcome';
import RecognitionStatistics from '@/components/RecognitionStatistics/RecognitionStatistics';
import { Anchor, Box, Center } from '@mantine/core';

export default function HomePage() {
  return (
    <Box className="requires-no-scroll" px={10}>
      <Box mih="100vh" id="main" pt={200}>
        <Welcome />
        <CallToAction />
        <Center>
          <Anchor href="#statistics" underline="never" fz={24} mt="lg">
            Statistics ↓
          </Anchor>
        </Center>
      </Box>
      <Box mih="100vh" id="statistics">
        <Center mb={200}>
          <Anchor href="#main" underline="never" fz={24} mt="lg">
            Back ↑
          </Anchor>
        </Center>
        <RecognitionStatistics />
      </Box>
    </Box>
  );
}
