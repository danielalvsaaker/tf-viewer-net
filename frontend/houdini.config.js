/// <references types="houdini-svelte">

/** @type {import('houdini').ConfigFile} */
const config = {
    watchSchema: {
        url: 'http://localhost:5178/graphql'
    },
    plugins: {
        'houdini-svelte': {}
    },
    scalars: {
        Upload: {
            type: 'File'
        },
        URL: {
            // <- The GraphQL Scalar
            type: 'string' // <-  The TypeScript type
        },
        DateTime: {
            // <- The GraphQL Scalar
            type: 'Date' // <-  The TypeScript type
        },
        Position: {
            // <- The GraphQL Scalar
            type: '[number, number]' // <-  The TypeScript type
        },
        Length: {
            // <- The GraphQL Scalar
            type: 'number' // <-  The TypeScript type
        },
        Speed: {
            // <- The GraphQL Scalar
            type: 'number' // <-  The TypeScript type
        }
    },
    types: {
        User: {
            keys: ['userId'],
            resolve: {
                queryField: 'user'
            }
        }
    }
};

export default config;
