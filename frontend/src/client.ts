import { HoudiniClient } from '$houdini';

export default new HoudiniClient({
    url: 'http://localhost:5178/graphql',

    fetchParams({ session }) {
        return {
            headers: {
                Authorization: `Bearer ${session?.accessToken}`,
                'GraphQL-preflight': '1'
            }
        };
    }
});
