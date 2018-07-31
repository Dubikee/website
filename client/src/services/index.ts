import { HttpClient } from "./HttpClient/HttpClient";

export { HttpClient };

export let ServicesStore = {
  httpClient: new HttpClient()
};

export let ServiceTypes = {
  HttpClient: "httpClient"
};
