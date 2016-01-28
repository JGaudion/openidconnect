export class App {
  configureRouter(config, router) {
    config.title = 'Authorization Manager';
    config.map([
      { route: ['', 'clients'], name: 'clients', moduleId: 'clients', nav: true, title: 'Clients' },
      { route: 'client/:id/manage', name: 'manageClient', moduleId: 'manage-client', title: 'Manage Client' },
      { route: 'client/new', name: 'newClient', moduleId: 'edit-client', nav: true, title: 'New Client'}
    ]);

    this.router = router;
  }
}
