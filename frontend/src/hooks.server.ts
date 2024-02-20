import { handle as auth } from './auth';
import { sequence } from '@sveltejs/kit/hooks';
import { redirect } from '@sveltejs/kit';

export const handle = sequence(auth, async ({ event, resolve }) => {
    const session = await event.locals.auth();

    if (session?.error == 'AccessTokenExpiredError') {
        const { csrfToken } = await event.fetch(`/auth/csrf`).then((response) => response.json());

        await event.fetch('/auth/signout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-Auth-Return-Redirect': '1'
            },
            body: new URLSearchParams({ csrfToken })
        });

        redirect(303, '/auth/signin');
    }

    if (!session && !event.url.pathname.startsWith('/auth')) {
        redirect(303, '/auth/signin');
    }

    if (session && event.url.pathname.startsWith('/auth')) {
        redirect(303, '/');
    }

    return resolve(event);
});
