import GetRecognitionStatistics from '@/api/get-recognition-statistics';
import { Center, Stack, Title } from '@mantine/core';
import RecognitionStatisticsText from '@/components/RecognitionStatistics/RecognitionStatisticsText';
import classes from './RecognitionStatistics.module.css';

export default async function RecognitionStatistics() {
  const statistics = await GetRecognitionStatistics();

  if (!statistics?.ok) {
    return <></>;
  }

  const totalRecognitions = statistics.value.total_recognitions;
  const correctlyRecognized = statistics.value.correctly_recognized;
  const accuracy = Math.round((correctlyRecognized / totalRecognitions) * 100);

  return (
    <Center>
      <Stack>
        <Title ta="center" className={classes.title}>
          Recognition statistics
        </Title>
        <RecognitionStatisticsText
          totalRecognitions={totalRecognitions}
          correctlyRecognized={correctlyRecognized}
          accuracy={accuracy}
        />
      </Stack>
    </Center>
  );
}
