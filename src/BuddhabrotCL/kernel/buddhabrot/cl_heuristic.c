// (c) Nikolai Voronin 2011-2017
// https://github.com/nikvoronin/BuddhabrotCL

#include "rng/cl_taus88.h"
#include "cl_mandelbrot.h"

__kernel void HeuristicBuddhabrot(
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
	__global uint4* outputBuffer)
{
	uint id = get_global_id(0);

	float2 rand = cl_frand2(id, rngBuffer);

	float rew = reMax - reMin;
	float imh = imMax - imMin;
	uint jMax = (uint)(4.f / rew * 100.f);

	float rr = rew / width;
	float ri = imh / height;

	rew = 1.0f / rew;
	imh = 1.0f / imh;

	float2 c = (float2)(mix(-2.0f, 2.0f, rand.x), mix(-2.0f, 2.0f, rand.y));

	uint jLim = jMax * 10;
	float alpha = 0.0f;
	uint j = 0;
	uint atscr = 0, lastatscr;
	int iter = 0;
	float2 z = 0.0f;
	while ((j < jMax) && (j < jLim))
	{
		lastatscr = atscr;
		atscr = 0;
		iter = 0;
		z = 0.0f;

		if (cl_inside_mandelbrot(c, minIter, maxIter, escapeOrbit))
		{
			int x, y;
			iter = 0;
			z = 0.0f;
			int i;

			// Evaluate
			while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
			{
				z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;

				x = (z.x - reMin) * rew * width;
				y = height - (z.y - imMin) * imh * height;

				if ((iter > minIter) && (x > 0) && (y > 0) && (x < width) && (y < height))
				{
					atscr++;

					i = x + (y * width);

					outputBuffer[i].w++;

					if (iter <= limColor.x)
						outputBuffer[i].x++;
					else
						if ((iter > limColor.x) && (iter < limColor.y))
							outputBuffer[i].y++;
						else
							if (iter >= limColor.y)
								outputBuffer[i].z++;
				} // if

				iter++;
			} // while
		} // else if

		// heuristics
		if (!atscr)
			break;
		else
			if (atscr != lastatscr)
				if (atscr > lastatscr)
					jMax *= 1.2f;
				else
					jMax *= 0.8f;

		// Mutate
		float q = cl_frand(id, rngBuffer);
		alpha = q * 2.f * 3.1415926f;
		c.x += rr * cos(alpha);
		c.y += ri * sin(alpha);
		rr *= 1.5f;
		ri *= 1.5f;

		j++;
	} // while j
}