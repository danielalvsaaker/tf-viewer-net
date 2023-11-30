import { SvelteKitAuth } from '@auth/sveltekit';
import { env } from '$env/dynamic/private';
import type { JWT } from '@auth/core/jwt';
import { sequence } from '@sveltejs/kit/hooks';
import { redirect } from '@sveltejs/kit';

export const handle = sequence(
    SvelteKitAuth({
        trustHost: true,
        pages: {
            signIn: '/auth/signIn'
        },
        providers: [
            {
                id: env.PROVIDER_ID!,
                name: env.PROVIDER_NAME!,
                type: 'oidc',
                issuer: env.ISSUER,
                clientId: env.CLIENT_ID,
                clientSecret: env.CLIENT_SECRET
            }
        ],
        callbacks: {
            jwt: async ({ token, account }) => {
                if (account) {
                    return {
                        ...token,
                        access_token: account.access_token,
                        expires_at: account.expires_at
                    };
                }

                return token;
            },
            session: async ({ session, token }) => {
                return {
                    ...session,
                    user: {
                        id: token.sub!,
                        ...session.user
                    },
                    accessToken: token.access_token,
                    error:
                        (token as JWT).expires_at * 1000 < Date.now()
                            ? 'AccessTokenExpiredError'
                            : undefined
                };
            }
        }
    }),
    async ({ event, resolve }) => {
        const session = await event.locals.getSession();

        if (session?.error == 'AccessTokenExpiredError') {
            const { csrfToken } = await event
                .fetch(`/auth/csrf`)
                .then((response) => response.json());

            await event.fetch('/auth/signout', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'X-Auth-Return-Redirect': '1'
                },
                body: new URLSearchParams({ csrfToken })
            });

            throw redirect(303, '/auth/signin');
        }

        if (!session && !event.url.pathname.startsWith('/auth')) {
            throw redirect(303, '/auth/signin');
        }

        if (session && event.url.pathname.startsWith('/auth')) {
            throw redirect(303, '/');
        }

        return resolve(event);
    }
);
