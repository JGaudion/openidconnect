import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {ApiService} from 'api-service';

@inject(ApiService, Router)
export class EditClient {
  heading = 'Edit Client';
  submitText = 'Update Client';

  name = "";
  id = "";
  enabled = true;
  claimsUri = "";

  isNewClient = false;
  clientAdded = false;

  constructor(api, router) {
    this.api = api;
    this.router = router;
  }

  activate(params) {
    this.id = params.id;
    this.isNewClient = !this.id;

    if (this.isNewClient) {
      this.heading = "New Client";
      this.submitText = "Add Client";
    } else {
      return this.api.get('clients/' + this.id)
        .then(response => response.json(), response => {
          console.error("Error getting client " + this.id + ": " + JSON.stringify(response));
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
      this.api.post('clients', client)
        .then(response => {
          this.clientAdded = true;
        }, response => {
          console.error("Error creating client: " + JSON.stringify(response));
          this.errorMessage = "Error creating client";
        });
    } else {
      this.api.put('clients/' + this.id, client)
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
