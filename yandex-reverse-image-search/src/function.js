import search from "./search.js";

const handler = async function (event, context) {
  const bodyJson = Buffer.from(event.body, "base64").toString("utf8");
  console.log(bodyJson);
  const body = JSON.parse(bodyJson);
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
