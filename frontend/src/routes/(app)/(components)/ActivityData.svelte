<script lang="ts">
    import * as Card from '$lib/components/ui/card';

    export let title: string;
    export let value: string | null;
</script>

<Card.Root class="flex-auto">
    <Card.Header class="flex flex-row items-center justify-between space-y-0 pb-2 px-4 md:px-6">
        <Card.Title class="text-sm font-medium">{title}</Card.Title>
    </Card.Header>
    <Card.Content class="pt-0 px-4 md:px-6">
        <div class="md:text-xl text-base font-bold">
            {value}
        </div>
    </Card.Content>
</Card.Root>
