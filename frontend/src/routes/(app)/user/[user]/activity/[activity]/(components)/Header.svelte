<script lang="ts">
    import { fragment, graphql, type ActivityHeader } from '$houdini';
    import { DateTime } from 'luxon';

    export let activity: ActivityHeader;
    $: data = fragment(activity, graphql(`
        fragment ActivityHeader on Activity {
            userId
            activityId
            startTime
            previous {
                activityId
            }
            next {
                activityId
            }
        }
    `));
</script>

<div class="flex justify-between items-center mb-6">
    <div class="text-right">
        <a
            href={$data.previous
                ? `/user/${$data.userId}/activity/${$data.previous.activityId}`
                : undefined}
        >
            <svg
                class="w-5 h-5 m-auto {$data.previous
                    ? 'dark:fill-white fill-slate-900'
                    : 'fill-slate-400'}"
                viewBox="0 0 20 20"
            >
                <path
                    clip-rule="evenodd"
                    fill-rule="evenodd"
                    d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z"
                />
            </svg>
        </a>
    </div>
    <div class="col-span-4 text-center text-sm md:text-lg font-semibold">
        <h1>{$data.startTime.toLocaleString(DateTime.DATETIME_MED_WITH_WEEKDAY)}</h1>
    </div>
    <div class="text-left">
        <a
            href={$data.next
                ? `/user/${$data.userId}/activity/${$data.next.activityId}`
                : undefined}
        >
            <svg
                class="w-5 h-5 m-auto {$data.next
                    ? 'dark:fill-white fill-slate-900'
                    : 'fill-slate-400'}"
                viewBox="0 0 20 20"
            >
                <path
                    clip-rule="evenodd"
                    fill-rule="evenodd"
                    d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
                />
            </svg>
        </a>
    </div>
</div>