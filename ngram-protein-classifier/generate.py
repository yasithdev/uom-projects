import re

import numpy as np
import pandas as pd
from sklearn.feature_extraction.text import CountVectorizer

pd.set_option('display.max_columns', 500)
pd.set_option('display.width', 1000)

ds = pd.read_csv("data/dataset_clean_p1.csv")

# Remove class labels with low frequencies
ds_vc = ds['Classification'].value_counts()
ds_vc = ds_vc[(ds_vc >= ds_vc.mean() + ds_vc.std())].head()
print(ds_vc)
ds_clean = ds[ds['Classification'].isin(ds_vc.index)]

# Data Cleaning
ds_clean['Sequence'] = ds_clean['Sequence'].map(lambda x: re.sub(r'([^A-Z]|\s)+', '-', str(x).upper()))

# Sample Dataset
ds_sampled = ds_clean.groupby('Classification')
ds_sampled = pd.DataFrame(ds_sampled.apply(lambda x: x.sample(ds_sampled.size().min())).reset_index(drop=True))
ds_sampled.insert(len(ds_sampled.columns) - 1, 'Sequence Length', ds_sampled['Sequence'].apply(lambda x: len(x)))
print(ds_sampled)

# Feature expansion
V = CountVectorizer(lowercase=False, ngram_range=(2, 2), tokenizer=lambda x: list(x), dtype=np.int)
X = V.fit_transform(ds_sampled['Sequence'].tolist())
print(len(V.get_feature_names()))

vec_df = pd.DataFrame(X.toarray(), columns=V.get_feature_names())
ds_sampled = ds_sampled.drop(['Sequence'], axis=1)
df = pd.merge(left=vec_df, right=ds_sampled, left_index=True, right_index=True).drop_duplicates()

# Postprocessing
df['Classification'] = df['Classification'].apply(lambda x: str(x).replace(',', '&'))

# Write out
df.to_csv("data/dataset_out.csv", index=False)
