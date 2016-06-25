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