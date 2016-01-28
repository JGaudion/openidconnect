export class ManageClientUsers {
  heading = 'Manage Client Users';

  constructor() {
  }

  activate(params) {
    this.clientId = params.id;
  }
}
