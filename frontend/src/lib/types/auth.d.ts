declare module '@auth/core/types' {
    interface Session {
        accessToken?: string | undefined;
        error?: 'AccessTokenExpiredError';
    }
}

declare module '@auth/core/jwt' {
    interface JWT {
        access_token?: string | undefined;
        expires_at: number | undefined;
    }
}

export {};
