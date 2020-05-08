from scrapy.spiders import CrawlSpider, Rule
from scrapy.linkextractors import LinkExtractor

from dailymirror.items import DailymirrorArticle

XPATH_TITLE = '//*[self::h1 or self::h2 or self::h3 or self::h4]//text()[normalize-space()]'
XPATH_DATE = '//div[contains(@class, "well")]//text()[normalize-space()]'
XPATH_COMMENTS = '//div[contains(@class, "comment-views")]//text()[normalize-space()]'
XPATH_VIEWS = '//div[contains(@class, "comment-views")]//text()[normalize-space()]'
XPATH_CONTENT = '//div[contains(@class, "inner-text") or contains(@class,"im-cont")]/p//text()[normalize-space()]'


class ArticleSpider(CrawlSpider):
    name = "articles"
    allowed_domains = ['dailymirror.lk']
    start_urls = ['http://www.dailymirror.lk/news']

    rules = (
        Rule(LinkExtractor(allow=(r'.*\/(article|news)\/.*',), ), callback='parse_item', follow=True),
    )

    def parse_item(self, response):
        item = DailymirrorArticle()
        item['url'] = response.url

        title_selector = response.xpath(XPATH_TITLE)
        date_selector = response.xpath(XPATH_DATE)
        comments_selector = response.xpath(XPATH_COMMENTS)
        views_selector = response.xpath(XPATH_VIEWS)
        item['title'] = title_selector[0].extract() if len(title_selector) > 0 else ''
        item['date'] = date_selector[0].extract() if len(date_selector) > 0 else ''
        item['comments'] = comments_selector[0].extract() if len(comments_selector) > 0 else ''
        item['views'] = views_selector[1].extract() if len(views_selector) > 1 else ''
        item['content'] = ' '.join(response.xpath(XPATH_CONTENT).extract())
        item['keywords'] = []
        yield item
