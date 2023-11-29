<script lang="ts">
    import { graphql, paginatedFragment, type UserFeed } from '$houdini';
    import { onMount } from 'svelte';
    import * as Card from '$lib/components/ui/card';
    import * as Avatar from '$lib/components/ui/avatar';
    import Thumbnail from '$lib/components/Thumbnail.svelte';
    import { Loader2 } from 'lucide-svelte';
    import { DateTime } from 'luxon';
    import ActivityData from './ActivityData.svelte';

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
                            totalTimerTime(unit: MILLISECOND)
                            sessions {
                                distance(unit: KILOMETER)
                                speedMax(unit: KILOMETER_PER_HOUR)
                                speedAverage(unit: KILOMETER_PER_HOUR)
                                ascent
                                calories
                            }
                            ...ActivityId
                        }
                    }
                }
            }
        `)
    );

    let target: HTMLElement;

    onMount(() => {
        const intersectionObserver = new IntersectionObserver(
            ([entry]) => {
                if (!entry.isIntersecting) {
                    return;
                }

                if ('requestIdleCallback' in window) {
                    requestIdleCallback(() => userFeed.loadNextPage(), { timeout: 1000 });
                } else {
                    userFeed.loadNextPage();
                }
            },
            {
                rootMargin: '80%'
            }
        );

        intersectionObserver.observe(target);

        return () => intersectionObserver.disconnect();
    });

    const locale = navigator.language;
</script>

{#if $userFeed?.data?.feed?.edges}
    {#each $userFeed.data.feed.edges.map((edge) => edge.node) as activity}
        <Card.Root class="mb-4">
            <Card.Header class="">
                <div class="flex items-center space-x-4">
                    <Avatar.Root class="h-8 w-8">
                        <Avatar.Image src={activity.owner?.picture} />
                        <Avatar.Fallback>
                            {activity.owner?.name.slice(0, 1).toUpperCase()}
                        </Avatar.Fallback>
                    </Avatar.Root>
                    <h3>{activity.owner?.name}</h3>
                </div>
                <Card.Title class="text-base md:text-lg">
                    {activity.startTime.toLocaleString(DateTime.DATETIME_MED_WITH_WEEKDAY)}
                </Card.Title>
            </Card.Header>
            <a href={`/user/${activity.userId}/activity/${activity.activityId}`}>
                <Card.Content>
                    <Thumbnail
                        class="h-72 aspect-square md:h-96 md:aspect-video w-full rounded"
                        {activity}
                    />
                    <div
                        class="grid auto-rows-fr grid-cols-2 md:grid-cols-3 gap-1 items-center mt-2"
                    >
                        <ActivityData
                            title="Elapsed time"
                            value={activity.totalTimerTime
                                .rescale()
                                .normalize()
                                .toFormat('hh:mm:ss')}
                        />
                        <ActivityData
                            title="Distance"
                            value={activity.sessions
                                .reduce((acc, { distance }) => (distance ? acc + distance : acc), 0)
                                .toLocaleString(locale, {
                                    style: 'unit',
                                    unit: 'kilometer',
                                    maximumSignificantDigits: 3
                                })}
                        />
                        <ActivityData
                            title="Max. speed"
                            value={activity.sessions
                                .reduce((acc, { speedMax }) => Math.max(acc, speedMax ?? 0), 0)
                                .toLocaleString(locale, {
                                    style: 'unit',
                                    unit: 'kilometer-per-hour',
                                    maximumSignificantDigits: 3
                                })}
                        />
                        <ActivityData
                            title="Avg. speed"
                            value={activity.sessions
                                .reduce(
                                    (acc, { speedAverage }) => Math.max(acc, speedAverage ?? 0),
                                    0
                                )
                                .toLocaleString(locale, {
                                    style: 'unit',
                                    unit: 'kilometer-per-hour',
                                    maximumSignificantDigits: 3
                                })}
                        />
                        <ActivityData
                            title="Calories"
                            value="{activity.sessions.reduce(
                                (acc, { calories }) => Math.max(acc, calories ?? 0),
                                0
                            )}
                                    kcal"
                        />
                        <ActivityData
                            title="Ascent"
                            value={activity.sessions
                                .reduce((acc, { ascent }) => Math.max(acc, ascent ?? 0), 0)
                                .toLocaleString(locale, {
                                    style: 'unit',
                                    unit: 'meter',
                                    maximumSignificantDigits: 3
                                })}
                        />
                    </div>
                </Card.Content>
            </a>
        </Card.Root>
    {/each}
    <div bind:this={target} />
    {#if $userFeed.pageInfo.hasNextPage}
        <Loader2 class="animate-spin mx-auto" size={36} />
    {/if}
{/if}
