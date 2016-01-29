import {inject} from 'aurelia-framework';
import {ApiService} from 'api-service';
import {EventAggregator} from 'aurelia-event-aggregator';
import {GroupAddedMessage} from 'messages/group-added-message';

@inject(ApiService, EventAggregator)
export class ManageClientGroups {
  heading = 'Manage Client Groups';
  groups = [];

  constructor(api, eventAggregator) {
    this.api = api;
    eventAggregator.subscribe(GroupAddedMessage, message => {
      console.log(this);
      if (message.clientId === this.clientId) {
        this.loadGroups();
      }
    });
  }

  loadGroups() {
    return this.api.get('clients/' + this.clientId + '/groups')
      .then(response => response.json(), response => {
        console.error("Error getting groups for client " + this.clientId + ": " + JSON.stringify(response))
        console.log(response);
        this.errorMessage = "Error getting groups";
      }).then(groups => this.groups = groups);
  }

  activate(params) {
    this.clientId = params.id;

    this.loadGroups();
  }

  configureRouter(config, router) {
    config.map([
      { route: '', name: 'manageGroupsHome', moduleId: 'empty'},
      { route: ':groupId/edit', name: 'editGroup', moduleId: 'edit-group'},
      { route: 'new', name: 'newGroup', moduleId: 'new-group', nav: true, title: "New Group" }
    ]);

    this.router = router;
  }

  isActive(instruction, groupId) {
    return !!instruction && !!instruction.params.groupId && instruction.params.groupId === groupId;
  }
}
