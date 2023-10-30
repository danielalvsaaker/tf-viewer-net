import { SvelteKitAuth } from '@auth/sveltekit';
import { env } from '$env/dynamic/private';
import type { JWT } from '@auth/core/jwt';

export const handle = SvelteKitAuth({
    trustHost: true,
    providers: [
        {
            id: env.PROVIDER_ID,
            name: env.PROVIDER_NAME,
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
});
