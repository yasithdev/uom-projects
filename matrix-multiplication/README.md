# matrix-multiplication

Matrix Multiplication, Sequential vs Parallel

Course - Concurrent Programming

## Instructions

### How to compile
```bash
gcc -o lab4_sequential.out -fopenmp lab4_sequential.c data.c matrix.c time.c
gcc -o lab4_parallel.out   -fopenmp lab4_parallel.c   data.c matrix.c time.c
gcc -o lab4_optimized.out  -fopenmp lab4_optimized.c  data.c matrix.c time.c
```

### How to execute after compiling
./lab4_<select_type>.out <matrix_size>

### Output
When executed, the program outputs the time taken to complete.

### Evaluation Process (on linux)
```bash
./evaluate.sh
```

This program outputs a set of readings to out/readings_<date>.csv in the format
	matrix_size,iteration_number,sequential_time,parallel_time,optimized_time
