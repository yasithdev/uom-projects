# -*- coding: utf-8 -*-

# Define here the models for your scraped items
#
# See documentation in:
# https://doc.scrapy.org/en/latest/topics/items.html

import scrapy


class DailymirrorArticle(scrapy.Item):
    # define the fields for your item here like:
    url = scrapy.Field()
    title = scrapy.Field()
    date = scrapy.Field()
    comments = scrapy.Field()
    views = scrapy.Field()
    content = scrapy.Field()
    keywords = scrapy.Field()

