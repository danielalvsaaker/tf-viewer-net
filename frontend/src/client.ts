import { HoudiniClient } from '$houdini';
import { env } from '$env/dynamic/public';

export default new HoudiniClient({
    url: `${env.PUBLIC_BACKEND_URL}/graphql`,

    fetchParams({ session }) {
        return {
            headers: {
                Authorization: `Bearer ${session?.accessToken}`,
                'GraphQL-preflight': '1'
            }
        };
    }
});
