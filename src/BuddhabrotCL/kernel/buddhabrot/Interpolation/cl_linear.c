﻿void line(
	int x0,
	int y0,
	int x1,
	int y1,
	uint width,
	uint height,
	__global uint4*  out,
	int p)
{
	int dx = abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
	int dy = abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
	int err = (dx > dy ? dx : -dy) / 2, e2;

	for (;;)
	{
		if((x0 > 0) && (y0 > 0) && (x0 < width) && (y0 < height))
			switch (p)
			{
			case 0:
				out[x0 + (y0 * width)].x++;
				break;
			case 1:
				out[x0 + (y0 * width)].y++;
				break;
			case 2:
				out[x0 + (y0 * width)].z++;
				break;
			}

		if (x0 == x1 && y0 == y1) break;

		e2 = err;

		if (e2 >-dx) { err -= dy; x0 += sx; }
		if (e2 < dy) { err += dx; y0 += sy; }
	}
}

uint isInMSet(
	const float2 c,
	const uint minIter,
	const uint maxIter,
	const float escapeOrbit)
{
	int iter = 0;
	float2 z = 0.0f;

	if (!(((c.x - 0.25f)*(c.x - 0.25f) + (c.y * c.y))*(((c.x - 0.25f)*(c.x - 0.25f) + (c.y * c.y)) + (c.x - 0.25f)) < 0.25f* c.y * c.y))  //main cardioid
	{
		if (!((c.x + 1.0f) * (c.x + 1.0f) + (c.y * c.y) < 0.0625f))            //2nd order period bulb
		{
			if (!((((c.x + 1.309f)*(c.x + 1.309f)) + c.y*c.y) < 0.00345f))    //smaller bulb left of the period-2 bulb
			{
				if (!((((c.x + 0.125f)*(c.x + 0.125f)) + (c.y - 0.744f)*(c.y - 0.744f)) < 0.0088f))      // smaller bulb bottom of the main cardioid
				{
					if (!((((c.x + 0.125f)*(c.x + 0.125f)) + (c.y + 0.744f)*(c.y + 0.744f)) < 0.0088f))  //smaller bulb top of the main cardioid
					{
						while ((iter < maxIter) && (z.x*z.x + z.y*z.y < escapeOrbit))       //Bruteforce check  
						{
							z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;
							iter++;
						}

						if ((iter > minIter) && (iter < maxIter))
							return 0;
					}
				}
			}
		}
	}
	return 1;
}

__kernel void buddhabrot(
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

	// taus88
	uint s1 = rngBuffer[id].x;
	uint s2 = rngBuffer[id].y;
	uint s3 = rngBuffer[id].z;

	float2 rand;

	uint b;
	b = (((s1 << 13) ^ s1) >> 19);
	s1 = (((s1 & 4294967294) << 12) ^ b);
	b = (((s2 << 2) ^ s2) >> 25);
	s2 = (((s2 & 4294967288) << 4) ^ b);
	b = (((s3 << 3) ^ s3) >> 11);
	s3 = (((s3 & 4294967280) << 17) ^ b);

	rand.x = (float)((s1 ^ s2 ^ s3) * 2.3283064365e-10);

	b = (((s1 << 13) ^ s1) >> 19);
	s1 = (((s1 & 4294967294) << 12) ^ b);
	b = (((s2 << 2) ^ s2) >> 25);
	s2 = (((s2 & 4294967288) << 4) ^ b);
	b = (((s3 << 3) ^ s3) >> 11);
	s3 = (((s3 & 4294967280) << 17) ^ b);

	rand.y = (float)((s1 ^ s2 ^ s3) * 2.3283064365e-10);

	rngBuffer[id] = (uint4)(s1, s2, s3, b);

	float2 c = (float2)(mix(-2.0f, 2.0f, rand.x), mix(-2.0f, 2.0f, rand.y));

	if (!isInMSet(c, minIter, maxIter, escapeOrbit))
	{
		int x1, y1;
		int x0 = -1;
		int y0 = -1;
		int iter = 0;
		float2 z = 0.0f;
		int i;

		while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
		{
			z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0f)) + c;
			x1 = (z.x - reMin) / (reMax - reMin) * width;
			y1 = height - (z.y - imMin) / (imMax - imMin) * height;

			if (iter > minIter)
			{
				int p = -1;
				if ((iter > minColor.x) && (iter < maxColor.x))
					p = 0;
				else
					if ((iter > minColor.y) && (iter < maxColor.y))
						p = 1;
					else
						if ((iter > minColor.z) && (iter < maxColor.z))
							p = 2;

				if (x0 > -1 && y0 > -1 && p > -1)
					line(x0, y0, x1, y1, width, height, outputBuffer, p);

				x0 = x1; y0 = y1;
			} // if

			iter++;
		} // while
	} // if		
}