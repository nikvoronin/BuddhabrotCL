__kernel void xorshift(
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

	uint s1 = rngBuffer[id].x;
	uint s2 = rngBuffer[id].y;

	uint st = s1 ^ (s1 << 11);
	s1 = s2;
	s2 = s2 ^ (s2 >> 19) ^ (st ^ (st >> 18));
	
	rngBuffer[id] = (uint4)(s1, s2, 0, 0);
	float2 rand = (float2)((float)st / UINT_MAX, (float)s1 / UINT_MAX);

	float2 c = (float2)(mix(0.0f, width - 1.0f, rand.x), mix(0.0f, height - 1.0f, rand.y));
	uint i = (uint)(c.x + c.y * width);

	outputBuffer[i].x += 1;
	outputBuffer[i].y += 1;
	outputBuffer[i].z += 1;
}