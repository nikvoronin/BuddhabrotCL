// (c) Nikolai Voronin 2011-2017
// https://github.com/nikvoronin/BuddhabrotCL

#include "rng/cl_taus88.h"
#include "cl_mandelbrot.h"

__kernel void NaiveBuddhabrot(
    const float reMin,
    const float reMax,
    const float imMin,
    const float imMax,
    const uint  minIter,
    const uint  maxIter,
    const uint  width,
    const uint  height,
    const float escapeOrbit,
	const float2 cc,
	const uint4 minColor,
    const uint4 maxColor,
    const uint isgrayscale,
	__global uint4* rngBuffer,
	__global uint4*  outputBuffer)
{
	int id = get_global_id(0);

	float2 rand = cl_frand2(id, rngBuffer);

	float2 c = (float2)(mix(-2.0f, 2.0f, rand.x), mix(-2.0f, 2.0f, rand.y));

	if (cl_inside_mandelbrot(c, minIter, maxIter, escapeOrbit))
	{
		int x, y;
		int iter = 0;
		float2 z = 0.0f;
		int i;

		while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
		{
			z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;
			x = (z.x - reMin) / (reMax - reMin) * width;
			y = height - (z.y - imMin) / (imMax - imMin) * height;

			if ((iter > minIter) && (x > 0) && (y > 0) && (x < width) && (y < height))
			{
				i = x + (y * width);

				if (isgrayscale)
					outputBuffer[i].x++;
				else
					if ((iter > minColor.x) && (iter < maxColor.x))
						outputBuffer[i].x++;
					else
						if ((iter > minColor.y) && (iter < maxColor.y))
							outputBuffer[i].y++;
						else
							if ((iter > minColor.z) && (iter < maxColor.z))
								outputBuffer[i].z++;
			} // if

			iter++;
		} // while
	} // if
}