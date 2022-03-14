<script lang="ts">
  import { Button, Spinner } from "sveltestrap";
  import type { ButtonColor } from "sveltestrap/src/Button";

  export let color: ButtonColor = "primary";
  export let execute: () => Promise<any>;

  let isWaiting: boolean;

  const performExecute = async () => {
    try {
      isWaiting = true;
      return await execute();
    } catch (err) {
      console.log(err);
    } finally {
      isWaiting = false;
    }
  };
</script>

<Button
  {color}
  on:click={async () => await performExecute()}
  disabled={isWaiting}
  style="position: relative"
>
  {#if isWaiting}
    <div style="visibility: hidden;">
      <slot>Submit</slot>
    </div>
    <Spinner
      style="position: absolute;
    left: 0;
    right: 0;
    top: 0;
    bottom: 0;
    margin: auto;
    height: 24px;
    width: 24px;"
    />
  {:else}
    <slot>Submit</slot>
  {/if}
</Button>
