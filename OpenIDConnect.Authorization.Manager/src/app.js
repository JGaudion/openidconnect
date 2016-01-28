export class App {
  configureRouter(config, router) {
    config.title = 'Authorization Manager';
    config.map([
      { route: '', name: 'home', moduleId: 'home', nav: false },
      { route: 'clients', name: 'clients', moduleId: 'clients', nav: true, title: 'Clients' },
      { route: 'users', name: 'users', moduleId: 'users', nav: true, title: 'Users' },
      { route: 'client/:id/manage', name: 'manageClient', moduleId: 'manage-client', title: 'Manage Client' },
    ]);

    this.router = router;
  }
}
