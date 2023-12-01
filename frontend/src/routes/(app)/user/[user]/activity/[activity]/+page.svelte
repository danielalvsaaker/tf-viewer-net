<script lang="ts">
    import type { PageData } from './$houdini';
    import Sidebar from './Sidebar.svelte';
    import Header from './(components)/Header.svelte';
    import Map from './Map.svelte';

    export let data: PageData;

    $: ({ Activity } = data);
    $: user = $Activity.data!.user;
    $: activity = user!.activity!;
</script>

<div class="grid lg:grid-cols-5">
    <div class="col-span-3 lg:col-span-4 lg:border-l">
        <main class="container">
            <Header {activity}/>
            <Map class="h-96 w-full" user={$Activity.variables.user} activity={$Activity.variables.activity}/>
        </main>
    </div>
    <div class="hidden lg:block">
        <Sidebar />
    </div>
</div>
