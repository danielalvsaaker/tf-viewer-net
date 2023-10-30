import { setSession } from '$houdini';
import type { LayoutServerLoad } from './$types';

export const load: LayoutServerLoad = async (event) => {
    const session = await event.locals.getSession();

    if (session) {
        setSession(event, session);
    }

    return { session };
};
