<script lang="ts">
    import { Skeleton } from '$lib/components/ui/skeleton/';
    import { backOut } from 'svelte/easing';
    import { scale } from 'svelte/transition';
    import { cn } from '$lib/utils';
    import type { HTMLImgAttributes } from 'svelte/elements';

    type $$Props = HTMLImgAttributes & { promiseFn: () => Promise<string> };
    let className: $$Props['class'] = undefined;
    export { className as class };

    export let promiseFn: () => Promise<string>;

    const promise = promiseFn();

    const destroy = (_: HTMLImageElement, url: string) => {
        return {
            destroy() {
                URL.revokeObjectURL(url);
            }
        };
    };
</script>

{#await promise}
    <Skeleton class={className} />
{:then src}
    <!-- svelte-ignore a11y-missing-attribute -->
    <img
        {src}
        use:destroy={src}
        class={cn('object-cover', className)}
        transition:scale={{ delay: 50, duration: 300, easing: backOut, start: 0.8 }}
        {...$$restProps}
    />
{/await}
