type Provider = {
    id: string;
    name: string;
    type: string;
    signinUrl: string;
    callbackUrl: string;
};

type Data = {
    provider: Provider;
    params: SignInAuthorizationParams;
};

import type { SignInAuthorizationParams } from '@auth/sveltekit/client';

export async function load({ url, fetch }): Promise<Data> {
    const providers: object = await fetch('/auth/providers').then((response) => response.json());

    return {
        provider: Object.values(providers).at(0),
        params: url.searchParams
    };
}
