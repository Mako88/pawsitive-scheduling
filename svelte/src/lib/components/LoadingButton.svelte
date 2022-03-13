<script lang="ts">
import Button from "@smui/button";
import CircularProgress from "@smui/circular-progress";

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


<Button on:click={async () => await performExecute()} variant="raised" disabled={isWaiting}>
    {#if isWaiting}
        <span style="visibility: hidden">
            <slot>Submit</slot>
        </span>
        <CircularProgress style="height: 32px; width: 32px; position: absolute;" indeterminate />
    {:else}
        <slot>Submit</slot>
    {/if}
</Button>