import search from "./search.js";

const handler = async function (event, context) {
  console.log(event);
  const body = JSON.parse(event.body);
  const imageUrl = body.imageUrl;

  const result = await search(imageUrl);

  return {
    statusCode: 200,
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(result),
  };
};

export { handler };
