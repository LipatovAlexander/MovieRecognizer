import Logo from '@/components/Logo/Logo';
import { AppShell, AppShellHeader, AppShellMain, Center, Box } from '@mantine/core';

export default function RecognitionLayout({ children }: { children: any }) {
  return (
    <AppShell header={{ height: 60 }} padding="md">
      <AppShellHeader>
        <Center>
          <Logo />
        </Center>
      </AppShellHeader>
      <AppShellMain>
        <Center mih="calc(100vh - 92px)">
          <Box w="100%" maw={1000}>
            {children}
          </Box>
        </Center>
      </AppShellMain>
    </AppShell>
  );
}
