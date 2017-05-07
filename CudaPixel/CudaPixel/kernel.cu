
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>#
#include <iostream>

static unsigned char *texels;
static int width, height;

static void readBmp(char *filename)
{
	FILE *fd;
	fd = fopen(filename, "rb");
	if (fd == NULL)
	{
		printf("Error: fopen failed\n");
		return;
	}

	unsigned char header[54];

	// Read header
	fread(header, sizeof(unsigned char), 54, fd);

	// Capture dimensions
	width = *(int*)&header[18];
	height = *(int*)&header[22];

	int padding = 0;

	// Calculate padding
	while ((width * 3 + padding) % 4 != 0)
	{
		padding++;
	}

	// Compute new width, which includes padding
	int widthnew = width * 3 + padding;

	// Allocate memory to store image data (non-padded)
	texels = (unsigned char *)malloc(width * height * 3 * sizeof(unsigned char));
	if (texels == NULL)
	{
		printf("Error: Malloc failed\n");
		return;
	}

	// Allocate temporary memory to read widthnew size of data
	unsigned char* data = (unsigned char *)malloc(widthnew * sizeof(unsigned int));

	// Read row by row of data and remove padded data.
	for (int i = 0; i<height; i++)
	{
		// Read widthnew length of data
		fread(data, sizeof(unsigned char), widthnew, fd);

		// Retain width length of data, and swizzle RB component.
		// BMP stores in BGR format, my usecase needs RGB format
		for (int j = 0; j < width * 3; j += 3)
		{
			int index = (i * width * 3) + (j);
			texels[index + 0] = data[j + 2];
			texels[index + 1] = data[j + 1];
			texels[index + 2] = data[j + 0];
		}
	}

	free(data);
	fclose(fd);
}





cudaError_t addWithCuda(int *c, const int *a, const int *b, unsigned int size);

__global__ void addKernel(int *c, const int *a, const int *b)
{
    int i = threadIdx.x;
    c[i] = a[i] + b[i];
}

int main()
{
	printf("Start reading image\n");
	readBmp("cEdDG.bmp");
	printf("Finished");
	printf("%s", texels);
	scanf("%d");
	return 0;
}

// Helper function for using CUDA to add vectors in parallel.

