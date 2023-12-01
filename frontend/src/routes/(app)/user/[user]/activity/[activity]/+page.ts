import { error } from "@sveltejs/kit";
import type { AfterLoadEvent } from "./$houdini";

export const _houdini_afterLoad = ({ data }: AfterLoadEvent) => {
    if (!data.Activity.user?.activity) {
        throw error(404);
    }
};