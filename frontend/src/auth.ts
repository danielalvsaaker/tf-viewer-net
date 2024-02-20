import { SvelteKitAuth } from '@auth/sveltekit';
import { env } from '$env/dynamic/private';

export const { handle, signIn, signOut } = SvelteKitAuth({
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
        // @ts-expect-error JWT type does not expect 'expires_at'
        jwt: async ({ token, account, profile }) => {
            if (account && profile) {
                return {
                    ...token,
                    sub: profile.sub,
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
                    ...session.user,
                    id: token.sub!
                },
                accessToken: token.access_token,
                error: token.expires_at * 1000 < Date.now() ? 'AccessTokenExpiredError' : undefined
            };
        }
    }
});
