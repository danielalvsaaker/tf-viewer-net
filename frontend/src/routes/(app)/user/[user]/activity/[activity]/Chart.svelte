<script lang="ts">
    import { LayerCake, ScaledSvg, Html, Svg } from 'layercake';
    import AxisX from '$lib/components/charts/AxisX.svelte';
    import type { Activity$result } from '$houdini';
    import Line from '$lib/components/charts/Line.svelte';
    import AxisY from '$lib/components/charts/AxisY.svelte';
    import QuadTree from '$lib/components/charts/QuadTree.svelte';

    export let data: Activity$result;

    export let hover;
    export let visible;
</script>

<div class="h-48 w-full">
    <LayerCake
        ssr={true}
        percentRange={true}
        padding={{ top: 8, right: 10, bottom: 20, left: 30 }}
        data={data.user?.activity?.records}
        x={(d) => d.distance}
        y={(d) => d.altitude}
        xNice
    >
        <ScaledSvg>
            <Line />
        </ScaledSvg>
        <Html>
            <AxisX
                baseline={true}
                gridlines={false}
                formatTick={(d) => `${d} m`}
                tickMarks={true}
            />
            <AxisY
                baseline={true}
                gridlines={false}
                formatTick={(d) => `${d} m`}
                tickMarks={true}
                xTick={-30}
            />
            <QuadTree y="x" let:x let:y bind:visible bind:found={hover} />
        </Html>
    </LayerCake>
</div>
