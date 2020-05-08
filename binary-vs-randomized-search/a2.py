# Author - P.Y.M. Jayawardana
# Index# - 140265D

import random
import sys
import time

array = []
search_elements = []


def timeit(f):
    s = int(round(time.time() * 1000))
    f()
    e = int(round(time.time() * 1000))
    print(str(e - s) + " ms")


def get_sorted_array(array_size: int, search_size: int):
    global array, search_elements
    array = random.sample(range(1, sys.maxsize), 10 ** array_size)
    array.sort()
    print("(array - OK) ", end='')

    existent_elements = random.sample(array, int(search_size * 0.7))
    nonexistent_elements = []

    while len(nonexistent_elements) < int(search_size * 0.3):
        t = random.randrange(sys.maxsize)
        if not binary_search(t, 0, len(array) - 1):
            nonexistent_elements += [t]

    search_elements = existent_elements + nonexistent_elements
    random.shuffle(search_elements)
    print("(search - OK) ", end='')


def binary_search(e, start, end):
    if start > end:
        return -1
    split = round((start + end) / 2)
    if array[split] == e:
        return split
    elif array[split] < e:
        return binary_search(e, start, split - 1)
    elif array[split] > e:
        return binary_search(e, split + 1, end)
    else:
        return -1


def randomized_search(e, start, end):
    if start > end:
        return -1
    split = random.randint(start, end)
    if array[split] == e:
        return split
    elif array[split] < e:
        return randomized_search(e, start, split - 1)
    elif array[split] > e:
        return randomized_search(e, split + 1, end)
    else:
        return -1


def binary_search_all():
    for i, e in enumerate(search_elements):
        binary_search(e, 0, len(array) - 1)


def randomized_search_all():
    for i, e in enumerate(search_elements):
        randomized_search(e, 0, len(array) - 1)


def cleanup():
    global array, search_elements
    del array
    del search_elements


for x in [5, 6, 7]:
    print("10 ^ " + str(x) + ":")
    print("\tPopulating arrays... ", end='')
    get_sorted_array(x, 1000)
    print("DONE!")
    print("\tBinary Search (Normal) > " + str(x), end='')
    timeit(binary_search_all)
    print("\tBinary Search (Randomized) > " + str(x), end='')
    timeit(randomized_search_all)
    print("Cleaning up... ", end='')
    cleanup()
    print("DONE!\n")
