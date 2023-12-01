<script lang="ts">
    import { cn } from '$lib/utils';
    import { Map, type LngLatBoundsLike } from 'maplibre-gl';
    import { onMount } from 'svelte';
    import { Skeleton } from '$lib/components/ui/skeleton';
    import type { GeoJSONSource, GeoJSONSourceSpecification } from 'maplibre-gl';
    import { browser } from '$app/environment';

    let container: HTMLElement;
    let map: Map;
    let skeleton: Skeleton;

    export let geojson: GeoJSONSourceSpecification['data'] | null = null;
    export let bounds: LngLatBoundsLike;
    let className: string | null = null;
    export { className as class };

    let loaded = false;

    $: if (loaded && geojson) {
        const source = map.getSource('records') as GeoJSONSource;
        source.setData(geojson);
        map.fitBounds(bounds, {
            padding: 40
        });
    }

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
            }
        });

        map.on('load', () => {
            skeleton.$destroy();
            loaded = true;

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

            /*
                map.addSource('point', {
                    type: 'geojson',
                    data: {
                        type: 'Point'
                    }
                });

                map.addLayer({
                    id: 'point',
                    source: 'point',
                    type: 'circle',
                    paint: {
                        'circle-radius': 6,
                        'circle-color': '#ffffff',
                        'circle-stroke-color': '#ff0000',
                        'circle-stroke-width': 4
                    },
                    layout: {
                        visibility: 'none'
                    }
                });
                */
        });
        return () => map.remove();
    });
</script>

<div bind:this={container} class={cn('rounded overflow-hidden', className)}>
    <Skeleton bind:this={skeleton} class={className} />
</div>
