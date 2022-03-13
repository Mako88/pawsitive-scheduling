<script lang="ts">
    import LayoutGrid, { Cell } from '@smui/layout-grid';
    import Textfield from '@smui/textfield';
    import HelperText from '@smui/textfield/helper-text';
    import Button from '@smui/button';

    let username = '';
    let password = '';

    const addUser = async () => {
        
    };
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
        <Button on:click={addUser}>Test</Button>
    </Cell>
</LayoutGrid>