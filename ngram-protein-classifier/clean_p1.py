import numpy as np
import pandas as pd


def levenshtein(seq1, seq2) -> int:
    size_x = len(seq1) + 1
    size_y = len(seq2) + 1
    matrix = np.zeros((size_x, size_y))
    for x in range(size_x):
        matrix[x, 0] = x
    for y in range(size_y):
        matrix[0, y] = y

    for x in range(1, size_x):
        for y in range(1, size_y):
            if seq1[x - 1] == seq2[y - 1]:
                matrix[x, y] = min(
                    matrix[x - 1, y] + 1,
                    matrix[x - 1, y - 1],
                    matrix[x, y - 1] + 1
                )
            else:
                matrix[x, y] = min(
                    matrix[x - 1, y] + 1,
                    matrix[x - 1, y - 1] + 1,
                    matrix[x, y - 1] + 1
                )
    # print(matrix)
    return matrix[size_x - 1, size_y - 1]


def clean_misspelled(df):
    uc = -1
    while True:
        ld = sorted(np.ndarray.tolist(df['Classification'].unique()))
        if uc == len(ld):
            return df
        else:
            uc = len(ld)
        print(len(ld))
        m = len(ld) - 1
        while True:
            changes = False
            print("Starting from " + str(m) + "...")
            for i in range(m, 0, -1):
                if 0 < levenshtein(ld[i], ld[i - 1]) <= 3:
                    if not changes:
                        m = i - 1
                        changes = True
                        df['Classification'] = df['Classification'].apply(lambda x: ld[i - 1] if x == ld[i] else x)
                        break
            if not changes:
                break


ds = pd.read_csv("data/dataset.csv")
# Initial metrics
ds['Classification'] = ds['Classification'].apply(lambda x: str(x))
print(len(ds['Classification'].unique()))
# Cleanup
ds['Classification'] = ds['Classification'].apply(lambda x: x.upper())
dc = clean_misspelled(ds)
dc.to_csv("data/dataset_clean_p1.csv", index=False)

