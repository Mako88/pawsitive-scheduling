<script lang="ts">
  import {
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
    Collapse,
    Dropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
  } from "sveltestrap";

  let isOpen = false;

  const openUpdate = (event) => {
    isOpen = event.detail.isOpen;
  };
</script>

<Navbar color="primary" dark expand="md">
  <NavbarBrand href="/">Pawsitivity Scheduling</NavbarBrand>
  <NavbarToggler on:click={() => (isOpen = !isOpen)} />
  <Collapse {isOpen} navbar expand="md" on:update={openUpdate}>
    <Nav class="ms-auto" navbar>
      <NavItem>
        <NavLink href="/">Home</NavLink>
      </NavItem>
      <NavItem>
        <NavLink href="/users/add">Add User</NavLink>
      </NavItem>
      <Dropdown nav inNavbar>
        <DropdownToggle nav caret>Options</DropdownToggle>
        <DropdownMenu end>
          <DropdownItem>Option 1</DropdownItem>
          <DropdownItem>Option 2</DropdownItem>
          <DropdownItem divider />
          <DropdownItem>Reset</DropdownItem>
        </DropdownMenu>
      </Dropdown>
    </Nav>
  </Collapse>
</Navbar>

<slot />
