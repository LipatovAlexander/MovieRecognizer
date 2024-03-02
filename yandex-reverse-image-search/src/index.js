import search from "./search.js";

const imageUrl =
  "https://storage.yandexcloud.net/movie-recognizer/tmp2h3XiK.png";
const result = await search(imageUrl);

console.log(result);
