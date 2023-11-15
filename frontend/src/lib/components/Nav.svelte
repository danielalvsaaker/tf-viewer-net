<script lang="ts">
    export let user: string | undefined;
</script>

<nav class="flex items-center space-x-4 lg:space-x-6">
    <a href="/" class="text-l font-large transition-colors hover:text-primary"> Tf-viewer </a>
    {#if user}
        <a
            href={`/user/${user}/activity`}
            class="text-sm font-medium transition-colors hover:text-primary"
        >
            Activities
        </a>
        <a href="/upload" class="text-sm font-medium transition-colors hover:text-primary">
            Upload
        </a>
    {/if}
</nav>
