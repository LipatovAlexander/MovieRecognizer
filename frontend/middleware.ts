import { NextRequest, NextResponse } from 'next/server';
import { v4 as uuidv4 } from 'uuid';

export function middleware(request: NextRequest) {
  const response = NextResponse.next();
  const userIdCookie = request.cookies.get('user_id');

  if (!userIdCookie) {
    const userId = uuidv4();
    const expires = new Date();
    expires.setFullYear(expires.getFullYear() + 100);
    response.cookies.set('user_id', userId, { path: '/', httpOnly: true, expires });
  }

  return response;
}

export const config = {
  matcher: '/:path*',
};
