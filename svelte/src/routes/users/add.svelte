<script lang="ts">
import LayoutGrid, { Cell } from '@smui/layout-grid';
import Textfield from '@smui/textfield';
import HelperText from '@smui/textfield/helper-text';
import LoadingButton from '$lib/components/LoadingButton.svelte';
import { addUser } from "$lib/backendClient";

let username: string = '';
let password: string = '';
</script>

<LayoutGrid>
    <Cell span={6}>
        <Textfield variant="outlined" bind:value={username} label="Username">
            <HelperText slot="helper">Enter a Username</HelperText>
        </Textfield>
    </Cell>
    <Cell span={6}>
        <Textfield variant="outlined" bind:value={password} label="Password">
            <HelperText slot="helper">Enter a Password</HelperText>
        </Textfield>
        <LoadingButton execute={async () => await addUser(username, password)} />
    </Cell>
</LayoutGrid>