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
                    token.accessToken = account.access_token;
                }

                return token;
            },
            session: async (params) => {
                const { token } = params;

                return {
                    ...params.session,
                    user: {
                        id: params.token.sub!,
                        ...params.session.user
                    },
                    accessToken: (token as JWT).accessToken
                };
            }
        }
    }),
    async ({ event, resolve }) => {
        const session = await event.locals.getSession();

        if (!session && !event.url.pathname.startsWith('/auth')) {
            throw redirect(303, '/auth/signin');
        }

        if (session && event.url.pathname.startsWith('/auth')) {
            throw redirect(303, '/');
        }

        return resolve(event);
    }
);
