<script lang="ts">
    import 'carbon-components-svelte/css/all.css';
    import {
        Header,
        SideNav,
        SideNavItems,
        SideNavMenu,
        SideNavMenuItem,
        SideNavLink,
        SideNavDivider,
        SkipToContent,
        Content,
    } from 'carbon-components-svelte';   
</script>
  
  <Header company="Pawsitivity" platformName="Scheduling">
  </Header>
  
  <SideNav isOpen={true}>
    <SideNavItems>
      <SideNavLink href="/" text="Home" />
      <SideNavLink href="/users/add" text="Add User" />
      <SideNavLink text="Link 3" />
      <SideNavMenu text="Menu">
        <SideNavMenuItem href="/" text="Link 1" />
        <SideNavMenuItem href="/" text="Link 2" />
        <SideNavMenuItem href="/" text="Link 3" />
      </SideNavMenu>
      <SideNavDivider />
      <SideNavLink text="Link 4" />
    </SideNavItems>
  </SideNav>
  
  <Content>
    <slot></slot>
  </Content>
  