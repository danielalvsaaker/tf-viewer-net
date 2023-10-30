<script lang="ts">
    import * as DropdownMenu from '$lib/components/ui/dropdown-menu';
    import * as Avatar from '$lib/components/ui/avatar';
    import { Button } from '$lib/components/ui/button';
    import { page } from '$app/stores';
    import { signOut } from '@auth/sveltekit/client';

    let user = $page.data.session?.user;
</script>

<DropdownMenu.Root positioning={{ placement: 'bottom-end' }}>
    <DropdownMenu.Trigger asChild let:builder>
        <Button variant="ghost" builders={[builder]} class="relative h-8 w-8 rounded-full">
            <Avatar.Root class="h-8 w-8">
                <Avatar.Image src={user?.image} />
                <Avatar.Fallback>{user?.name?.slice(0, 1).toUpperCase()}</Avatar.Fallback>
            </Avatar.Root>
        </Button>
    </DropdownMenu.Trigger>
    <DropdownMenu.Content class="w-56">
        <DropdownMenu.Label class="font-normal">
            <div class="flex flex-col space-y-1">
                <p class="text-sm font-medium leading-none">{user?.name}</p>
                <p class="text-xs leading-none text-muted-foreground">{user?.email}</p>
            </div>
        </DropdownMenu.Label>
        <DropdownMenu.Separator />
        <DropdownMenu.Group>
            <a href="/">
                <DropdownMenu.Item>
                    Profile
                    <DropdownMenu.Shortcut>⇧⌘P</DropdownMenu.Shortcut>
                </DropdownMenu.Item>
            </a>
            <DropdownMenu.Item>
                Settings
                <DropdownMenu.Shortcut>⌘S</DropdownMenu.Shortcut>
            </DropdownMenu.Item>
        </DropdownMenu.Group>
        <DropdownMenu.Separator />
        <DropdownMenu.Item on:click={() => signOut()}>
            Log out
            <DropdownMenu.Shortcut>⇧⌘Q</DropdownMenu.Shortcut>
        </DropdownMenu.Item>
    </DropdownMenu.Content>
</DropdownMenu.Root>
