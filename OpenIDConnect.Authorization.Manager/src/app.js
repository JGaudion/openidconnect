export class App {
  configureRouter(config, router) {
    config.title = 'Authentication Manager';
    config.map([
      { route: ['', 'clients'], name: 'clients',      moduleId: 'clients',      nav: true, title: 'Clients' },
    ]);

    this.router = router;
  }
}
