#include "rng/cl_taus88.h"

__kernel void burningship(
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
	float2 rand = random2(rngBuffer);

	float2 p = (float2)(mix(reMin, reMax, rand.x), mix(imMin, imMax, rand.y));
	float2 c = p;
	float2 z = 0.0;

	int iter = 0;
	
	while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
	{
		z = fabs(z);
		z = (float2)(z.x * z.x - z.y * z.y, z.x * z.y + z.y * z.x);
		z.y = -z.y;
		z += c;
		
		iter++;
	}

	int x = (p.x - reMin) / (reMax - reMin) * width;
	int y = height - (p.y - imMin) / (imMax - imMin) * height;

	if ((x > 0) && (y > 0) && (x < width) && (y < height))
	{
		int i = x + y * width;

		if (iter == maxIter)
			outputBuffer[x + y * width].x = 0;
		else
		{
			float k = 1.0 / half_log(2.0);
			float hk = half_log(0.5) * k;
			uint color = 5 + iter - hk - half_log(half_log(sqrt(z.x*z.x + z.y*z.y))) * k;

			if (isgrayscale)
				outputBuffer[i].x += color;
			else
				if ((iter > minColor.x) && (iter < maxColor.x))
					outputBuffer[i].x += color;
				else
					if ((iter > minColor.y) && (iter < maxColor.y))
						outputBuffer[i].y += color;
					else
						if ((iter > minColor.z) && (iter < maxColor.z))
							outputBuffer[i].z += color;
		}
	}
}