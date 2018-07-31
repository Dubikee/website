import { HttpClient } from './HttpClient';

test("httpclient test", () => {
   let client = new HttpClient();
   expect(client.num).toEqual(12);
})