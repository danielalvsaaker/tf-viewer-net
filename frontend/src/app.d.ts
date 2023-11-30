// See https://kit.svelte.dev/docs/types#app

import type { DefaultSession } from '@auth/core/types';

// for information about these interfaces
declare global {
    namespace App {
        interface Session {
            accessToken?: string;
            user:
                | ({
                      id: string | undefined;
                  } & DefaultSession['user'])
                | null;
            error?: 'AccessTokenExpiredError';
        }
        // interface Error {}
        // interface Locals {}
        // interface PageData {}
        // interface Platform {}
    }
}

export {};
