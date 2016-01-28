import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient, Router)
export class EditClient {
  heading = 'Edit Client';
  submitText = 'Update Client';

  name = "";
  id = "";
  enabled = true;
  claimsUri = "";

  isNewClient = false;
  clientAdded = false;

  constructor(http, router) {
    http.configure(config => {
      config
        .useStandardConfiguration()
        .withBaseUrl('https://localhost:44392');
    });

    this.http = http;
    this.router = router;
  }

  activate(params) {
    this.id = params.id;
    this.isNewClient = !this.id;

    if (this.isNewClient) {
      this.heading = "New Client";
      this.submitText = "Add Client";
    } else {
      return this.http.fetch('/api/clients/' + this.id)
        .then(response => response.json(), response => {
          console.error("Error getting client " + this.id + ": " + JSON.stringify());
          this.errorMessage = "Error getting client " + this.id;
        })
        .then(client => {
          this.name = client.name;
          this.enabled = client.enabled;
          this.claimsUri = client.claimsUri;
        });
    }
  }

  submit() {
    var client = {
      id: this.id,
      name: this.name,
      enabled: this.enabled,
      claimsUri: this.claimsUri
    };
    if (this.isNewClient) {
      this.http.fetch('/api/clients', {
        method: 'post',
        body: json(client)
      })
        .then(response => {
          this.clientAdded = true;
        }, response => {
          console.error("Error creating client: " + JSON.stringify(response));
          this.errorMessage = "Error creating client";
        });
    } else {
      this.http.fetch('/api/clients/' + this.id, {
        method: 'put',
        body: json(client)
      })
        .then(response => {
          console.log("Client updated!");
        }, response => {
          console.error("Error updating client " + this.id + ": " + JSON.stringify(response));
          this.errorMessage = "Error updating client";
        });
    }
  }

  manage() {
    this.router.navigateToRoute('manageClient', { id: this.id });
  }
}
