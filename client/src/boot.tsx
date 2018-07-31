import App from "./App";
import { bootstrap } from "./utils/core";

bootstrap()
  .with(App)
  .does(async () => console.log("Application starts"))
  .mount("#root")
  .start();
