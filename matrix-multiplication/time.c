#include "time.h"


void markStartTime() {
    gettimeofday(&start, NULL);
}

void markStopTime() {
    gettimeofday(&stop, NULL);
}

int getMillis() {
    return (int) ((double) (stop.tv_usec - start.tv_usec) / 1000 + (double) (stop.tv_sec - start.tv_sec) * 1000 + 0.5);
}