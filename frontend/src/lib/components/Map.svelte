<script lang="ts">
    import { cn } from '$lib/utils';
    import { Map, type LngLatBoundsLike } from 'maplibre-gl';
    import { onMount } from 'svelte';
    import { Skeleton } from '$lib/components/ui/skeleton';
    import type { GeoJSONSourceSpecification } from 'maplibre-gl';

    let container: HTMLElement;
    let map: Map;
    let skeleton: Skeleton;

    export let geojson: GeoJSONSourceSpecification['data'] | null = null;
    export let bounds: LngLatBoundsLike;
    export let interactive = true;
    let className: string | null = null;
    export { className as class };

    onMount(() => {
        map = new Map({
            container,
            style: {
                version: 8,
                sources: {
                    'raster-tiles': {
                        type: 'raster',
                        tiles: ['https://tile.openstreetmap.org/{z}/{x}/{y}.png'],
                        tileSize: 256
                    }
                },
                layers: [
                    {
                        id: 'simple-tiles',
                        type: 'raster',
                        source: 'raster-tiles',
                        minzoom: 0,
                        maxzoom: 22
                    }
                ]
            },
            attributionControl: false,
            bounds,
            fitBoundsOptions: {
                padding: 40
            },
            interactive
        });
        map.on('load', () => {
            skeleton.$destroy();

            if (geojson) {
                map.addSource('records', {
                    type: 'geojson',
                    data: geojson
                });
                map.addLayer({
                    id: 'records',
                    type: 'line',
                    source: 'records',
                    layout: {
                        'line-join': 'round',
                        'line-cap': 'round'
                    },
                    paint: {
                        'line-color': '#ff0000',
                        'line-width': 4
                    }
                });
            }
        });
        return () => map.remove();
    });
</script>

<div bind:this={container} class={cn('rounded overflow-hidden', className)}>
    <Skeleton bind:this={skeleton} class={className} />
</div>
