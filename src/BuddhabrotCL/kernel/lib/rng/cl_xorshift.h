#ifndef __RNG_ALREADY_PRESENT
#define __RNG_ALREADY_PRESENT

float2 cl_frand2(uint id, __global uint4* rngBuffer)
{
	uint s1 = rngBuffer[id].x;
	uint s2 = rngBuffer[id].y;

	uint st = s1 ^ (s1 << 11);
	s1 = s2;
	s2 = s2 ^ (s2 >> 19) ^ (st ^ (st >> 18));

	rngBuffer[id] = (uint4)(s1, s2, 0, 0);

	return (float2)((float)st / UINT_MAX, (float)s1 / UINT_MAX);
}

float cl_frand(uint id, __global uint4* rngBuffer)
{
	uint s1 = rngBuffer[id].x;
	uint s2 = rngBuffer[id].y;

	uint st = s1 ^ (s1 << 11);
	s1 = s2;
	s2 = s2 ^ (s2 >> 19) ^ (st ^ (st >> 18));

	rngBuffer[id] = (uint4)(s1, s2, 0, 0);

	return (float)st / UINT_MAX;
}
#endif