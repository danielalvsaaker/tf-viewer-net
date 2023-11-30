import { getSession, graphql } from '$houdini';
import type { HomepageVariables } from './$houdini';

export const _houdini_load = graphql(`
    query Homepage($user: String!) @cache(policy: CacheAndNetwork) {
        user(userId: $user) {
            ...UserFeed
        }
    }
`);

export const _HomepageVariables: HomepageVariables = async (event) => {
    const session = (await getSession(event)) as App.Session;

    return { user: session.user!.id! };
};
