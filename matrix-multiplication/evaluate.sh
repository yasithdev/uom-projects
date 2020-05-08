#!/bin/bash

# Initial matrix size
mat_size=200
# Minimum matrix size
max_size=2000

# Create Blank CSV to store readings and write headers
mkdir out
filename="readings_$(date '+%Y%m%d').csv"
touch out/${filename}
echo "Size,Iteration,Sequential(ms),Parallel(ms),Optimized(ms)"
# Compile C code
gcc -o out/lab4_sequential.out -fopenmp lab4_sequential.c data.c matrix.c time.c
gcc -o out/lab4_parallel.out   -fopenmp lab4_parallel.c   data.c matrix.c time.c
gcc -o out/lab4_optimized.out  -fopenmp lab4_optimized.c  data.c matrix.c time.c

# Execute for all matrix sizes
while [ ${mat_size} -le ${max_size} ]
do
	echo "Matrix Size : ${mat_size}x${mat_size}"
	# Get 100 readings for consistency
	i=1
	while [ ${i} -le 25 ]
	do
		# get times of execution
        time_s=$(out/lab4_sequential.out ${mat_size})
		time_p=$(out/lab4_parallel.out   ${mat_size})
		time_o=$(out/lab4_optimized.out  ${mat_size})
		# write output to CSV
		echo "$mat_size,$i,$time_s,$time_p,$time_o"
		echo "$mat_size,$i,$time_s,$time_p,$time_o" >> out/${filename}
		# Increment i by 1
		i=$((i+1))
	done
	# Decrement matrix size by 200
	mat_size=$((mat_size+200))
done