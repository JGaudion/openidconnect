import {inject} from 'aurelia-framework';
import {ApiService} from 'api-service';

@inject(ApiService)
export class EditGroupUsers {
  users = [];

  constructor(api) {
    this.api = api;
  }

  activate(params) {
    this.groupId = params.groupId;
    this.clientId = params.id;

    this.updateCurrentUsers();

    this.api.get('users?page=1&pageSize=10')
      .then(response => response.json())
      .then(users => this.allUsers = users.items);
  }

  updateCurrentUsers() {
    return this.api.get('clients/' + this.clientId + '/groups/' + this.groupId + '/users?page=1&pageSize=10')
      .then(response => response.json(), response => console.error(response))
      .then(users => {
        this.currentUsers = users.items;
      });
  }

  addUser(user) {
    this.api.post('clients/' + this.clientId + '/groups/' + this.groupId + '/users', user)
      .then(response => {
        console.log("User added!");
        this.updateCurrentUsers();
      }, response => console.log("Failed to add user"));
  }

  removeUser(user) {
    this.api.delete('clients/' + this.clientId + '/groups/' + this.groupId + '/users/' + user.id)
      .then(response => {
        console.log("User deleted!");
        this.updateCurrentUsers();
      }, response => console.log("Failed to delete user"));
  }
}
