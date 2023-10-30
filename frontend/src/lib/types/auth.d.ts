import type { DefaultSession } from '@auth/core/types';

declare module '@auth/core/types' {
    interface Session extends DefaultSession {
        accessToken?: string | undefined;
    }
}

declare module '@auth/core/jwt' {
    interface JWT extends DefaultJWT {
        accessToken?: string | undefined;
    }
}

export {};
