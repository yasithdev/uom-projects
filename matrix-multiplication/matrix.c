#include <stdlib.h>
#include "matrix.h"

#define MAT_THREADS 8


void matAlloc(double **mat) {
#pragma omp parallel for num_threads(MAT_THREADS) shared(mat)
    for (int i = 0; i < N; ++i) {
        mat[i] = malloc(N * sizeof(double));
    }
}

void matFill(double **mat) {
#pragma omp parallel for num_threads(MAT_THREADS) shared(mat)
    for (int i = 0; i < N; ++i) {
        for (int j = 0; j < N; ++j) {
            mat[i][j] = (double)rand();
        }
    }
}

void matFree(double **mat) {
#pragma omp parallel for num_threads(MAT_THREADS) shared(mat)
    for (int i = 0; i < N; ++i) {
        free(mat[i]);
    }
    free(mat);
}

double **matTrans(double **mat) {
    double **mat_trans = malloc(N * sizeof(double *));
    matAlloc(mat_trans);
#pragma omp parallel for num_threads(MAT_THREADS) shared(mat, mat_trans)
    for (int i = 0; i < N; ++i) {
        for (int j = 0; j < N; ++j) {
            mat_trans[i][j] = mat[j][i];
        }
    }
    return mat_trans;
}