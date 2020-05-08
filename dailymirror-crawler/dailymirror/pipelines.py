# -*- coding: utf-8 -*-

# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://doc.scrapy.org/en/latest/topics/item-pipeline.html
import json
import re

from scrapy.exceptions import DropItem
from unidecode import unidecode

RE_GARBAGE = re.compile(r'[^\S -~]')


class DailymirrorPipeline(object):
    keywords = []

    def __init__(self) -> None:
        super().__init__()
        with open('./keywords', 'r') as doc:
            self.keywords = [x.strip() for x in doc.read().split('\n')]

    def process_item(self, item, spider):
        """
        Clean the item fields for better utilization of data
        :type spider: scrapy.spiders.Spider
        :type item: dailymirror.items.DailymirrorArticle
        """
        # Clean each field of items and return it
        item['url'] = RE_GARBAGE.sub('', unidecode(item['url'])).replace(r'\s+', ' ').strip()
        item['content'] = RE_GARBAGE.sub('', unidecode(item['content'])).replace(r'\s+', ' ').strip()
        item['title'] = RE_GARBAGE.sub('', unidecode(item['title'])).replace(r'\s+', ' ').strip()
        item['comments'] = RE_GARBAGE.sub('', unidecode(item['comments'])).replace(r'\s+', ' ').strip()
        item['date'] = RE_GARBAGE.sub('', unidecode(item['date'])).replace(r'\s+', ' ').strip()
        item['views'] = RE_GARBAGE.sub('', unidecode(item['views'])).replace(r'\s+', ' ').strip()
        for x in self.keywords:
            if x in item['content'] or x in item['title']:
                return item
        raise DropItem(item)


class WriteToFilePipeline(object):
    i = 1

    def process_item(self, item, spider):
        """
        Clean the item fields for better utilization of data
        :type spider: scrapy.spiders.Spider
        :type item: dailymirror.items.DailymirrorArticle
        """
        # Save file as JSON in folder
        with open('./docs/' + str(item['title']) + '.json', 'w') as doc:
            print(item['url'])
            doc.write(json.dumps(item.__dict__['_values'], sort_keys=True, indent=4, separators=(',', ': ')))
            self.i += 1
