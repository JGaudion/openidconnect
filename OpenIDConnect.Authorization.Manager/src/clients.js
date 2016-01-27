import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class Clients {
  heading = 'Clients';
  users = [];

  constructor(http) {
    http.configure(config => {
      // config
      //   .useStandardConfiguration()
      //   .withBaseUrl('https://api.github.com/');
    });

    this.clients = [
      {
        id: "angular14"
      },
      {
        id: "AngularMaterial"
      }
    ]

    this.http = http;
  }

  activate() {
    // return this.http.fetch('users')
    //   .then(response => response.json())
    //   .then(users => this.users = users);
  }
}
