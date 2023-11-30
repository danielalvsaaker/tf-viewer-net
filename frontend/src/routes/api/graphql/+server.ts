import type { RequestHandler } from '@sveltejs/kit';
import { env } from '$env/dynamic/private';
import { request } from 'undici';
import { Readable } from 'node:stream';
import type { ReadableStream } from 'node:stream/web';

export const POST: RequestHandler = async (event) => {
    const session = await event.locals.getSession();

    // https://github.com/nodejs/undici/issues/1462
    const response = await request(`${env.BACKEND_URL}/graphql`, {
        body: Readable.fromWeb(event.request.body as ReadableStream<any>),
        method: 'POST',
        headers: {
            Authorization: `Bearer ${session?.accessToken}`,
            ...Object.fromEntries(
                Array.from(event.request.headers.entries()).filter(
                    ([key]) => !['host', 'origin', 'referer'].includes(key.toLowerCase())
                )
            )
        }
    });

    return new Response(response.body.body, {
        status: response.statusCode,
        headers: response.headers as HeadersInit
    });
};
