import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class ManageClientUsers {
  heading = 'Manage Client Users';

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
}
