<script lang="ts">
    import UserNav from '$lib/components/UserNav.svelte';
    import Nav from '$lib/components/Nav.svelte';
    import { page } from '$app/stores';

    const user = $page.data.session?.user;
</script>

<div class="flex h-16 items-center px-4">
    <Nav user={user?.id} />
    <div class="ml-auto flex items-center space-x-4">
        {#if user}
            <UserNav />
        {/if}
    </div>
</div>
<main class="container">
    <slot />
</main>
