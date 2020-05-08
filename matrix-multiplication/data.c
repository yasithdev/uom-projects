#include <sys/time.h>
#include "data.h"

void init() {
    // Set new seed value
    srand((unsigned int) time(0));
    // Allocate matrices
    mat1 = malloc(N * sizeof(double *));
    mat2 = malloc(N * sizeof(double *));
    mat3 = malloc(N * sizeof(double *));
    matAlloc(mat1);
    matAlloc(mat2);
    matAlloc(mat3);
    // Populate mat1 and mat2 with random values
    matFill(mat1);
    matFill(mat2);
}

void cleanup() {
    // Free resources
    matFree(mat1);
    matFree(mat2);
    matFree(mat3);
}




