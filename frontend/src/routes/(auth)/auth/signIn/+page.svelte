<script lang="ts">
    import Button from '$lib/components/ui/button/button.svelte';
    import { signIn } from '@auth/sveltekit/client';

    export let data;
</script>

<div class="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
    <div class="space-y-2 text-center">
        <h1 class="text-2xl font-semibold tracking-tight">Sign in</h1>
    </div>
    <Button
        variant="outline"
        type="button"
        on:click={() => signIn(data.provider.id, { callbackUrl: '/', redirect: true })}
    >
        Sign in
    </Button>
</div>
