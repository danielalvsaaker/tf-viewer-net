<script lang="ts">
    import { fragment, graphql, type ActivityId, type ActivityId$data } from '$houdini';
    import Image from '$lib/components/Image.svelte';

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

    const getImage = async ({ userId, activityId }: ActivityId$data) =>
        await fetch(
            `/api/user/${userId}/activity/${activityId}/thumbnail.png?width=${screen.availWidth}&height=${screen.availHeight}`
        )
            .then((response) => response.blob())
            .then((blob) => URL.createObjectURL(blob));
</script>

<Image promiseFn={() => getImage($data)} class={className} />
