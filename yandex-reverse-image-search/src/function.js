import search from "./search.js";

const handler = async function (event, context) {
  const body = JSON.parse(event.body);
  const imageUrl = body.imageUrl;

  const result = await search(imageUrl);

  return {
    statusCode: 200,
    headers: {
      "Content-Type": "text/plain",
    },
    body: JSON.stringify(result),
  };
};

export { handler };
