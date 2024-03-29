import { setSession } from '$houdini';
import type { LayoutServerLoad } from './$types';

export const load: LayoutServerLoad = async (event) => {
    const session = await event.locals.auth();

    if (session) {
        setSession(event, { user: session.user });
    }

    return {
        session: {
            user: session?.user
        }
    };
};
