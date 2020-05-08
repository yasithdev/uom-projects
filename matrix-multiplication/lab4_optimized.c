#include <stdio.h>
#include "matrix.h"
#include "data.h"
#include "time.h"

#define NUM_THREADS 8

int main(int argc, char **argv) {
    N = (int) strtol(argv[1], NULL, 10);
    init();
//    Second matrix is transposed
    double **mat2_trans = matTrans(mat2);
//    Number of threads were defined according to available number of physical (2) and logical (4) cores
//    OpenMP scheduling was set to static because all threads have same workload
    markStartTime();
#pragma omp parallel for num_threads(NUM_THREADS)
    // Loop nest optimization. Calculate two rows and two columns in one iteration
    for (int i = 0; i < N; i += 2) {
        // Preload mat1 rows
        double *mat1_row0 = mat1[i];
        double *mat1_row1 = mat1[i + 1];
        double sum00, sum01, sum10, sum11;
        // Iterate
        for (int j = 0; j < N; j += 2) {
            // Preload mat2_trans rows
            double *mat2_col0 = mat2_trans[j];
            double *mat2_col1 = mat2_trans[j + 1];
            // Initialize sums to zero
            sum00 = 0, sum01 = 0, sum10 = 0, sum11 = 0;
            for (int k = 0; k < N; k += 2) {
                // Since pointer anyways increments by 1, change to ++. Group multiple commands together to reduce
                // for loop iterations
                sum00 += mat1_row0[k] * mat2_col0[k] + mat1_row0[k + 1] * mat2_col0[k + 1];
                sum01 += mat1_row0[k] * mat2_col1[k] + mat1_row0[k + 1] * mat2_col1[k + 1];
                sum10 += mat1_row1[k] * mat2_col0[k] + mat1_row1[k + 1] * mat2_col0[k + 1];
                sum11 += mat1_row1[k] * mat2_col1[k] + mat1_row1[k + 1] * mat2_col1[k + 1];
            }
            mat3[i][j] = sum00;
            mat3[i][j + 1] = sum01;
            mat3[i + 1][j] = sum10;
            mat3[i + 1][j + 1] = sum11;
        }
    }
    markStopTime();
    cleanup();
    printf("%d\n", getMillis());
    return 0;
}