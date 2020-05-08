cmake_minimum_required(VERSION 3.9)
project(lab4 C)

set(CMAKE_C_STANDARD 99)
set(CMAKE_C_COMPILER /usr/local/bin/gcc-7)
set(CMAKE_C_FLAGS "${CMAKE_CXX_FLAGS} -fopenmp")

add_executable(lab4 matrix.c matrix.h data.c data.h time.c time.h lab4_optimized.c)