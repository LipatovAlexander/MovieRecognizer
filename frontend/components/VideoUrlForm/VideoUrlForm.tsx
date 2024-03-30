import useIsMobile from '@/helpers/useIsMobile';
import { Button, Group, TextInput } from '@mantine/core';
import classes from './VideoUrlForm.module.css';
import { useFormStatus } from 'react-dom';

export default function VideoUrlForm() {
  const { pending } = useFormStatus();
  const isMobile = useIsMobile();

  const size = isMobile ? 'sm' : 'xl';

  return (
    <Group>
      <TextInput
        name="videoUrl"
        required
        type="url"
        style={{ flex: 1 }}
        size={size}
        placeholder="Enter YouTube video URL"
        classNames={classes}
      />
      <Button size={size} type="submit" loading={pending}>
        Recognize
      </Button>
    </Group>
  );
}
