<script lang="ts">
    import { graphql } from "$houdini";
    import Map from "$lib/components/Map.svelte";
    import Skeleton from "$lib/components/ui/skeleton/skeleton.svelte";
    import type { Map_ActivityVariables } from "./$houdini";

    export const _Map_ActivityVariables: Map_ActivityVariables = ({ props }) => {
        return { ...props };
    };

    const store = graphql(`
        query Map_Activity($user: String!, $activity: String!) @load {
            user(userId: $user) {
                activity(activityId: $activity) {
                    activityId
                    boundingBox {
                        bbox
                    }
                    records {
                        position {
                            coordinates
                        }
                        distance
                        heartRate
                        altitude
                    }
                }
            }
        }`);

    $: geojson = {
            'type': 'Feature',
            'geometry': {
                'type': 'LineString',
                'coordinates': $store.data
                    ?.user
                    ?.activity
                    ?.records
                    .map(record => record.position?.coordinates)
                    .filter(coordinates => coordinates != null)
            }
    };
        

    export let user: string;
    export let activity: string;
    let className: string;
    export { className as class };
</script>

{#if $store.fetching}
    <Skeleton class={className}/>
{:else}
    <Map bounds={$store.data.user.activity.boundingBox.bbox} class={className} {geojson}/>
{/if}