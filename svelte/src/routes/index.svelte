<script lang="ts">
    import { Container, Row, Col, Input } from "sveltestrap";
    import Calendar from "$lib/components/Calendar.svelte";


    const groomers = [
        {id: -1, value: "All Groomers"},
        {id: 1, value: "Olivia"},
        {id: 2, value: "Sid"}
    ];

    let selected = groomers[0].id;
</script>

<Container>
    <Row>
        <Col xs="10">
        </Col>
        <Col xs="2">
            <Input type="select" bind:value={selected}>
                {#each groomers as groomer}
                    <option value={groomer.id}>{groomer.value}</option>
                {/each}
            </Input>
        </Col>
    </Row>
    <Row>
        <Col xs="12">
            <Calendar />
        </Col>
    </Row>
</Container>