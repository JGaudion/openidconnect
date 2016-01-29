import {inject} from 'aurelia-framework';
import {ApiService} from 'api-service';

@inject(ApiService)
export class EditGroup {
  heading = 'EditGroup';
  group = {};

  constructor(api) {
    this.api = api;
  }

  activate(params) {
    this.clientId = params.id;

    return this.api.get('clients/' + this.clientId + '/groups/' + params.groupId)
      .then(response => response.json())
      .then(group => this.group = group);
  }
}
