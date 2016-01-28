import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {ApiService} from 'api-service';
import {EventAggregator} from 'aurelia-event-aggregator';
import {GroupAddedMessage} from 'messages/group-added-message';

@inject(ApiService, Router, EventAggregator)
export class NewGroup {
  name = "";

  groupAdded = false;
  constructor(api, router, eventAggregator) {
    this.api = api;
    this.router = router;
    this.eventAggregator = eventAggregator;
  }

  activate(params) {
    this.clientId = params.id;
  }

  submit() {

    var group = {
      name: this.name,
      id: ""
    };
    this.api.post('clients/' + this.clientId + '/groups', group)
      .then(response => {
        console.log("Group added!");
        this.groupAdded = true;
        this.eventAggregator.publish(new GroupAddedMessage(group, this.clientId));
        this.router.navigateToRoute('manageGroupsHome');
      }, response => {
        console.error("Error updating adding group to client " + this.clientId + ": " + JSON.stringify(response));
        this.errorMessage = "Error updating group";
      });
  }
}
