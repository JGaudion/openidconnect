import {inject} from 'aurelia-framework';
import {ApiService} from 'api-service';

@inject(ApiService)
export class EditGroupClaims {
  allowUnkownClaims = true;
  existingClaims = [];

  constructor(api) {
    this.api = api;
  }

  activate(params) {
    this.clientId = params.id;
    this.groupId = params.groupId;

    return this.api.get('clients/' + params.id + '/groups/' + params.groupId + '/claims')
      .then(response => response.json())
      .then(claims => this.existingClaims = claims);
  }

  addNewClaim(newClaim) {

    this.api.post('clients/' + this.clientId + '/groups/' + this.groupId + '/claims', newClaim)
      .then(response => response.json())
      .then(addedClaim => {
        var updatedClaims = this.existingClaims.slice();
        updatedClaims[updatedClaims.length] =
          {
            id: addedClaim.id,
            type: addedClaim.type,
            value: addedClaim.value
          };
        this.existingClaims = updatedClaims;
      });
  }

  updateClaim(updatedClaim) {

    var existingClaim = this.existingClaims.find(c => c.id == updatedClaim.id);
    existingClaim.type = updatedClaim.type;
    existingClaim.value = updatedClaim.value;
  }

  claimModified(existingClaim, newClaim) {
    return existingClaim.type !== newClaim.type || existingClaim.value !== newClaim.value;
  }
}
