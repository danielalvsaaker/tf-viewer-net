import StaticMaps, { type AddLineOptions } from 'staticmaps';

export interface Activity {
    readonly boundingBox: {
        readonly bbox: number[];
    };
    readonly records: {
        position: {
            coordinates: [number, number];
        } | null;
    }[];
}

export interface Dimensions {
    readonly width: number;
    readonly height: number;
}

function activityToPolyline(activity: Activity): AddLineOptions {
    return {
        coords: activity.records
            .filter((record) => record.position)
            .map((record) => record.position!.coordinates),
        color: '#ff0000',
        width: 4
    };
}

export async function generateMap(
    activity: Activity,
    { width, height }: Dimensions
): Promise<Buffer> {
    const map = new StaticMaps({
        width,
        height
    });

    map.addLine(activityToPolyline(activity));

    await map.render();
    return await map.image.buffer();
}
