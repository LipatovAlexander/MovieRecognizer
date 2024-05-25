import '@mantine/core/styles.css';
import React from 'react';
import { MantineProvider, ColorSchemeScript } from '@mantine/core';
import { theme } from '@/theme';
import './global.css';
import '@mantine/carousel/styles.css';
import { CookiesProvider } from 'next-client-cookies/server';

export const fetchCache = 'default-no-store';

export const metadata = {
  title: 'Movie Recognizer',
  description:
    'Identify the movie from a YouTube clip quickly with our easy-to-use recognition tool. Find out the film title in seconds!',
};

export default function RootLayout({ children }: { children: any }) {
  return (
    <html lang="en">
      <head>
        <ColorSchemeScript />
        <link rel="shortcut icon" href="/favicon.ico" />
        <meta
          name="viewport"
          content="minimum-scale=1, initial-scale=1, width=device-width, user-scalable=no"
        />
      </head>
      <body>
        <MantineProvider defaultColorScheme="auto" theme={theme}>
          <CookiesProvider>{children}</CookiesProvider>
        </MantineProvider>
      </body>
    </html>
  );
}
