import {inject, singleton} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';

@singleton()
@inject(HttpClient)
export class ApiService {
  constructor(http) {
    http.configure(config => {
      config
        .useStandardConfiguration()
        .withBaseUrl('https://localhost:44392/api/');
      console.log("Configured with https://localhost:44392/api/");
    });

    this.http = http;
  }

  get(uri) {
    return this.http.fetch(uri);
  }

  post(uri, body) {
    return this.http.fetch(uri, {
      method: 'POST',
      body: json(body)
    });
  }

  put(uri, body) {
    return this.http.fetch(uri, {
      method: 'PUT',
      body: json(body)
    });
  }

  delete(uri) {
    return this.http.fetch(uri, {
      method: 'DELETE'
    });
  }
}
