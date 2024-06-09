'use client';

import { Highlight } from '@mantine/core';
import classes from './RecognitionStatistics.module.css';
import useIsMobile from '@/helpers/useIsMobile';

export default function RecognitionStatisticsText({
  totalRecognitions,
  correctlyRecognized,
  accuracy,
}: {
  totalRecognitions: number;
  correctlyRecognized: number;
  accuracy: number;
}) {
  const isMobile = useIsMobile();

  const text = `${totalRecognitions} videos successfully processed so far, ${correctlyRecognized} recognized correctly (${accuracy}% accuracy).`;

  return (
    <Highlight
      highlight={[totalRecognitions.toString(), correctlyRecognized.toString(), `${accuracy}%`]}
      highlightStyles={{
        backgroundImage:
          'linear-gradient(45deg, var(--mantine-color-cyan-5), var(--mantine-color-indigo-5))',
        fontWeight: 700,
        WebkitBackgroundClip: 'text',
        WebkitTextFillColor: 'transparent',
        fontSize: !isMobile ? '40px' : '30px',
      }}
      className={classes.text}
    >
      {text}
    </Highlight>
  );
}
