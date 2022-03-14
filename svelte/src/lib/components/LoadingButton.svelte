<script lang="ts">
    import { Button, InlineLoading } from "carbon-components-svelte";

    export let execute: () => Promise<any>;

    let isWaiting: boolean;

    const performExecute = async () => {
        try {
            isWaiting = true;
            return await execute();
        }
        catch (err) {
            console.log(err);
        }
        finally {
            isWaiting = false;
        }
    };

</script>


<Button on:click={async () => await performExecute()} disabled={isWaiting}>
    {#if isWaiting}
        <slot>Submit</slot>
        <InlineLoading style="margin-left: 10px;" />
    {:else}
        <slot>Submit</slot>
    {/if}
</Button>