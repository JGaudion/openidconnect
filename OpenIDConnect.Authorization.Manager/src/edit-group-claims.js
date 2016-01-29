export class EditGroupClaims {
  allowUnkownClaims = true;
  existingClaims = [];

  constructor() {
  }

  activate() {
    this.existingClaims = [
      {
        id: 1,
        type: "can_read_news",
        value: "true"
      },
      {
        id: 2,
        type: "can_edit_news",
        value: "false"
      }
    ];
  }

  addNewClaim(newClaim) {
    var updatedClaims = this.existingClaims.slice();
    updatedClaims[updatedClaims.length] =
      {
        id: updatedClaims.length,
        type: newClaim.type,
        value: newClaim.value
      };
    this.existingClaims = updatedClaims;
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
