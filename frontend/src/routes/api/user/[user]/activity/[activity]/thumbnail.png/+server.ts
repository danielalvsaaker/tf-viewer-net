import { graphql, setSession } from '$houdini';
import { generateMap } from '$lib/server/map';
import type { RequestHandler } from './$types';
import type { Activity } from '$lib/server/map.ts';

const activityStore = graphql(
    `
        query Activity($user: String!, $activity: String!) {
            user(userId: $user) {
                activity(activityId: $activity) {
                    boundingBox {
                        bbox
                    }
                    records {
                        position {
                            coordinates
                        }
                    }
                }
            }
        }
    `
);

export const GET: RequestHandler = async (event) => {
    const session = await event.locals.getSession();
    if (!session) {
        return new Response(null, { status: 401 });
    }

    const width = Number(event.url.searchParams.get('width'));
    const height = Number(event.url.searchParams.get('height'));

    setSession(event, session);

    const activity = await activityStore.fetch({
        event,
        variables: event.params
    });

    if (!activity.data?.user?.activity) {
        return new Response(null, { status: 404 });
    }

    if (!activity.data.user.activity.boundingBox.bbox) {
        return new Response(null, { status: 204 });
    }

    const image = await generateMap(activity.data.user.activity as Activity, {
        width,
        height
    });

    return new Response(image, {
        headers: {
            'Content-Type': 'image/png',
            'Cache-Control': `max-age=${3600 * 24}`
        }
    });
};
