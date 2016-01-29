import {inject} from 'aurelia-framework';
import {ApiService} from 'api-service';

@inject(ApiService)
export class Clients {
  heading = 'Clients';
  clients = [];

  constructor(api) {
    this.api = api;
  }

  activate() {
    return this.api.get('clients')
       .then(response => response.json())
       .then(clients => this.clients = clients);
  }

  get hasClients() {
    return this.clients.length > 0;
  }
}
