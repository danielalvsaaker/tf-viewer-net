<script lang="ts">
    import { graphql, paginatedFragment, type UserFeed } from '$houdini';
    import { onMount } from 'svelte';
    import * as Card from '$lib/components/ui/card';
    import * as Avatar from '$lib/components/ui/avatar';
    import Thumbnail from '$lib/components/Thumbnail.svelte';
    import { Loader2 } from 'lucide-svelte';

    export let user: UserFeed;
    $: userFeed = paginatedFragment(
        user,
        graphql(`
            fragment UserFeed on User {
                feed(first: 5) @paginate {
                    edges {
                        node {
                            userId
                            activityId
                            owner {
                                name
                                picture
                            }
                            startTime

                            ...ActivityId
                        }
                    }
                }
            }
        `)
    );

    let refs: HTMLElement[] = [];
    let intersectionObserver: IntersectionObserver;

    onMount(() => {
        intersectionObserver = new IntersectionObserver(([entry]) => {
            if (!entry.isIntersecting) {
                return;
            }

            if (window.requestIdleCallback) {
                requestIdleCallback(() => userFeed.loadNextPage(), { timeout: 1000 });
            } else {
                userFeed.loadNextPage();
            }

            intersectionObserver.unobserve(entry?.target);
        });

        return () => intersectionObserver.disconnect();
    });

    $: {
        if (refs.length >= 2) {
            intersectionObserver.observe(refs[refs.length - 2]);
        }
    }
</script>

{#if $userFeed?.data?.feed?.edges}
    {#each $userFeed.data.feed.edges.map((edge) => edge.node) as activity, i}
        <a href={`/user/${activity.userId}/activity/${activity.activityId}`} bind:this={refs[i]}>
            <Card.Root class="mb-4">
                <Card.Header>
                    <div class="flex items-center space-x-4">
                        <Avatar.Root class="h-8 w-8">
                            <Avatar.Image src={activity.owner?.picture} />
                            <Avatar.Fallback
                                >{activity.owner?.name.slice(0, 1).toUpperCase()}</Avatar.Fallback
                            >
                        </Avatar.Root>
                        <h3>{activity.owner?.name}</h3>
                    </div>
                    <Card.Title>{activity.startTime}</Card.Title>
                </Card.Header>
                <Card.Content>
                    <Thumbnail class="h-96 w-full rounded" {activity} />
                </Card.Content>
            </Card.Root>
        </a>
    {/each}
    {#if $userFeed.pageInfo.hasNextPage}
        <Loader2 class="animate-spin mx-auto" size={36} />
    {/if}
{/if}
