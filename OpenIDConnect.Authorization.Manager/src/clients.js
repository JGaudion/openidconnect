import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class Clients {
  heading = 'Clients';
  clients = [];

  constructor(http) {
    http.configure(config => {
      config
       .useStandardConfiguration()
       .withBaseUrl('https://localhost:44392/api/');
    });

    this.http = http;
  }

  activate() {
    return this.http.fetch('clients')
       .then(response => response.json())
       .then(clients => this.clients = clients);
  }

  get hasClients() {
    return this.clients.length > 0;
  }
}
