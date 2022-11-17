<script lang="ts">
  import { Router, Route } from 'src/Router';
  import { Navbar, SideMenu } from 'src/Components';
  import { Home } from 'src/Pages';
</script>

<main>
  <Navbar />
  <SideMenu />
  <Router>
    <Route path="/" component={Home}/>
  </Router>
</main>
