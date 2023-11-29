<script lang="ts">
    import type { PageData } from './$houdini';
    import ActivityFeed from './(components)/ActivityFeed.svelte';

    export let data: PageData;

    $: ({ Homepage } = data);
</script>

{#if $Homepage?.data?.user}
    <ActivityFeed user={$Homepage.data.user} />
{/if}
