import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class Users {
  heading = 'Users';
  users = [];

  constructor(http) {
    http.configure(config => {
      config
       .useStandardConfiguration()
       .withBaseUrl('https://localhost:44353/api/');
    });

    this.http = http;
  }

  activate() {
    return this.http.fetch('users?page=1&pageSize=25')
       .then(response => response.json())
       .then(response => {         
         this.paging = response.paging;
         this.users = response.items;
       });
  }

  get hasUsers() {
    return this.users.length > 0;
  }
}
