from collections import Counter
from math import log10

doc1 = open("./docs/document1.txt", "r")
doc2 = open("./docs/document2.txt", "r")
doc3 = open("./docs/document3.txt", "r")

doc1_content = doc1.read()
doc2_content = doc2.read()
doc3_content = doc3.read()

doc1_tokens = [s.lower().strip() for s in doc1_content.split()]
doc2_tokens = [s.lower().strip() for s in doc2_content.split()]
doc3_tokens = [s.lower().strip() for s in doc3_content.split()]
# TF
doc1_tf = Counter(dict((k, round(1 + log10(v), 3)) for k, v in Counter(doc1_tokens).items()))
doc2_tf = Counter(dict((k, round(1 + log10(v), 3)) for k, v in Counter(doc2_tokens).items()))
doc3_tf = Counter(dict((k, round(1 + log10(v), 3)) for k, v in Counter(doc3_tokens).items()))
# CF
cf = doc1_tf + doc2_tf + doc3_tf
# IDF
doc_idf = dict(
    (x, round(log10(3 / (int(x in doc1_tf.keys()) + int(x in doc2_tf.keys()) + int(x in doc3_tf.keys()))), 3))
    for x in cf.keys()
)

# TF-IDF
doc1_tf_idf = Counter(dict((k, v * doc_idf.get(k)) for k, v in doc1_tf.items()))
doc2_tf_idf = Counter(dict((k, v * doc_idf.get(k)) for k, v in doc2_tf.items()))
doc3_tf_idf = Counter(dict((k, v * doc_idf.get(k)) for k, v in doc3_tf.items()))

# Answer
index_no = 65
doc1_term = sorted(doc1_tf.keys(), key=lambda x: x[0])[index_no]
doc2_term = sorted(doc2_tf.keys(), key=lambda x: x[0])[index_no]
doc3_term = sorted(doc3_tf.keys(), key=lambda x: x[0])[index_no]

# Print
print("140265D")
print("1")
print("document1:" + str(len(doc1_tf.keys())))
print("document2:" + str(len(doc2_tf.keys())))
print("document3:" + str(len(doc3_tf.keys())))
print()
print(2)
print("document1:" + str(doc1_term) + "," + str(doc1_tf.get(doc1_term)))
print("document2:" + str(doc2_term) + "," + str(doc2_tf.get(doc2_term)))
print("document3:" + str(doc3_term) + "," + str(doc3_tf.get(doc3_term)))
print()
print(3)
print("document1:" + str(doc1_term) + "," + str(doc_idf.get(doc1_term)))
print("document2:" + str(doc2_term) + "," + str(doc_idf.get(doc2_term)))
print("document3:" + str(doc3_term) + "," + str(doc_idf.get(doc3_term)))
print()
print(4)
print("document1:" + ",".join([x[0] for x in doc1_tf_idf.most_common(10)]))
print("document2:" + ",".join([x[0] for x in doc2_tf_idf.most_common(10)]))
print("document3:" + ",".join([x[0] for x in doc3_tf_idf.most_common(10)]))
