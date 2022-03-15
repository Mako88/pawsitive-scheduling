<script lang="ts">
    import { Calendar } from "@fullcalendar/core";
    import dayGridPlugin from  "@fullcalendar/daygrid";
    import interactionPlugin from "@fullcalendar/interaction"
    import timeGridPlugin from "@fullcalendar/timegrid"
    import scrollGridPlugin from "@fullcalendar/scrollgrid";
    import { onMount } from "svelte";

    let calendar;
    let calendarEl;

    onMount(async () => {
        await import("@fullcalendar/core/vdom");

        calendar = new Calendar(calendarEl, {
            plugins: [ dayGridPlugin, timeGridPlugin, interactionPlugin, scrollGridPlugin ],
            initialView: 'timeGridDay',
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,listWeek'
            }
        });

        calendar.render();
    });
    
</script>

<div bind:this={calendarEl} />