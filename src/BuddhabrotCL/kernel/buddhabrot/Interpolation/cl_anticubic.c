float bcube(
	float y0,
	float y1,
	float y2,
	float y3,
	float d)
{
	float a0, a1, a2, a3, d2;

	d2 = d * d;

	a0 = 0.5 * y3 - 1.5 * y2 - 0.5 * y0 + 1.5 * y1;
	a1 = y0 - 2.5 * y1 + 2 * y2 - 0.5 * y3;
	a2 = 0.5 * y2 - 0.5 * y0;
	a3 = y1;

	return (a0 * d * d2 + a1 * d2 + a2 * d + a3);
}

void line(
	float2 z0,
	float2 z1,
	float2 z2,
	float2 z3,
	uint width,
	uint height,
	__global uint4*  out,
	int p)
{
	int dx = abs((int)z2.x - (int)z1.x);
	int sx = z1.x < z2.x ? 1 : -1;
	float odx = 1.0f / (dx + 0.0000001f);

	int x = z1.x, y = z1.y;
	int oy = -1;
	int i = 0;
	while (i <= dx)
	{
		if ((x > 0) && (y > 0) && (x < width) && (y < height))
		{
			if (oy != y)
			{
				switch (p)
				{
				case 0:
					out[x + (y * width)].x++;
					break;
				case 1:
					out[x + (y * width)].y++;
					break;
				case 2:
					out[x + (y * width)].z++;
					break;
				}
			}
		}

		oy = y;
		x = z1.x + i * sx;
		y = bcube(z0.y, z1.y, z2.y, z3.y, odx * (float)i);
		i++;
	}
}

//Check if choosen point is in MSet
bool isInMSet(
	const float2 c,
	const uint minIter,
	const uint maxIter,
	const float escapeOrbit)
{
	int iter = 0;
	float2 z = 0.0;

	if (!(((c.x - 0.25)*(c.x - 0.25) + (c.y * c.y))*(((c.x - 0.25)*(c.x - 0.25) + (c.y * c.y)) + (c.x - 0.25)) < 0.25* c.y * c.y))  //main cardioid
	{
		if (!((c.x + 1.0) * (c.x + 1.0) + (c.y * c.y) < 0.0625))            //2nd order period bulb
		{
			if (!((((c.x + 1.309)*(c.x + 1.309)) + c.y*c.y) < 0.00345))    //smaller bulb left of the period-2 bulb
			{
				if (!((((c.x + 0.125)*(c.x + 0.125)) + (c.y - 0.744)*(c.y - 0.744)) < 0.0088))      // smaller bulb bottom of the main cardioid
				{
					if (!((((c.x + 0.125)*(c.x + 0.125)) + (c.y + 0.744)*(c.y + 0.744)) < 0.0088))  //smaller bulb top of the main cardioid
					{
						while ((iter < maxIter) && (z.x*z.x + z.y*z.y < escapeOrbit))       //Bruteforce check  
						{
							z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0)) + c;
							iter++;
						}

						if ((iter > minIter) && (iter < maxIter))
							return false;
					}
				}
			}
		}
	}
	return true;
}

float signum(float value)
{
	if (value > 0) return 1;
	if (value < 0) return -1;
	return 0;
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

	float2 c = (float2)(mix(-2, 2, rand.x), mix(-2, 2, rand.y));

	if (isInMSet(c, minIter, maxIter, escapeOrbit))
	{
		int x, y;
		float2 z0 = 0.0, z1 = 0.0, z2 = 0.0, z3 = 0.0;
		int zn = 0;
		int iter = 0;
		float2 z = 0.0;
		int i;

		while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
		{
			z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0)) + c;
			x = (z.x - reMin) / (reMax - reMin) * width;
			y = height - (z.y - imMin) / (imMax - imMin) * height;

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

				switch (zn)
				{
				case 0:
					z1 = (float2)((float)x, (float)y);
					zn = 1;
					break;
				case 1:
					z2 = (float2)((float)x, (float)y);
					zn = 2;
					break;
				case 2:
					z3 = (float2)((float)x, (float)y);
					zn = 3;
					break;
				case 3:
					z0 = z1;
					z1 = z2;
					z2 = z3;
					z3 = (float2)((float)x, (float)y);
					break;
				} // switch

				if (zn == 3 && p > -1)
					line(z0, z1, z2, z3, width, height, outputBuffer, p);
			} // if iter

			iter++;
		} // while
	} // if		
}