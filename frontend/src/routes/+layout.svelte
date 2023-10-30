<script lang="ts">
    import '../app.css';
    import UserNav from '$lib/components/UserNav.svelte';
    import Nav from '$lib/components/Nav.svelte';

    export let data;
</script>

<div class="flex h-16 items-center px-4">
    <Nav />
    <div class="ml-auto flex items-center space-x-4">
        {#if data.session?.user}
            <UserNav />
        {/if}
    </div>
</div>
<main class="container">
    <slot />
</main>
