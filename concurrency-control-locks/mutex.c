#include<stdio.h>
#include <stdlib.h>
#include <time.h>
#include <pthread.h>
#include "linkedlist.h"
#include "timer.h"

#define MAX_NUM 65536

node_t *head = NULL;
int *func_array, *val_array;
pthread_mutex_t mutex;

int n, m;
float m_member, m_insert, m_delete;
long thread_count;


// Get arguments
void getargs(int argc, char *argv[]) {
    n = (int) strtol(argv[1], (char **) NULL, 10);
    m = (int) strtol(argv[2], (char **) NULL, 10);
    m_member = strtof(argv[3], (char **) NULL);
    m_insert = strtof(argv[4], (char **) NULL);
    m_delete = strtof(argv[5], (char **) NULL);
    thread_count = (int) strtol(argv[6], NULL, 10);
}

// Random populate
void populate(int n) {
    int i = 0;
    head = malloc(sizeof(node_t));
    int head_val = rand() % MAX_NUM;
    head->val = head_val;
    head->next = NULL;
    while (i < n - 1) {
        int num = rand() % MAX_NUM;
        if (n_member(num, head) == 0) {
            n_insert(num, &head);
            i++;
        }
    }
    func_array = malloc(m * sizeof(int));
    val_array = malloc(m * sizeof(int));
    long count_array[3] = {(long) (m * m_member), (long) (m * m_insert), (long) (m * m_delete)};
    i = 0;
    while (i < m) {
        int choice = (rand() % 3);
        if (count_array[choice] > 0) {
            val_array[i] = (rand() % MAX_NUM);
            func_array[i] = choice;
            count_array[choice]--;
            i++;
        }
    }
}

void * process(void *rank) {
    long my_rank = (long) rank;
    long my_m = m / thread_count;
    long my_first_i = my_m * my_rank;
    long my_last_i = my_first_i + my_m;

    for (long i = my_first_i; i < my_last_i; i++) {
        int choice = func_array[i];
        int val = val_array[i];
        if (choice == 0) {
            pthread_mutex_lock(&mutex);
            n_member(val, head);
            pthread_mutex_unlock(&mutex);
        } else if (choice == 1) {
            pthread_mutex_lock(&mutex);
            n_insert(val, &head);
            pthread_mutex_unlock(&mutex);
        } else if (choice == 2) {
            pthread_mutex_lock(&mutex);
            n_delete(val, &head);
            pthread_mutex_unlock(&mutex);
        }
    }
    return NULL;
}

double run(int argc, char *argv[]) {
    pthread_t *thread_handles;
    time_t t;
    double start, finish, elapsed;

    getargs(argc, argv);

    thread_handles = (pthread_t *) malloc(thread_count * sizeof(pthread_t));
    pthread_mutex_init(&mutex, NULL);

    srand((unsigned) time(&t));
    populate(n);

    GET_TIME(start);
    for (int thread = 0; thread < thread_count; thread++) {
        pthread_create(&thread_handles[thread], NULL, process, (void *) thread);
    }

    for (int thread = 0; thread < thread_count; thread++) {
        pthread_join(thread_handles[thread], NULL);
    }
    GET_TIME(finish);

    pthread_mutex_destroy(&mutex);
    free(thread_handles);

    elapsed = (finish - start) * 1000;
    printf("%f ms\n", elapsed);
    return elapsed;
}

int main(int argc, char *argv[]) {
    int iterations = 10;
    double *readings = malloc(iterations * sizeof(double));

    for (int i = 0; i < iterations; ++i) {
        double reading = run(argc, argv);
        readings[i] = reading;
    }

    double sum = 0;
    for (int j = 0; j < iterations; ++j) {
        sum += readings[j];
    }
    double avg = sum / iterations;

    double sq_sum = 0;
    for (int j = 0; j < iterations; ++j) {
        sq_sum += pow(readings[j] - avg, 2);
    }
    double std = sqrt(sq_sum / iterations);

    printf("Avg: %f\nStd: %f", avg, std);
    return 0;
}
