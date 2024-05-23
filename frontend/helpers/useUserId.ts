import { useCookies } from 'next-client-cookies';

export default function useUserId() {
  const cookies = useCookies();

  const userId = cookies.get('user_id');

  if (!userId) {
    throw new Error('user_id cookie is not set');
  }

  return userId;
}
