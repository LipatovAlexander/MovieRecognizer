import { CheerioCrawler, Configuration } from "crawlee";

export default async function search(imageUrl) {
  const startUrls = [
    `https://yandex.com/images/search/?url=${imageUrl}&rpt=imageview`,
  ];

  const crawler = new CheerioCrawler(
    {
      requestHandler: async ({ $, pushData, log }) => {
        let knowledge_graph = [];

        $(".CbirObjectResponse").each(function () {
          log.info("knowledge graph");

          const title = $(this).find(".CbirObjectResponse-Title").text().trim();
          const subtitle = $(this)
            .find(".CbirObjectResponse-Subtitle")
            .text()
            .trim();
          const description = $(this)
            .find(".CbirObjectResponse-Description")
            .text()
            .trim();
          const link = $(this)
            .find(".CbirObjectResponse-SourceLink")
            .attr("href")
            .trim();
          const thumbnail = $(this)
            .find(".MMImage.Thumb-Image")
            .attr("src")
            .trim();
          const source = $(this)
            .find(".CbirObjectResponse-Source")
            .text()
            .trim();

          knowledge_graph.push({
            title: title,
            subtitle: subtitle,
            description: description,
            link: link,
            source: source,
            thumbnail: thumbnail,
          });
        });

        await pushData({
          knowledge_graph,
        });
      },
    },
    new Configuration({
      persistStorage: false,
    }),
  );

  await crawler.run(startUrls);

  const data = await crawler.getData();
  const item = data.items[0];

  return item;
}
