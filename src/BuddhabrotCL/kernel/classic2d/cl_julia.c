__kernel void julia(
	const float reMin,
	const float reMax,
	const float imMin,
	const float imMax,
	const uint  minIter,
	const uint  maxIter,
	const uint  width,
	const uint  height,
	const float escapeOrbit,
	const uint4 minColor,
	const uint4 maxColor,
	const uint isgrayscale,
	const uint hackMode,
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

	const float deltaRe = (reMax - reMin);
	const float deltaIm = (imMax - imMin);

	float2 p = (float2)(mix(reMin, reMax, rand.x), mix(imMin, imMax, rand.y));
	float2 z = p;
	float2 c = (float2)(-0.75, 0.27015);

	int iter = 0;

	while ((iter < maxIter) && ((z.x * z.x + z.y * z.y) < escapeOrbit))
	{
		z = (float2)(z.x * z.x - z.y * z.y, (z.x * z.y * 2.0)) + c;
		iter++;
	}

	int x = (width * (p.x - reMin) / deltaRe);
	int y = height - (height * (p.y - imMin) / deltaIm);

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