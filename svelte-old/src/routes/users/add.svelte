<script lang="ts">
  import {
    Form,
    FormGroup,
    Input,
    Label,
    Container,
    Row,
    Col,
  } from "sveltestrap";
  import LoadingButton from "$lib/components/LoadingButton.svelte";
  import { addUser } from "$lib/backendClient";

  let username: string = "";
  let password: string = "";
</script>

<Form>
  <Container>
    <Row>
      <Col>
        <FormGroup floating label="Username">
          <Input bind:value={username} placeholder="Enter a Username" />
        </FormGroup>
      </Col>
      <Col>
        <FormGroup floating label="Password">
          <Input bind:value={password} placeholder="Enter a Password" />
        </FormGroup>
        <LoadingButton
          execute={async () => await addUser(username, password)}
        />
      </Col>
    </Row>
  </Container>
</Form>
