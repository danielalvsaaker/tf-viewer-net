import { HoudiniClient } from '$houdini';

export default new HoudiniClient({
    url: '/api/graphql',

    fetchParams() {
        return {
            headers: {
                'GraphQL-preflight': '1'
            }
        };
    }
});
