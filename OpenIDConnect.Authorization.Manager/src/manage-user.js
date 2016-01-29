import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class ManageUser {
  heading = 'Manage User';

  constructor(http) {
    http.configure(config => {
       config
         .useStandardConfiguration()
         .withBaseUrl('https://localhost:44392/api/');
    });

    this.http = http;
  }

  activate(params) {
    this.username = params.id;

    return this.http.fetch('users/' + this.username + '/groups')
       .then(response => response.json())
       .then(groups => this.groups = groups);
  }
}
