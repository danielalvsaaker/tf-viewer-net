<script lang="ts">
    import { fragment, graphql, type ActivityId, type ActivityId$data } from '$houdini';
    import Image from '$lib/components/Image.svelte';
    import { useMediaQuery } from '$lib/utils';

    let height = 400,
        width = 400;

    const isMd = useMediaQuery('(min-width: 768px');

    $: if ($isMd) {
        width = Math.trunc(700 * (16 / 9));
        height = Math.trunc(700 / (16 / 9));
    }

    let className: string | undefined = undefined;
    export { className as class };
    export let activity: ActivityId;

    $: data = fragment(
        activity,
        graphql(`
            fragment ActivityId on Activity {
                userId
                activityId
            }
        `)
    );

    const getImage = async (
        { userId, activityId }: ActivityId$data,
        height: number,
        width: number
    ) =>
        await fetch(
            `/api/user/${userId}/activity/${activityId}/thumbnail.png?width=${width}&height=${height}`
        )
            .then((response) => response.blob())
            .then((blob) => URL.createObjectURL(blob));
</script>

<Image promiseFn={() => getImage($data, height, width)} class={className} />
