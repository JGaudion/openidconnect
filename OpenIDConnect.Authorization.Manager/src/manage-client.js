import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class ManageClient {
  heading = 'ManageClient';
  clientId = "";

  constructor(http) {
    http.configure(config => {
      // config
      //   .useStandardConfiguration()
      //   .withBaseUrl('https://api.github.com/');
    });

    this.http = http;
  }

  activate(params) {
    this.clientId = params.id;

    // return this.http.fetch('users')
    //   .then(response => response.json())
    //   .then(users => this.users = users);
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
