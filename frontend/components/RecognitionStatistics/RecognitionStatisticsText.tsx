import { Highlight } from '@mantine/core';

export default function RecognitionStatisticsText({
  totalRecognized,
  correctlyRecognized,
  accuracy,
}: {
  totalRecognized: number;
  correctlyRecognized: number;
  accuracy: number;
}) {
  const text = `${totalRecognized} movies recognized so far, ${correctlyRecognized} correctly (${accuracy}% accuracy).`;

  return (
    <Highlight
      highlight={[totalRecognized.toString(), correctlyRecognized.toString(), `${accuracy}%`]}
      fz="30px"
      highlightStyles={{
        backgroundImage:
          'linear-gradient(45deg, var(--mantine-color-cyan-5), var(--mantine-color-indigo-5))',
        fontWeight: 700,
        WebkitBackgroundClip: 'text',
        WebkitTextFillColor: 'transparent',
        fontSize: '40px',
      }}
    >
      {text}
    </Highlight>
  );
}
