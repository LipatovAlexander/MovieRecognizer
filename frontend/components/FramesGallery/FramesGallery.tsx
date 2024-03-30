import { Button, Center, Image, Modal, Text } from '@mantine/core';
import { Carousel } from '@mantine/carousel';
import VideoFrame from '@/types/VideoFrame';
import { useDisclosure } from '@mantine/hooks';
import Link from 'next/link';

export default function FramesGallery({ frames }: { frames: VideoFrame[] }) {
  const [opened, { open, close }] = useDisclosure(false);

  return (
    <Center>
      <Modal opened={opened} onClose={close} title="Frames gallery" centered size="auto">
        <Carousel
          maw={1000}
          slideSize="20%"
          slideGap="md"
          align="center"
          slidesToScroll={5}
          nextControlProps={{
            style: { backgroundColor: 'black', opacity: 0.5, border: 'none', color: 'white' },
          }}
          previousControlProps={{
            style: { backgroundColor: 'black', opacity: 0.5, border: 'none', color: 'white' },
          }}
        >
          {frames.map((frame) => {
            const movie =
              !!frame.recognized_titles && frame.recognized_titles.length > 0
                ? frame.recognized_titles.at(0)
                : null;

            return (
              <Carousel.Slide key={frame.fileUrl}>
                <Image style={{ opacity: !!movie ? 1 : 0.3 }} fit="contain" src={frame.fileUrl} />
                {!!movie && (
                  <Text mt="sm">
                    <Link target="_blank" href={movie.link}>
                      {movie.title}
                    </Link>
                  </Text>
                )}
              </Carousel.Slide>
            );
          })}
        </Carousel>
      </Modal>

      <Button variant="outline" onClick={open}>
        Show frames
      </Button>
    </Center>
  );
}
