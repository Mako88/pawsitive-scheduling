<script lang="ts">
  import "@fullcalendar/core/vdom";
  import { Calendar } from "@fullcalendar/core";
  import interactionPlugin from "@fullcalendar/interaction";
  import resourceTimeGridPlugin from "@fullcalendar/resource-timegrid";
  import { onMount } from "svelte";

  let calendar;
  let calendarEl;

  onMount(async () => {
    calendar = new Calendar(calendarEl, {
      plugins: [interactionPlugin, resourceTimeGridPlugin],
      initialView: "timeGridDay",
      headerToolbar: {
        left: "prev,next today",
        center: "title",
        right: "dayGridMonth,timeGridWeek,listWeek",
      },
      schedulerLicenseKey: "CC-Attribution-NonCommercial-NoDerivatives",
    });

    calendar.render();
  });
</script>

<div bind:this={calendarEl} />
