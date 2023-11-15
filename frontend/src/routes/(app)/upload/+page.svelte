<script lang="ts">
    import { graphql } from '$houdini';

    let files: FileList;

    const UploadActivity = graphql(`
        mutation UploadActivity($file: Upload!) {
            uploadActivity(input: { file: $file }) {
                activity {
                    activityId
                }
            }
        }
    `);
</script>

<input
    type="file"
    multiple
    bind:files
    on:change={() =>
        Array.from(files).forEach((file) => {
            UploadActivity.mutate({ file });
        })}
/>
