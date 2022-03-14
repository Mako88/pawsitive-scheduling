<script lang="ts">
import { Grid, Row, Column, TextInput } from "carbon-components-svelte";
import LoadingButton from '$lib/components/LoadingButton.svelte';
import { addUser } from "$lib/backendClient";

let username: string = '';
let password: string = '';
</script>

<Grid>
    <Row>
        <Column>
            <TextInput bind:value={username} labelText="Username" placeholder="Enter a Username" />
        </Column>
        <Column>
            <TextInput bind:value={password} labelText="Password" placeholder="Enter a Password" />
            <LoadingButton execute={async () => await addUser(username, password)} />
        </Column>
    </Row>
</Grid>