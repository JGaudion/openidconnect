import {inject} from 'aurelia-framework';
import {ApiService} from 'api-service';

@inject(ApiService)
export class Users {
  heading = 'Users';
  users = [];

  constructor(api) {
    this.api = api;
  }

  activate() {
    return this.api.get('users?page=1&pageSize=25')
       .then(response => response.json())
       .then(users => this.users = users);
  }

  get hasUsers() {
    return this.users.length > 0;
  }
}
