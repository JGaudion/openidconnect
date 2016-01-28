import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class ManageClientGroups {
  heading = 'Manage Client Groups';
  groups = [];

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
    console.log("Activate called...");
    this.groups = [ {
      "id": "group1"
    },
    {
      "id": "group2"
    }]
  }

  configureRouter(config, router) {
    config.map([
      { route: '', name: 'empty', moduleId: 'empty'},
      { route: ':groupId/edit', name: 'editGroup', moduleId: 'edit-group'}
    ]);

    console.log("Configure router called....");

    this.router = router;
  }
}
