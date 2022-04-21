<script lang="ts">
  import LoadingButton from "$lib/components/LoadingButton.svelte";
  import { addUser } from "$lib/backendClient";
  import Calendar from "$lib/components/Calendar.svelte";
</script>

<LoadingButton execute={async () => await addUser("test", "test")} />
<Calendar />
