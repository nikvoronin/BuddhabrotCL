﻿// (c) Nikolai Voronin 2011-2017
// https://github.com/nikvoronin/BuddhabrotCL

#include "rng/cl_taus88.h"

__kernel void Kaliset(
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
	const uint4 limColor,
	__global uint4* rngBuffer,
	__global uint4*  outputBuffer)
{
	int id = get_global_id(0);

	float2 rand = cl_frand2(id, rngBuffer);

	float2 p = (float2)(mix(reMin, reMax, rand.x), mix(imMin, imMax, rand.y));
	float2 z = p;
	float2 c = cc; //(float2)(1.83071, 1.63084);

	int iter = 0;

	const float r = 0;
	const float scale = -1.9231;

	while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
	{
		float m = dot(z, z);

		if (m < r) {
			z = fabs(z) / (r * r);
		}
		else {
			z = fabs(z) / m * scale;
		}
		
		z = z + c;

		iter++;
	}

	int x = (p.x - reMin) / (reMax - reMin) * width;
	int y = height - (p.y - imMin) / (imMax - imMin) * height;

	if ((x > 0) && (y > 0) && (x < width) && (y < height))
	{
		int i = x + y * width;

		float k = 1.0 / half_log(2.0);
		float hk = half_log(0.5) * k;
		uint color = 5 + iter - hk - half_log(half_log(sqrt(z.x*z.x + z.y*z.y))) * k;

		outputBuffer[i].w+= color;

		if (iter <= limColor.x)
			outputBuffer[i].x += color;
		else
			if ((iter > limColor.x) && (iter < limColor.y))
				outputBuffer[i].y += color;
			else
				if (iter >= limColor.y)
					outputBuffer[i].z += color;
	}
}