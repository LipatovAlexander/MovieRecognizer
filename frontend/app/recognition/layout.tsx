import Logo from '@/components/Logo/Logo';
import { AppShell, AppShellHeader, AppShellMain, Center } from '@mantine/core';

export default function RecognitionLayout({ children }: { children: any }) {
  return (
    <AppShell header={{ height: 60 }} padding="md">
      <AppShellHeader>
        <Center>
          <Logo />
        </Center>
      </AppShellHeader>
      <AppShellMain>
        <Center mih="calc(100vh - 92px)">{children}</Center>
      </AppShellMain>
    </AppShell>
  );
}
