// (c) Nikolai Voronin 2011-2017
// https://github.com/nikvoronin/BuddhabrotCL

#include "rng/cl_taus88.h"
#include "cl_mandelbrot.h"

__kernel void MetropolisHastings(
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
	__global uint4* outputBuffer)
{
	uint id = get_global_id(0);

	float2 rand = cl_frand2(id, rngBuffer);

	float rew = reMax - reMin;
	float imh = imMax - imMin;
	int jMax = (int)(log(4.f / rew + 1.f) * 300.f / log(maxIter + 1.f));

	float r1 = rew * 0.0001f;
	float r2 = imh * 0.1f;
	float logreim = -log(r2 / r1);

	rew = 1.0f / rew;
	imh = 1.0f / imh;

	float2 c = (float2)(mix(-2.0f, 2.0f, rand.x), mix(-2.0f, 2.0f, rand.y));

	int iter = 0;
	float2 z = 0.0f;
	iter = 0;
	z = 0.0f;

	// Metropolis-Hastings

	int x, y;
	int i;

	// WarmUp
	uint atscr = 0;
	while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
	{
		z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;

		x = (z.x - reMin) * rew * width;
		y = height - (z.y - imMin) * imh * height;

		if ((iter > minIter) && (x > 0) && (y > 0) && (x < width) && (y < height))
			atscr++;

		iter++;
	} // while

	float2 prev_c = c;
	float prev_contrib;
	if (iter)
		prev_contrib = atscr / (float)iter;
	else
		prev_contrib = 0.f;
	float prev_iter = iter;
	uint prev_atscr = atscr;
	for (int j = 0; j < jMax; j++)
	{
		// Mutate
		float2 mutc = prev_c;
		float q = mix(0.0f, 5.0f, cl_frand(id, rngBuffer));
		if (q < 4.0f)
		{
			float a = cl_frand(id, rngBuffer);
			float b = cl_frand(id, rngBuffer);
			float phi = a * 2.0f * 3.1415926f;
			float r = r2 * exp(logreim * b);

			mutc.x += r * cos(phi);
			mutc.y += r * sin(phi);
		}
		else
		{
			mutc.x = mix(-2.0f, 2.0f, cl_frand(id, rngBuffer));
			mutc.y = mix(-2.0f, 2.0f, cl_frand(id, rngBuffer));
		}

		// Test
		if (!cl_inside_mandelbrot(mutc, minIter, maxIter, escapeOrbit))
			continue;

		// Evaluate
		z = 0.0f;
		uint mut_iter = 0;
		uint mut_atscr = 0;
		while ((mut_iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
		{
			z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + mutc;

			x = (z.x - reMin) * rew * width;
			y = height - (z.y - imMin) * imh * height;

			if ((iter > minIter) && (x > 0) && (y > 0) && (x < width) && (y < height))
				mut_atscr++;

			mut_iter++;
		} // while

		if (!(mut_atscr && mut_iter))
			continue; // for j

		float mut_contrib = (float)mut_atscr / (float)mut_iter;

		// Transition probability
		float t1 = (1.f - (mut_iter - mut_atscr) / mut_iter) / (1.f - (prev_iter - prev_atscr) / prev_iter);
		float t2 = (1.f - (prev_iter - prev_atscr) / prev_iter) / (1.f - (mut_iter - mut_atscr) / mut_iter);
		float alpha = min(1.0f, exp(log(mut_contrib * t1) - log(prev_contrib * t2)));
		float rnd = cl_frand(id, rngBuffer);

		if (alpha > rnd)
		{
			// accept
			prev_contrib = mut_contrib;
			prev_iter = mut_iter;
			prev_atscr = mut_atscr;
			prev_c = mutc;

			// draw
			iter = 0;
			z = 0.0f;
			while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
			{
				z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + mutc;

				x = (z.x - reMin) * rew * width;
				y = height - (z.y - imMin) * imh * height;

				if ((iter > minIter) && (x > 0) && (y > 0) && (x < width) && (y < height))
				{
					i = x + (y * width);

					if (isgrayscale)
						outputBuffer[i].x++;
					else
						if ((iter >= minColor.x) && (iter < maxColor.x))
							outputBuffer[i].x++;
						else
							if ((iter >= minColor.y) && (iter < maxColor.y))
								outputBuffer[i].y++;
							else
								if ((iter >= minColor.z) && (iter < maxColor.z))
									outputBuffer[i].z++;
				} // if

				iter++;
			} // while
		} // if alpha
	} // for j
}