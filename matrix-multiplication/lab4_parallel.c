#include <stdio.h>
#include "data.h"
#include "time.h"

int main(int argc, char **argv) {
    N = (int) strtol(argv[1], NULL, 10);
    init();
    markStartTime();
#pragma omp parallel for
    for (int i = 0; i < N; ++i) {
        for (int j = 0; j < N; ++j) {
            mat3[i][j] = 0;
            for (int k = 0; k < N; ++k) {
                mat3[i][j] += mat1[i][k] * mat2[k][j];
            }
        }
    }
    markStopTime();
    cleanup();
    printf("%d\n", getMillis());
    return 0;
}