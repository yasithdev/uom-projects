#ifndef LAB4_TIME_H
#define LAB4_TIME_H

#include <sys/time.h>

struct timeval stop, start;

void markStartTime();

void markStopTime();

int getMillis();

#endif //LAB4_TIME_H
