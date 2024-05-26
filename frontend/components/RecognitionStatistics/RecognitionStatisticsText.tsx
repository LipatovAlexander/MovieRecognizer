import { Highlight } from '@mantine/core';
import classes from './RecognitionStatistics.module.css';

export default function RecognitionStatisticsText({
  totalRecognitions,
  correctlyRecognized,
  accuracy,
}: {
  totalRecognitions: number;
  correctlyRecognized: number;
  accuracy: number;
}) {
  const text = `${totalRecognitions} videos processed so far, ${correctlyRecognized} recognized correctly (${accuracy}% accuracy).`;

  return (
    <Highlight
      highlight={[totalRecognitions.toString(), correctlyRecognized.toString(), `${accuracy}%`]}
      highlightStyles={{
        backgroundImage:
          'linear-gradient(45deg, var(--mantine-color-cyan-5), var(--mantine-color-indigo-5))',
        fontWeight: 700,
        WebkitBackgroundClip: 'text',
        WebkitTextFillColor: 'transparent',
        fontSize: '40px',
      }}
      className={classes.text}
    >
      {text}
    </Highlight>
  );
}
