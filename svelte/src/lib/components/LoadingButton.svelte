<script lang="ts">
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

<button on:click={async () => await performExecute()} disabled={isWaiting}>
  {#if isWaiting}
    <div style="visibility: hidden;">
      <slot>Submit</slot>
    </div>
    <div class="loader small absolute no-margin spinner" disabled />
  {:else}
    <slot>Submit</slot>
  {/if}
</button>

<style>
  .spinner {
    border-color: var(--on-primary);
  }
</style>
