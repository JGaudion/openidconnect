import {ApiService} from 'api-service';
import {inject} from 'aurelia-framework';

@inject(ApiService)
export class ManageClient {
  heading = 'ManageClient';
  clientId = "";

  constructor(api) {
    this.api = api;
  }

  activate(params) {
    this.clientId = params.id;

    return this.api.get('clients/' + this.clientId)
      .then(response => response.json())
      .then(client => this.client = client);
  }

  configureRouter(config, router) {
    config.map([
      { route: ['', 'edit'], name: 'editClient', moduleId: 'edit-client', nav: true, title: 'Edit Client' },
      { route: 'groups', name: 'manageGroups', moduleId: 'manage-groups', nav: true, title: 'Manage Groups' },
      { route: 'users', name: 'manageUsers', moduleId: 'manage-users', nav: true, title: 'Manage Users' },
    ]);

    this.router = router;
  }
}
